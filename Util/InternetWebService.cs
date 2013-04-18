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
    ///MailSender ��ժҪ˵��
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
            //�������������
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
            //TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        //IP��ַ������
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
                log.Error("�Ҳ���IP��ַ����Ӧ����:" + ip, ex);

                return null;
            }
        }


        //�ֻ����������
        public static string GetCountryCityByMobile(string mobileCode)
        {
            try
            {
                return mobileService.getMobileCodeInfo(mobileCode, "");
            }
            catch (Exception ex)
            {
                log.Error("�Ҳ����ֻ��������Ӧ����:" + mobileCode, ex);

                return null;
            }
        }

        public static int SmsQuery()
        {
            string timestamp = DateTime.Now.ToString("MMddHHmm");
            string hasdedPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Total.SmsPassword + timestamp, "MD5").ToLower();
            int ret = smsService.query(Total.SmsUsername, hasdedPassword, timestamp);

            if (ret == -1)
                throw new ApplicationException("��֤ʧ�ܣ�");
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
                    throw new ApplicationException("��֤ʧ�ܣ�");
                else
                    return
                        false;
        }
        //���Ͷ���
        /*******************************
         * �ύ����(һ�����100��)
         * ���� ���� 
         * name �û��� 
         * hashedpass ����MD5���ܺ������ 
         * timestamp ʱ����ַ���,��������ļ��� 
         * subphone Ŀ���ֻ�����,���Ͷ��ʱ����ʱ��","����,���ַǺ����ַ��������� 
         * msgsub ���͵���Ϣ,���ݲ��ܳ���70���֣���Ҫ���л��з� 
         * 
         * ����״̬(������0,Ϊ�ύ�ɹ�����Ϣ����) 
         * -1 ��֤ʧ�� 
         * -4 �û����� 
         * -5 �������ݳ���Ϊ0�򳬹����Ȼ��ύ�ĺ���������100 
         * 
         * MD5�����㷨
         * �㷨Ϊhashedpass=MD5(����+timestamp),����MD5��ʾ�����ϣ�㷨(����timestamp="08201535",����="123456"ʱ,ͨ��MD5�㷨�ó�hashedpass="7ee8b9960c4d09e06cfc2b77adac0214")
         * timestamp(ʱ���)�ĸ�ʽΪ"MMDDHHmm"��2007��8��20��15ʱ35�ֵ�ʱ���Ϊ08201535 
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
                                throw new ApplicationException("��֤ʧ�ܣ�");

                            case -4:
                                throw new ApplicationException("�û����㣡");

                            case -5:
                                throw new ApplicationException("�������ݳ���Ϊ0�򳬹����Ȼ��ύ�ĺ���������100��");

                            default:
                                break;
                        }
                    }
                }
                return iSuccessed;


            }
            catch (Exception ex)
            {
                log.Error("���Ͷ��ų���", ex);
                throw ex;
            }
        }


        //����Ԥ��

        /***************
         * 
         * webService��getWeather ���� 
         * �������Ԥ�����ݣ���������Ч theUserID �û�ID�����ɻ�����������Ԥ�����ݣ� 
         * ��������� ��������theUserID �û�ID������û���д���ַ��� theUserID = ""��
         * ��������theCityCode �����ͣ��ַ������������ݿ���Ϊ��������֮һ����ͨ�� getSupportCityDataset �� getSupportCityString ������ã���
         * 1. ���л������������
         * 2. ���л���� ID
         * 3. ����Ϊ���ַ���Ĭ�ϡ��Ϻ���
         * �������ݣ�һά�ַ������� String()���ṹ���£� 
         * Array(0) = "ʡ�� ����/�� �����������⣩" 
         * Array(1) = "��ѯ������Ԥ����������" 
         * Array(2) = "��ѯ������Ԥ������ID" 
         * Array(3) = "������ʱ�� ��ʽ��yyyy-MM-dd HH:mm:ss" 
         * Array(4) = "��ǰ����ʵ�������¡�����/������ʪ��" 
         * Array(5) = "��һ�� ����������������ǿ��" 
         * Array(6) = "��һ�� ����������ָ��" 
         * Array(7) = "��һ�� �ſ� ��ʽ��M��d�� �����ſ�" 
         * Array(8) = "��һ�� ����" 
         * Array(9) = "��һ�� ����/����" 
         * Array(10) = "��һ�� ����ͼ�� 1" 
         * Array(11) = "��һ�� ����ͼ�� 2" 
         * Array(12) = "�ڶ��� �ſ� ��ʽ��M��d�� �����ſ�" 
         * Array(13) = "�ڶ��� ����" 
         * Array(14) = "�ڶ��� ����/����" 
         * Array(15) = "�ڶ��� ����ͼ�� 1"
         * Array(16) = "�ڶ��� ����ͼ�� 2" ...... ......
         * 
         * ÿһ��ĸ�ʽͬ��
         * Array(12) -- Array(16) ...... Array(n-4) = "���һ�� �ſ� ��ʽ��M��d�� �����ſ�" 
         * Array(n-3) = "���һ�� ����" 
         * Array(n-2) = "���һ�� ����/����" 
         * Array(n-1) = "���һ�� ����ͼ�� 1" 
         * Array(n) = "���һ�� ����ͼ�� 2"
         * 
         * */
        public static Weather GetWeather()
        {
            //string ShanghaiCity="�Ϻ�,31112";


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
                log.Error("��ѯ��������", ex);

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