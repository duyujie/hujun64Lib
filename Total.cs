using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using com.hujun64.util;

namespace com.hujun64
{
    /// <summary>
    /// Total 的摘要说明。
    /// </summary>
    public class Total
    {
        private Total()
        {
        }
        public static readonly bool IsMirrorSite = Convert.ToBoolean(ConfigurationManager.AppSettings["isMirrorSite"]);//是否镜像网站
        public static readonly bool IsSyncMirror = Convert.ToBoolean(ConfigurationManager.AppSettings["isSyncMirror"]);//是否需要同步镜像
        public static readonly bool IsFullSiteurl = Convert.ToBoolean(ConfigurationManager.AppSettings["isFullSiteurl"]);//页面中的链接地址是否完全路径
        public static readonly bool IsCacheArticleContent = Convert.ToBoolean(ConfigurationManager.AppSettings["isCacheArticleContent"]);//是否缓存文章内容
        public static readonly bool IsCacheGuestbookContent = Convert.ToBoolean(ConfigurationManager.AppSettings["isCacheGuestbookContent"]);//是否缓存咨询内容
        public static readonly bool IsStaticToHtml = Convert.ToBoolean(ConfigurationManager.AppSettings["isStaticToHtml"] == null ? "false" : ConfigurationManager.AppSettings["isStaticToHtml"]);//是否静态化
       


        public static readonly string MonitorUrl = ConfigurationManager.AppSettings["monitorUrl"];//监控网站网址      
        public static readonly string SiteName = ConfigurationManager.AppSettings["sitename"];//网站名称             
        public static readonly string SiteUrl = ConfigurationManager.AppSettings["siteurl"];//网站网址      
        public static readonly string[] SiteIpArray = ConfigurationManager.AppSettings["siteip"].Split(',');//网站IP地址
        public static readonly string Logo = ConfigurationManager.AppSettings["logo"];//网站Logo
        public static readonly string LinkLogo = ConfigurationManager.AppSettings["linklogo"];//网站友情链接图标
        public static readonly bool EnableCGW = Convert.ToBoolean(ConfigurationManager.AppSettings["enableCGW"]);//是否启用敏感词替代功能
        public static readonly string Author = ConfigurationManager.AppSettings["author"];//网站作者
        public static readonly string Mobile = ConfigurationManager.AppSettings["mobile"];//手机
        public static readonly string Mobile2 = ConfigurationManager.AppSettings["mobile2"];//手机2
        public static readonly string Tel = ConfigurationManager.AppSettings["tel"];//固话
        public static readonly string Qq = ConfigurationManager.AppSettings["qq"];//QQ号
        public static readonly string Msn = ConfigurationManager.AppSettings["msn"];//MSN
        public static readonly string Email = ConfigurationManager.AppSettings["email"];//邮件        
        public static readonly string Fax = ConfigurationManager.AppSettings["fax"];//传真

        public static readonly string AddressWorkday = ConfigurationManager.AppSettings["addressWorkday"];//工作日地址
        public static readonly string AddressWeekend = ConfigurationManager.AppSettings["addressWeekend"];//周末地址
        public static readonly string NearbyWorkday = ConfigurationManager.AppSettings["nearbyWorkday"];//工作日地址附近
        public static readonly string NearbyWeekend = ConfigurationManager.AppSettings["nearbyWeekend"];//周末地址附近
        public static readonly string Office = ConfigurationManager.AppSettings["office"];//事务所
        public static readonly string OfficeUrl = ConfigurationManager.AppSettings["officeUrl"];//事务所网站
        public static readonly string OfficeIntro = ConfigurationManager.AppSettings["officeIntro"];//事务所介绍
        public static readonly string Sn = ConfigurationManager.AppSettings["sn"];//执业证号
        public static readonly string Zipcode = ConfigurationManager.AppSettings["zipcode"];//邮编
        public static readonly string Description = ConfigurationManager.AppSettings["description"];//网站描述
        public static readonly string Profile = ConfigurationManager.AppSettings["profile"];//个人描述
        public static readonly string FeeTitle = ConfigurationManager.AppSettings["feeTitle"];//个人描述
        public static readonly string PageMetaXml = ConfigurationManager.AppSettings["pageMetaXml"];//个人描述
        public static readonly int RefreshSleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["refreshSleepTime"]);




        public static readonly string AdminMail = ConfigurationManager.AppSettings["adminmail"];//站长邮箱
        public static readonly string AdminMailUser = ConfigurationManager.AppSettings["mailuser"];//站长邮箱登录用户
        public static readonly string AdminMailPasswd = ConfigurationManager.AppSettings["mailpasswd"];//站长邮箱登录密码
        public static readonly string SmtpServerHost = ConfigurationManager.AppSettings["smtpServerHost"];
        public static readonly int SmtpServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpServerPort"]);
        public static readonly bool SmtpServerEnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpServerEnableSSL"]);
        public static readonly string Copyright = ConfigurationManager.AppSettings["copyright"];//版权
        public static readonly string Title = ConfigurationManager.AppSettings["title"];//标题
        public static readonly string Keywords = ConfigurationManager.AppSettings["keywords"];//搜索关键字

        public static string PhysicalRootPath = ConfigurationManager.AppSettings["physicalRootPath"];//虚拟目录绝对路径
        public static readonly string Sitemap = ConfigurationManager.AppSettings["Sitemap"];//Sitemap
        public static readonly string BeianUrl = ConfigurationManager.AppSettings["beianUrl"];//备案官方网站地址
        public static readonly string BeianNumber = ConfigurationManager.AppSettings["beianNumber"];//备案号
        public static readonly string CnzzWebsiteId = ConfigurationManager.AppSettings["cnzzWebsiteId"];//CNZZ注册ID号
        public static readonly string AspxUrlOriginalLawyerSearch = ConfigurationManager.AppSettings["originalLawyerSearchUrl"];//律师律所检索URL出处


        //public static string DbConnectString = ConfigurationManager.AppSettings["dbConnectionString"];
        //public static string BackupDbConnectString = ConfigurationManager.AppSettings["backupDbConnectionString"];

        public static readonly string WebxmlUserId = ConfigurationManager.AppSettings["cn.com.webxml.www.userId"];//http://www.webxml.com.cn的注册用户名
        public static readonly string SmsUsername = ConfigurationManager.AppSettings["smsUsername"];//短信发送注册用户名
        public static readonly string SmsPassword = ConfigurationManager.AppSettings["smsPassword"];//短信发送密码

        public static readonly bool EnableWSProxy = Convert.ToBoolean(ConfigurationManager.AppSettings["enableWSProxy"]);//是否启用WebService的代理服务器
        public static readonly string WSProxyHost = ConfigurationManager.AppSettings["WSProxyHost"];//WebService的代理服务器
        public static readonly string WSProxyUserDomain = ConfigurationManager.AppSettings["WSProxyUserDomain"];//WebService的代理服务器用户
        public static readonly string WSProxyUserName = ConfigurationManager.AppSettings["WSProxyUserName"];//WebService的代理服务器用户
        public static readonly string WSProxyPassword = ConfigurationManager.AppSettings["WSProxyPassword"];//WebService的代理服务器密码

        public static readonly int ExpiresNewArticle = Convert.ToInt32(ConfigurationManager.AppSettings["expiresNewArticle"]);
        public static readonly int ExpiresNewGuestbook = Convert.ToInt32(ConfigurationManager.AppSettings["expiresNewGuestbook"]);
        public static readonly int ExpiresLog = Convert.ToInt32(ConfigurationManager.AppSettings["expiresLog"]);
        public static readonly double TimerInterval = Convert.ToDouble(ConfigurationManager.AppSettings["timerInterval"]);//毫秒
        public static readonly string[] RefreshSiteHoursArray = ConfigurationManager.AppSettings["refreshSiteHours"].Split(',');





        public static readonly string ErrorPage = "~/error.aspx";


        public static readonly string AspxUrlIndex = "index.aspx";
        public static readonly string AspxUrlShowdetail = "news_show.aspx";
        public static readonly string AspxUrlGuestbook = "guestbook.aspx";
        public static readonly string AspxUrlGuestAsk = "guestask.aspx";
        public static readonly string AspxUrlBuilding = "building.aspx";
        public static readonly string AspxUrlIntro = "intro.aspx";
        public static readonly string AspxUrlFee = "fee.aspx";
        public static readonly string AspxUrlModule = "listitem.aspx";
        public static readonly string AspxUrlBigClass = "listclass.aspx";
        public static readonly string AspxUrlSmallClass = "listclass.aspx";
        public static readonly string AspxUrlSearch = "result.aspx";
        public static readonly string AspxUrlLinkAll = "linkall.aspx";
        public static readonly string AspxUrlLinkApp = "linkapp.aspx";
        public static readonly string AspxUrlFrameFlzx = "frame_flzx.aspx";
        public static readonly string AspxUrlFrameTodaytips = "frame_todaytips.aspx";
        public static readonly string AspxUrlFrameHeader = "frame_header.aspx";
        public static readonly string AspxUrlFrameFooter = "frame_footer.aspx";
        public static readonly string AspxUrlFrameDropdownMenu = "dropdown_menu.aspx";
        public static readonly string AspxUrlAsyncTask = "asynctask.aspx";
        public static readonly string AspxUrlWebService = "webservice.aspx";
        public static readonly string AspxUrlStat = "stat.aspx";
        public static readonly string AspxUrlStatClick = "statclick.aspx";
        public static readonly string AspxUrlCnzzStat = ConfigurationManager.AppSettings["cnzzServer"] + "/stat.php?id=" + CnzzWebsiteId + "&web_id=" + CnzzWebsiteId + "&show=pic";
        public static readonly string AspxUrlSearchGoogle = "http://www.google.com/search?hl=zh-CN&newwindow=1&safe=strict&q=site:" + UtilHtml.GetSite(SiteUrl);
        public static readonly string AspxUrlSearchBaidu = "http://www.baidu.com/s?wd=site:" + UtilHtml.GetSite(SiteUrl);


        public static readonly string HtmlUrlIndex = UtilHtml.ReplaceAspxWithHtml(AspxUrlIndex);
        public static readonly string HtmlUrlShowdetail = UtilHtml.ReplaceAspxWithHtml(AspxUrlShowdetail);
        public static readonly string HtmlUrlGuestbook = UtilHtml.ReplaceAspxWithHtml(AspxUrlGuestbook);
        public static readonly string HtmlUrlGuestAsk = UtilHtml.ReplaceAspxWithHtml(AspxUrlGuestAsk);
        public static readonly string HtmlUrlBuilding = UtilHtml.ReplaceAspxWithHtml(AspxUrlBuilding);
        public static readonly string HtmlUrlIntro = UtilHtml.ReplaceAspxWithHtml(AspxUrlIntro);
        public static readonly string HtmlUrlFee = UtilHtml.ReplaceAspxWithHtml(AspxUrlFee);
        public static readonly string HtmlUrlModule = UtilHtml.ReplaceAspxWithHtml(AspxUrlModule);
        public static readonly string HtmlUrlBigClass = UtilHtml.ReplaceAspxWithHtml(AspxUrlBigClass);
        public static readonly string HtmlUrlSmallClass = UtilHtml.ReplaceAspxWithHtml(AspxUrlSmallClass);
        public static readonly string HtmlUrlSearch = UtilHtml.ReplaceAspxWithHtml(AspxUrlSearch);
        public static readonly string HtmlUrlLinkAll = UtilHtml.ReplaceAspxWithHtml(AspxUrlLinkAll);
        public static readonly string HtmlUrlLinkApp = UtilHtml.ReplaceAspxWithHtml(AspxUrlLinkApp);
        public static readonly string HtmlUrlFrameFlax = UtilHtml.ReplaceAspxWithHtml(AspxUrlFrameFlzx);
        public static readonly string HtmlUrlFrameTodaytips = UtilHtml.ReplaceAspxWithHtml(AspxUrlFrameTodaytips);
        public static readonly string HtmlUrlAsyncTask = UtilHtml.ReplaceAspxWithHtml(AspxUrlAsyncTask);
        public static readonly string HtmlUrlWebService = UtilHtml.ReplaceAspxWithHtml(AspxUrlWebService);
        public static readonly string HtmlUrlStat = UtilHtml.ReplaceAspxWithHtml(AspxUrlStat);
        public static readonly string HtmlUrlStatClick = UtilHtml.ReplaceAspxWithHtml(AspxUrlStatClick);

        public static readonly List<string> ServerIpList = HardwareInfo.GetIpAddress();
        public static bool IsDefinedServer = false;
        public static string CurrentSiteUrl = null;
        public static string CurrentSiteRootUrl = null;//可能带虚拟目录
        public static string ApplicationPath = null;
        public static string SiteId = null;

        public static readonly string HttpHeaderUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 2.0.50727)";

        public static readonly string HtmlPath = "html";
        public static readonly string IndexPath = "~/index";


        public static string CGWFileapth = "cgw/cgw.txt";


        public static readonly System.Text.Encoding EncodingDefault = Encoding.GetEncoding("gb2312");





        public static readonly int CookieExpiresDays = 365;
        public static readonly string IdFormatString = "000000";

        public static readonly int PageSizeDefault = 20;
        public static readonly int PageSizeGuestbook = 20;
        public static readonly int PageSizeListitem = 20;

        public static readonly int MaxSmsMsgSize = 70; //短信内容字数限制
        public static readonly int MaxSmsMobileSize = 100; //群发手机数目限制

        public static readonly int MaxSearchResultSize = 50; //搜索结果记录数限制

        public static readonly int MaxRowsIndexImageArticle = 4;
        public static readonly int MaxRowsIndexAllArticle = 10;
        public static readonly int MaxRowsIndexHot = 10;
        public static readonly int MaxRowsListClass = 8;
        public static readonly int MaxRowsIndexFlzx = 10;
        public static readonly int MaxRowsIndexSszz = 5;
        public static readonly int MaxRowsIndexSszzIndex = 8;
        public static readonly int MaxRowsIndexBasj = 5;
        public static readonly int MaxRowsIndexFee = 5;
        public static readonly int MaxRowsIndexIntro = 10;
        public static readonly int MaxRowsIndexPfzs = 8;
        public static readonly int MaxRowsIndexCyws = 5;


        public static readonly string ImgModuleTitleUrl = "<img src=\"images/titleno.JPG\" style=\"border-style: none; vertical-align: middle;width:20px;height:18px;\"/>";
        public static readonly string ImgModuleTitleGreenUrl = "<img src=\"images/arr7.gif\"/>";
        public static readonly string ImgMoreUrl = "images/more.gif";
        public static readonly string ImgNewUrl = "images/new1.gif";
        public static readonly string ImgProfileUrl = "images/profile.gif";
        public static readonly string ImgWriteGuestbookUrl = "images/reg.gif";
        public static readonly string ImgAddressWorkdayUrl = "images/hl_address.jpg";
        public static readonly string ImgAddressWeekendUrl = "images/sq_address.jpg";


        public static readonly string BannerFocusUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_focus.gif", "关注弱势群体维权。"+Total.SiteUrl+" 上海胡珺律师维护您的权益！", "580px", "60px"), Title);
        public static readonly string BannerHorseUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_horse.gif", "解决纠纷，马到功成。" + Total.SiteUrl + " 上海胡珺律师维护您的权益！", "580px", "60px"), Title);
        public static readonly string BannerCommonaltyUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_commonalty.gif", "受百姓之托，做大众律师。 " + Total.SiteUrl + " 上海胡珺律师维护您的权益！", "580px", "60px"), Title);
        public static readonly string BannerProfessionalUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_professional.gif", "因为专业，所以卓越。胡律师专注于民商事领域。 " + Total.SiteUrl + " 上海胡珺律师维护您的权益！", "580px", "60px"), Title);
        public static readonly string BannerNoMatterUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_nomatter.gif", "无事不生事，有事不怕事。" + Total.SiteUrl + " 上海胡珺律师维护您的权益！", "580px", "60px"), Title);
        public static readonly string BannerHireUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_hirelawyer.gif", "聘请律师。公道自在人心，维权还需法律。" + Total.SiteUrl + " 上海胡珺律师维护您的权益！", "580px", "60px"), Title);
        public static readonly string BannerRespectUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_respect.gif", "敬律师之业，行仁义之德。事辛苦之力，求法律之公。" + Total.SiteUrl + " 上海胡珺律师维护您的权益！", "580px", "60px"), Title);
        public static readonly string BannerObjectUrl = UtilHtml.BuildHref(AspxUrlIntro, UtilHtml.GetFullImageUrl(@"images/banner_object_blue.gif", "以维护委托人的合法利益为天职，以规避委托人的法律风险为目标。" + Total.SiteUrl + " 上海胡珺律师维护您的权益！", "580px", "60px"), Title);




        public static readonly string SplitMyLocation = " -> ";
        public static readonly string SplitTitle = "-";
        public static readonly string PrefixArticleId = "A";
        public static readonly string PrefixGuestbookId = "G";
        public static readonly string PrefixLinkId = "L";
        public static readonly string PrefixVisitId = "V";
        public static readonly string PrefixClientId = "C";
        public static readonly string PrefixArticlePictureId = "P";
        public static readonly string PrefixCgwWordsId = "CGW";

        public static readonly string DivIdContent = "articleContent";
        public static readonly string DivDefaultClassContent = "defaultContent";
        public static readonly string DivDefinedClassContent = "definedContent";

        public static readonly string ModuleNameIndex = "首页";
        public static readonly string ModuleNameFzrd = "法治热点";
        public static readonly string ModuleNameJdal = "经典案例";
        public static readonly string ModuleNameSyfg = "实用法规";
        public static readonly string ModuleNameGrsb = "个人随笔";
        public static readonly string ModuleNameSszz = "诉讼自助";
        public static readonly string ModuleNameSszs = "诉讼知识";
        public static readonly string ModuleNameBasj = "办案手记";
        public static readonly string ModuleNameLsjs = "律师介绍";
        public static readonly string ModuleNameCyws = "常用文书";
        public static readonly string ModuleNameFlzx = "网上咨询";
        public static readonly string ModuleNameFlzs = "法律知识";
        public static readonly string ModuleNameSfbz = "收费标准";
        public static readonly string ModuleNameYqlj = "友情链接";
        public static readonly string ModuleNameSqyqlj = "申请友情链接";
        public static readonly string ModuleNameGywm = "关于我们";


        public static readonly string BigClassNameJjfc = "经济房产类";
        public static readonly string BigClassNameLhjc = "离婚继承类";
        public static readonly string BigClassNameRssh = "人身损害类";
        public static readonly string BigClassNameGsld = "工伤劳动类";

         
        public static readonly string ModuleNameXwkf = "新闻看法";
        public static readonly string ModuleNameTpxw = "图片新闻";
        public static readonly string ModuleNameFwmm = "房屋买卖";
        public static readonly string ModuleNameZldy = "租赁抵押";
        public static readonly string ModuleNameLhjc = "离婚继承";
        public static readonly string ModuleNameCqaz = "拆迁安置";
        public static readonly string ModuleNameWygl = "物业管理";
        public static readonly string ModuleNameFwqs = "房屋权属";
        public static readonly string ModuleNameShpc = "损害赔偿"; 
        public static readonly string ModuleNameJskf = "建设开发";
        public static readonly string ModuleNameLsbw = "律师博文";
        public static readonly string ModuleNameLsbwsj = "律师博文与手记";
        public static readonly string ModuleNameShfzb = "上海法治报";

        public static List<string> IndexModuleClassIncludeBigClassNameList = new List<string>();

        public static readonly string BigClassNameYasf = "以案说法";
        public static readonly string BigClassNameFlkt = "法律课堂";
        public static readonly string BigClassNameFgjd = "法规解读";

        public static readonly string ContactWorkdayArticleTitle = "联系方式";
        public static readonly string ContactWeekendArticleTitle = "联系方式（周末）";
        public static readonly string ByOriginal = "本站原创";
        public static readonly string LawyerHujun = "胡珺律师";



        public static readonly string QueryStringModuleClassId = "module_id";
        public static readonly string QueryStringBigClassId = "big_id";
        public static readonly string QueryStringSmallClassId = "small_id";
        public static readonly string QueryStringArticleId = "nid";
        public static readonly string QueryStringDeletedArticleId = "dnid";
        public static readonly string QueryStringFeeId = "fid";
        public static readonly string QueryStringIntroId = "iid";
        public static readonly string QueryStringGuestbookId = "gid";
        public static readonly string QueryStringAction = "action";
        public static readonly string QueryStringRefreshType = "refreshType";
        public static readonly string QueryStringReferrer = "referrer";


        public static readonly string CONVERTING_CONFIG = "CONVERTING_CONFIG";

        static Total()
        {
            //Profile = Profile.Replace("上海房产律师", "<a href=\"http://www.shfclawyer.com\" title='上海房产律师' target=\"_blank\"><font color=\"blue\">上海房产律师</font></a>")
            //.Replace("上海律师", "<a href=\"http://www.hujun64.com\" title='上海律师' target=\"_blank\"><font color=\"blue\">上海律师</font></a>");
            
            IndexModuleClassIncludeBigClassNameList.Add(ModuleNameFwmm);
            IndexModuleClassIncludeBigClassNameList.Add(ModuleNameZldy);
            IndexModuleClassIncludeBigClassNameList.Add(ModuleNameLhjc);
            IndexModuleClassIncludeBigClassNameList.Add(ModuleNameCqaz);
            IndexModuleClassIncludeBigClassNameList.Add(ModuleNameFwqs);
            //IndexModuleClassIncludeBigClassNameList.Add(ModuleNameShpc);    
            IndexModuleClassIncludeBigClassNameList.Add(ModuleNameWygl);           
            //IndexModuleClassIncludeBigClassNameList.Add(ModuleNameJskf);
        }
    }



}
