using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Web.Security;

using com.hujun64.po;
using SharedWebClassLibrary.IpAddressSearchWebService;
using SharedWebClassLibrary.MobileCodeWS;
using SharedWebClassLibrary.WeatherWS;
using SharedWebClassLibrary.SmsService;
using SharedWebClassLibrary.shfclawyer;
using SharedWebClassLibrary.hujun64;

namespace com.hujun64.util
{
    /// <summary>
    ///MailSender 的摘要说明
    /// </summary>
    public class InternetWebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("InternetWebService");
        private static IpAddressSearchWebService ipService = new IpAddressSearchWebService();
        private static MobileCodeWS mobileService = new MobileCodeWS();
        private static WeatherWS weatherService = new WeatherWS();
        private static SmsService smsService = new SmsService();


        private static string SHFCLAWYER_USER_NAME="admin";
        private static string SHFCLAWYER_PASSWORD = FormsAuthentication.HashPasswordForStoringInConfigFile("admin888", "MD5");

        private static string HUJUN64_USER_NAME="admin";
        private static string HUJUN64_PASSWORD=FormsAuthentication.HashPasswordForStoringInConfigFile("admin888", "MD5");

        private static ShfclawyerService shfclawyerWebService = new ShfclawyerService();
        private static Hujun64Service hujun64WebService = new Hujun64Service();

        static InternetWebService()
        {
            //代理服务器设置
            if (Total.EnableWSProxy)
            {
                smsService.Proxy = UtilHttp.GetWebProxyInstance();
                weatherService.Proxy = UtilHttp.GetWebProxyInstance();
                mobileService.Proxy = UtilHttp.GetWebProxyInstance();
                ipService.Proxy = UtilHttp.GetWebProxyInstance();
            }
        }

        public InternetWebService()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        //IP地址归属地
        public static string GetCountryCityByIp(string ip)
        {
            try
            {
                string param = ip;
                if(!string.IsNullOrEmpty(Total.WebxmlUserId))
                    param = param + ":" + Total.WebxmlUserId;

                return ipService.getCountryCityByIp(param)[1];
            }
            catch (Exception ex)
            {
                log.Error("找不到IP地址的相应地域:" + ip, ex);

                return null;
            }
        }


        //手机号码归属地
        public static string GetCountryCityByMobile(string mobileCode)
        {
            try
            {
                return mobileService.getMobileCodeInfo(mobileCode, "");
            }
            catch (Exception ex)
            {
                log.Error("找不到手机号码的相应地域:" + mobileCode, ex);

                return null;
            }
        }

        public static int SmsQuery()
        {
            string timestamp = DateTime.Now.ToString("MMddHHmm");
            string hasdedPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Total.SmsPassword + timestamp, "MD5").ToLower();
            int ret = smsService.query(Total.SmsUsername, hasdedPassword, timestamp);

            if (ret == -1)
                throw new ApplicationException("验证失败！");
            else
                return ret;

        }

        public static bool SmsChangePassword(string newPassword)
        {
            string timestamp = DateTime.Now.ToString("MMddHHmm");
            string hasdedPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Total.SmsPassword + timestamp, "MD5").ToLower();
            int ret = smsService.changepw(Total.SmsUsername, hasdedPassword, timestamp, newPassword);
            if (ret == 0)
            {
                return true;
            }
            else
                if (ret == -1)
                    throw new ApplicationException("验证失败！");
                else
                    return
                        false;
        }
        //发送短信
        /*******************************
         * 提交短信(一次最多100条)
         * 参数 描述 
         * name 用户名 
         * hashedpass 经过MD5加密后的密码 
         * timestamp 时间戳字符串,用于密码的加密 
         * subphone 目标手机号码,发送多个时号码时用","隔开,出现非号码字符将被过滤 
         * msgsub 发送的信息,内容不能超过70个字，不要含有换行符 
         * 
         * 返回状态(若大于0,为提交成功的信息条数) 
         * -1 验证失败 
         * -4 用户余额不足 
         * -5 短信内容长度为0或超过长度或提交的号码数超过100 
         * 
         * MD5加密算法
         * 算法为hashedpass=MD5(密码+timestamp),其中MD5表示单向哈希算法(例如timestamp="08201535",密码="123456"时,通过MD5算法得出hashedpass="7ee8b9960c4d09e06cfc2b77adac0214")
         * timestamp(时间戳)的格式为"MMDDHHmm"如2007年8月20日15时35分的时间戳为08201535 
         * 
         * **************************************/
        public static int SmsSend(string msg, List<string> mobileCodeList)
        {

            string timestamp = DateTime.Now.ToString("MMddHHmm");

            int iSuccessed = 0;
            try
            {
                string hasdedPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Total.SmsPassword + timestamp, "MD5").ToLower();



                List<string> mobileToSendList = new List<string>();
                StringBuilder mobileSb = new StringBuilder();
                int n = 0;
                int loopTimes = 0;
                string msgContent = UtilHtml.RemoveHtmlTag(UtilHtml.RemoveEnterBreak(msg)); ;
                foreach (string mobileCode in mobileCodeList)
                {
                    mobileSb.Append(mobileCode);
                    mobileSb.Append(",");
                    n++;

                    if (n == Total.MaxSmsMobileSize || (n + loopTimes*Total.MaxSmsMobileSize) == mobileCodeList.Count)
                    {
                        mobileToSendList.Add(mobileSb.ToString().Remove(mobileSb.Length - 1));
                        mobileSb.Remove(0, mobileSb.Length);
                        n = 0;
                        loopTimes++;

                    }


                }
                
                foreach (string mobileToSend in mobileToSendList)
                {

                    int ret = smsService.submitmsg(Total.SmsUsername, hasdedPassword, timestamp, mobileToSend, msgContent);

                    if (ret > 0)
                        iSuccessed += ret;
                    else
                    {
                        switch (ret)
                        {
                            case -1:
                                throw new ApplicationException("验证失败！");

                            case -4:
                                throw new ApplicationException("用户余额不足！");

                            case -5:
                                throw new ApplicationException("短信内容长度为0或超过长度或提交的号码数超过100！");

                            default:
                                break;
                        }
                    }
                }
                return iSuccessed;


            }
            catch (Exception ex)
            {
                log.Error("发送短信出错！", ex);
                throw ex;
            }
        }


        //天气预报

        /***************
         * 
         * webService的getWeather 方法 
         * 获得天气预报数据，（输入有效 theUserID 用户ID参数可获得七天的天气预报数据） 
         * 输入参数： 参数名：theUserID 用户ID，免费用户填写空字符串 theUserID = ""。
         * 参数名：theCityCode ，类型：字符串，输入内容可以为以下数据之一（可通过 getSupportCityDataset 或 getSupportCityString 方法获得）：
         * 1. 城市或地区中文名称
         * 2. 城市或地区 ID
         * 3. 参数为空字符串默认“上海”
         * 返回数据：一维字符串数组 String()，结构如下： 
         * Array(0) = "省份 地区/洲 国家名（国外）" 
         * Array(1) = "查询的天气预报地区名称" 
         * Array(2) = "查询的天气预报地区ID" 
         * Array(3) = "最后更新时间 格式：yyyy-MM-dd HH:mm:ss" 
         * Array(4) = "当前天气实况：气温、风向/风力、湿度" 
         * Array(5) = "第一天 空气质量、紫外线强度" 
         * Array(6) = "第一天 天气和生活指数" 
         * Array(7) = "第一天 概况 格式：M月d日 天气概况" 
         * Array(8) = "第一天 气温" 
         * Array(9) = "第一天 风力/风向" 
         * Array(10) = "第一天 天气图标 1" 
         * Array(11) = "第一天 天气图标 2" 
         * Array(12) = "第二天 概况 格式：M月d日 天气概况" 
         * Array(13) = "第二天 气温" 
         * Array(14) = "第二天 风力/风向" 
         * Array(15) = "第二天 天气图标 1"
         * Array(16) = "第二天 天气图标 2" ...... ......
         * 
         * 每一天的格式同：
         * Array(12) -- Array(16) ...... Array(n-4) = "最后一天 概况 格式：M月d日 天气概况" 
         * Array(n-3) = "最后一天 气温" 
         * Array(n-2) = "最后一天 风力/风向" 
         * Array(n-1) = "最后一天 天气图标 1" 
         * Array(n) = "最后一天 天气图标 2"
         * 
         * */
        public static Weather GetWeather()
        {
            //string ShanghaiCity="上海,31112";


            try
            {
                Weather weather;
                string[] weatherArray = weatherService.getWeather("", "");
                if (weatherArray == null || weatherArray.Length == 0)
                    weather= new  Weather();
                else
                    weather = new Weather(weatherArray);

                return weather;

            }
            catch (Exception ex)
            {
                log.Error("查询天气出错！", ex);

                return new Weather();
            }
        }


        /*****
        * 
        * shfclawyer web service
        * 
        * **/
        
        public static bool PostGuestbook2Shfclawyer(com.hujun64.po.Guestbook guestbook)
        {

            SharedWebClassLibrary.shfclawyer.Guestbook wsGuestbook = new SharedWebClassLibrary.shfclawyer.Guestbook();
            Util.PropertyCopy(guestbook, ref wsGuestbook);
            return shfclawyerWebService.PostGuestbook(SHFCLAWYER_USER_NAME, SHFCLAWYER_PASSWORD, wsGuestbook);
        }
        public static bool DeleteGuestbook4Shfclawyer(String guestbookId)
        {
            return shfclawyerWebService.DeleteGuestbook(SHFCLAWYER_USER_NAME, SHFCLAWYER_PASSWORD, guestbookId);
        }


        public static bool PostArticle2Shfclawyer(com.hujun64.po.Article article)
        {

            SharedWebClassLibrary.shfclawyer.Article wsArticle = new SharedWebClassLibrary.shfclawyer.Article();
            Util.PropertyCopy(article, ref wsArticle);
            return shfclawyerWebService.PostArticle(SHFCLAWYER_USER_NAME, SHFCLAWYER_PASSWORD, wsArticle);
        }
        public static bool DeleteArticle4Shfclawyer(String articleId)
        {
            return shfclawyerWebService.DeleteArticle(SHFCLAWYER_USER_NAME, SHFCLAWYER_PASSWORD, articleId);
        }

        /*****
         * 
         * hujun64 web service
         * 
         * **/

        public static bool PostGuestbook2Hujun64(com.hujun64.po.Guestbook guestbook)
        {

            SharedWebClassLibrary.hujun64.Guestbook wsGuestbook = new SharedWebClassLibrary.hujun64.Guestbook();
            Util.PropertyCopy(guestbook, ref wsGuestbook);
            return hujun64WebService.PostGuestbook(HUJUN64_USER_NAME, HUJUN64_PASSWORD, wsGuestbook);
        }
        public static bool DeleteGuestbook4Hujun64(String guestbookId)
        {

            return hujun64WebService.DeleteGuestbook(HUJUN64_USER_NAME, HUJUN64_PASSWORD, guestbookId);
        }
        public static bool PostArticle2Hujun64(com.hujun64.po.Article article)
        {

            SharedWebClassLibrary.hujun64.Article wsArticle = new SharedWebClassLibrary.hujun64.Article();
            Util.PropertyCopy(article, ref wsArticle);
            return hujun64WebService.PostArticle(HUJUN64_USER_NAME, HUJUN64_PASSWORD, wsArticle);
        }
        public static bool DeleteArticle4Hujun64(String articleId)
        {

            return hujun64WebService.DeleteArticle(HUJUN64_USER_NAME, HUJUN64_PASSWORD, articleId);
        }
    }
}