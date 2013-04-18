using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

using com.hujun64.po;
using com.hujun64.type;
using com.hujun64.logic;

namespace com.hujun64.util
{
    /// <summary>
    ///Aspx2Html 的摘要说明
    /// </summary>
    public class UtilStatic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("UtilStatic");

        //private static bool isRunning = false;

        private ICommonService commonService;
        private IGuestbookService guestboookService;
        private IArticleService articleService;
        private IMainClassService mainClassService;
        private ILinkService linkService;


        private List<string> bigClassIdList;
        private List<string> smallClassIdList;
        private List<string> moduleClassIdList;
        private Hashtable smallBigClassIdHash;

        private Dictionary<string, DateTime> introArticleIdDict;
        private Dictionary<string, DateTime> feeArticleIdDict;
        private Dictionary<string, DateTime> articleIdDict;
        private Dictionary<string, DateTime> guestbookIdDict;
        private Dictionary<string, DateTime> linkIdDict;


        private Dictionary<string, string> urlFileDictionary;
        private List<SitemapUrl> urlSitemapList;




        private string rootPath;
        private string appPath;
        private HttpContext context;

        private string refreshType = RefreshType.ONLY_CHANGED;
        private DateTime timestamp;

        private static object privateStaticObject = new Object();
        private UtilStatic()
        {
            init();
        }
        public static UtilStatic GetInstance()
        {
            UtilStatic insteance = new UtilStatic();
            insteance.commonService = ServiceFactory.GetCommonService();
            insteance.mainClassService = ServiceFactory.GetMainClassService();
            insteance.articleService = ServiceFactory.GetArticleService();
            insteance.guestboookService = ServiceFactory.GetGuestbookService();
            insteance.linkService = ServiceFactory.GetLinkService();

            return insteance;
        }

        private void init()
        {
            context = HttpContext.Current;
            rootPath = HttpContext.Current.Server.MapPath("~/");
            appPath = UtilHttp.GetApplicationPath(context);

        }


        private void DoConvertAll(bool isNeedRefresh)
        {



            ConvertIndex();

            if (isNeedRefresh)
            {

                ConvertFrameHtml();
                ConvertGuestbook();
                ConvertArticle();
                ConvertModule();
                ConvertBigClass();
                ConvertSmallClass();
                ConvertIntro();
                ConvertFee();
                ConvertBuilding();
                ConvertLink();

                //write to html file
                UrlToFile();

                UpdateAllStaticStatus(true, timestamp);



            }

        }

        private void SetConvertStatus(Boolean convertStatus)
        {
            lock (privateStaticObject)
            {
                string isConverting = commonService.GetGlobalConfigValue(Total.CONVERTING_CONFIG);
                if (convertStatus && Boolean.TrueString.Equals(isConverting))
                    throw new ApplicationException("有另一个网站更新任务正在执行中，请稍后重试！");
                else
                    commonService.SetGlobalConfigValue(Total.CONVERTING_CONFIG, convertStatus.ToString());
            }
        }
        private bool IsConverting()
        {

            string isConverting = commonService.GetGlobalConfigValue(Total.CONVERTING_CONFIG);

            if (isConverting.Equals(Boolean.TrueString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //=================================================
        //功能描述：将所有aspx动态网页转换成html静态网页
        //输入参数：无
        //返回值：转换后的html url
        //时间：2008.12.09
        //=================================================
        public int ConvertAll(string refreshType)
        {
            if (!Total.IsStaticToHtml)
                return 0;

            log.Info("开始进行页面内容更新");


            int i = 0;
            while (IsConverting())
            {
                Thread.Sleep(10 * 1000);
                i++;
                if (i >= 6)
                {
                    throw new ApplicationException("有另一个网站更新任务正在执行中，请稍后重试！");
                }

            }

            try
            {

                SetConvertStatus(true);


                this.refreshType = refreshType;




                //refresh cache
                ServiceFactory.RefreshAllCache();

                urlFileDictionary = new Dictionary<string, string>();

                this.timestamp = commonService.GetSysdate();
                bool isNeedRefresh = false;

                urlSitemapList = new List<SitemapUrl>();


                ConvertIndex();
                switch (refreshType)
                {
                    case RefreshType.ALL_SITE:

                        UpdateAllStaticStatus(false, timestamp);
                        isNeedRefresh = getAllIdList();

                        DoConvertAll(isNeedRefresh);

                        break;
                    case RefreshType.ONLY_CHANGED:
                        isNeedRefresh = getAllIdList();



                        if (isNeedRefresh)
                        {

                            ConvertFrameHtml();

                            ConvertGuestbook();
                            ConvertArticle();
                            ConvertModule();
                            ConvertBigClass();
                            ConvertSmallClass();
                            ConvertIntro();
                            ConvertFee();


                            //write to html file
                            UrlToFile();

                            UpdateAllStaticStatus(true, timestamp);



                        }


                        DoConvertAll(isNeedRefresh);
                        break;
                    case RefreshType.ALL_FRAME_HTML:
                        getAllIdList();
                        ConvertFrameHtml();

                        //write to html file
                        UrlToFile();



                        break;
                    case RefreshType.ALL_MODULE_LIST:
                        getAllIdList(true);

                        ConvertModule();
                        ConvertBigClass();
                        ConvertSmallClass();
                        ConvertLink();

                        //write to html file
                        UrlToFile();

                        break;
                    case RefreshType.ALL_ARTICLE:
                        articleService.UpdateAllStatic(false, timestamp);
                        getAllIdList();

                        ConvertArticle();

                        ConvertIntro();
                        ConvertFee();
                        articleService.UpdateAllStatic(true, timestamp);

                        //write to html file
                        UrlToFile();


                        break;
                    case RefreshType.ALL_GUESTBOOK:
                        guestboookService.UpdateAllStatic(false, timestamp);
                        getAllIdList();

                        ConvertGuestbook();

                        guestboookService.UpdateAllStatic(true, timestamp);

                        //write to html file
                        UrlToFile();

                        break;
                    case RefreshType.ONLY_GUESTASK:
                        guestbookIdDict = guestboookService.GetAllGuestbookIdDict(false);

                        if (guestbookIdDict.Count > 0)
                        {
                            getClassIdList(false);

                            ConvertFrameHtml();
                            ConvertGuestbook();



                            guestboookService.UpdateListStatic(guestbookIdDict.Keys, true, timestamp);

                            //write to html file
                            UrlToFile();

                        }



                        break;
                    default:
                        break;
                }


                UpdateSitemap();

                log.Info("成功更新了" + urlFileDictionary.Count + "个静态页面！");
                return urlFileDictionary.Count;
            }

            catch (Exception ex)
            {
                log.Error("更新页面出错", ex);
                UtilMail.SendMailAsync("网站错误报告", ex.Message, Total.AdminMail, null);
                return 0;
            }
            finally
            {
                SetConvertStatus(false);
            }


        }
        private void UpdateAllStaticStatus(bool isStaticStatus, DateTime timestamp)
        {


            articleService.UpdateAllStatic(isStaticStatus, timestamp);
            guestboookService.UpdateAllStatic(isStaticStatus, timestamp);
            linkService.UpdateAllStatic(isStaticStatus, timestamp);

        }

        private void UpdateSitemap()
        {

            string sitemapXmlPath = rootPath + "/" + Total.Sitemap;
            try
            {
                List<string> urlList = new List<string>();
                foreach (string url in urlFileDictionary.Values)
                {
                    urlList.Add(url);
                }

                if (RefreshType.ALL_SITE == refreshType)
                {
                    UtilSitemap.UpdateSitemapTxt(rootPath, urlList, true);
                    UtilSitemap.UpdateSitemapXml(sitemapXmlPath, urlSitemapList, SitemapType.Google, true);
                }
                else
                {
                    UtilSitemap.UpdateSitemapTxt(rootPath, urlList, false);
                    UtilSitemap.UpdateSitemapXml(sitemapXmlPath, urlSitemapList, SitemapType.Google, false);
                }

            }
            catch (Exception ex)
            {
                log.Error("生成/更新Sitemap出错！", ex);
            }




        }
        private bool getAllIdList()
        {
            return getAllIdList(false);
        }
        private void getClassIdList(bool isRefreshAll)
        {
            bigClassIdList = mainClassService.GetAllBigClassIdList(isRefreshAll);
            smallClassIdList = mainClassService.GetAllSmallClassIdList(isRefreshAll);

            smallBigClassIdHash = mainClassService.GetSmallBigClassIdHash();

            if (bigClassIdList.Count > 0)
            {
                foreach (string bigId in bigClassIdList)
                {
                    ICollection smallIdKeyList = smallBigClassIdHash.Keys;
                    foreach (string smallId in smallIdKeyList)
                    {
                        if (smallBigClassIdHash.ContainsKey(smallId) && smallBigClassIdHash[smallId].Equals(bigId))
                        {
                            if (!smallClassIdList.Contains(smallId))
                            {
                                smallClassIdList.Add(smallId);
                            }
                        }
                    }
                }
            }
            if (smallClassIdList.Count > 0)
            {
                foreach (string smallId in smallClassIdList)
                {
                    if (!bigClassIdList.Contains((string)smallBigClassIdHash[smallId]))
                    {
                        bigClassIdList.Add((string)smallBigClassIdHash[smallId]);
                    }
                }
            }

            moduleClassIdList = mainClassService.GetAllModuleClassIdList(isRefreshAll);

        }
        private bool getAllIdList(bool isRefreshAll)
        {
            bool isNeedRefresh = false;

            if (RefreshType.ALL_SITE == refreshType)
                isRefreshAll = true;

            introArticleIdDict = articleService.GetAllIntroIdDict(isRefreshAll);
            feeArticleIdDict = articleService.GetAllFeeIdDict(isRefreshAll);
            articleIdDict = articleService.GetAllArticleIdDict(isRefreshAll);
            List<Article> fzbList = articleService.GetTopArticleByModuleName("法制报专栏", int.MaxValue);
            foreach (Article fzb in fzbList)
            {
                if (articleIdDict.ContainsKey(fzb.id))
                    articleIdDict.Remove(fzb.id);
            }


            guestbookIdDict = guestboookService.GetAllGuestbookIdDict(isRefreshAll);
            linkIdDict = linkService.GetAllLinkIdDict(isRefreshAll);

            if (introArticleIdDict.Count == 0 && feeArticleIdDict.Count == 0 && articleIdDict.Count == 0 && guestbookIdDict.Count == 0 && linkIdDict.Count == 0)
            {
                isNeedRefresh = false;
            }
            else
            {
                isNeedRefresh = true;
            }

            getClassIdList(isNeedRefresh);

            return isNeedRefresh;
        }


        private void ConvertArticle()
        {
            List<String> idList = new List<String>();
            idList.AddRange(articleIdDict.Keys);
            idList.Sort();

            foreach (string id in idList)
            {
                if (String.IsNullOrEmpty(id))
                    continue;

                try
                {
                    UrlToStatic(id, PageType.ARTICLE_TYPE, articleIdDict[id]);
                }
                catch (Exception ex)
                {
                    log.Error("Failed to static article:" + id);
                    throw ex;
                }
            }
        }
        private void ConvertGuestbook()
        {
            foreach (string id in guestbookIdDict.Keys)
            {
                UrlToStatic(id, PageType.GUESTBOOK_TYPE, guestbookIdDict[id]);
            }

            //list guestbook
            UrlToStatic("", PageType.GUESTBOOK_TYPE, DateTime.Now);
        }
        private void ConvertSmallClass()
        {

            foreach (string id in smallClassIdList)
            {
                UrlToStatic(id, PageType.SMALL_CLASS_TYPE, DateTime.Now);
            }
        }
        private void ConvertFrameHtml()
        {

            UrlFrameToStatic(PageType.FRAME_HEADER);

            UrlFrameToStatic(PageType.FRAME_TODAYTIPS);
            UrlFrameToStatic(PageType.FRAME_FOOTER);
            UrlFrameToStatic(PageType.GUESTBOOK_TYPE);

            List<string> frameBigClassIdList = mainClassService.GetAllBigClassIdList(true);

            Hashtable frameSmallBigClassIdHash = mainClassService.GetSmallBigClassIdHash();


            UrlFrameToStatic(frameBigClassIdList, null, PageType.FRAME_FLZX);
        }
        private void ConvertBigClass()
        {

            foreach (string id in bigClassIdList)
            {
                UrlToStatic(id, PageType.BIG_CLASS_TYPE, DateTime.Now);
            }
        }
        private void ConvertModule()
        {
            foreach (string id in moduleClassIdList)
            {
                UrlToStatic(id, PageType.MODULE_TYPE, DateTime.Now);
            }
        }
        private void ConvertIntro()
        {
            UrlToStatic("", PageType.INTRO_TYPE, DateTime.Now);

            foreach (string id in introArticleIdDict.Keys)
            {
                UrlToStatic(id, PageType.INTRO_TYPE, introArticleIdDict[id]);
            }

        }
        private void ConvertFee()
        {
            UrlToStatic("", PageType.FEE_TYPE, DateTime.Now);

            foreach (string id in feeArticleIdDict.Keys)
            {
                UrlToStatic(id, PageType.FEE_TYPE, feeArticleIdDict[id]);
            }

        }
        private void ConvertLink()
        {
            UrlToStatic("", PageType.LINK_ALL_TYPE, DateTime.Now);

        }


        private void ConvertBuilding()
        {
            if (RefreshType.ALL_SITE == this.refreshType)
                UrlToStatic("", PageType.BUILDING_TYPE, DateTime.Now);

        }
        private void ConvertIndex()
        {
            UrlToStatic("", PageType.INDEX_TYPE, DateTime.Now);

            if (moduleClassIdList == null || moduleClassIdList.Count == 0)
            {

                this.getClassIdList(false);
            }
            foreach (String moduleClassId in this.moduleClassIdList)
            {
                UrlToStatic(moduleClassId, PageType.INDEX_TYPE, DateTime.Now);
            }





        }


        //=================================================
        //功能描述：将网页中的aspx链接替换成html链接
        //输入参数：网页内容
        //返回值：无
        //时间：2008.12.09
        //=================================================
        private string ConvertPageContent(string pageUrl, string pageContent)
        {

            int hrefIndex;
            int hrefBegin;
            int hrefEnd;
            string priorContent;
            string nextContent;
            bool isInJavascript = false;

            string splitHref = "aspx";


            StringBuilder newPageContent = new StringBuilder(pageContent);



            newPageContent.Replace("\"../", "\"");

            newPageContent.Replace("\"/../", "\"/");



            string content = newPageContent.ToString();
            newPageContent.Remove(0, newPageContent.Length);

            StringBuilder hrefAspxSb = new StringBuilder();
            StringBuilder hrefHtmlSb = new StringBuilder();

            while ((hrefIndex = content.IndexOf(splitHref)) > 0)
            {
                priorContent = content.Substring(0, hrefIndex);
                nextContent = content.Substring(hrefIndex);

                hrefBegin = priorContent.LastIndexOf("'");
                if (priorContent.LastIndexOf("'") > priorContent.LastIndexOf("\""))
                {
                    isInJavascript = true;
                    hrefBegin = priorContent.LastIndexOf("'");
                }
                else
                {
                    isInJavascript = false;
                    hrefBegin = priorContent.LastIndexOf("\"");
                }

                hrefAspxSb.Remove(0, hrefAspxSb.Length);
                hrefAspxSb.Append(priorContent.Substring(hrefBegin + 1));


                if (isInJavascript)
                {
                    hrefEnd = nextContent.IndexOf("'");
                }
                else
                {
                    hrefEnd = nextContent.IndexOf("\"");
                }

                hrefAspxSb.Append(nextContent.Substring(0, hrefEnd));
                newPageContent.Append(content.Substring(0, hrefBegin + 1));

                content = content.Substring(hrefIndex + hrefEnd);

                /**
                 *  "&amp;"就是'&'   
                 * 因为&是转义符号，没有什么特别的意思   
                 * &amp;   =   &   
                 * 在HTML中的&用&amp;   来表示
                 */
                if (hrefAspxSb.ToString().IndexOf("&amp;") > 0)
                {
                    hrefAspxSb.Replace("&amp;", "&");
                }


                hrefHtmlSb.Remove(0, hrefHtmlSb.Length);
                hrefHtmlSb.Append(UtilHtml.ConvertAspxWithHtml(hrefAspxSb.ToString()));

                newPageContent.Append(hrefHtmlSb);
            }
            newPageContent.Append(content);
            newPageContent.Replace(Total.HtmlPath + "/" + Total.HtmlUrlIndex, Total.HtmlUrlIndex);


            return newPageContent.ToString();
        }




        private List<string> getPageUrlList(string aspxUrl, int recordCount, int pageSize)
        {
            int pageNum = recordCount / pageSize + 1;
            if (pageNum <= 1)
                return null;

            StringBuilder newAspxUrlSb = new StringBuilder();
            List<string> pageUrlList = new List<string>(pageNum);
            bool hasParams = false;
            if (!aspxUrl.Trim().EndsWith(".aspx"))
            {
                hasParams = true;
            }

            for (int i = 0; i <= pageNum; i++)
            {
                newAspxUrlSb.Remove(0, newAspxUrlSb.Length);
                newAspxUrlSb.Append(aspxUrl);

                if (i > 0)
                {
                    if (hasParams)
                    {
                        newAspxUrlSb.Append("&");
                    }
                    else
                    {
                        newAspxUrlSb.Append("?");
                    }
                    newAspxUrlSb.Append("page=");
                    newAspxUrlSb.Append(i);
                }

                pageUrlList.Add(newAspxUrlSb.ToString());

            }
            return pageUrlList;
        }
        private void MapAspx2HtmlWithPage(string aspxUrl, string htmlUrl, int rowCount, int pageSize, SitemapUrl sitemapUrl)
        {

            List<string> pageUrlList = getPageUrlList(aspxUrl, rowCount, pageSize);

            if (pageUrlList != null)
            {
                foreach (string pageUrl in pageUrlList)
                {
                    MapAspx2Html(pageUrl, htmlUrl, sitemapUrl);
                }
            }

        }
        private void MapAspx2HtmlWithBigSmallClass(List<string> bigClassList, Hashtable smallBigClassHash, string modouleClassId, string aspxUrl, string htmlUrl, PageType pageType, int pageSize, bool withSmall, SitemapUrl sitemapUrl)
        {

            int rowCount = 0;
            bool hasOtherParam = false;

            if (aspxUrl.Contains("?"))
                hasOtherParam = true;

            StringBuilder newAspxUrlSb = new StringBuilder();
            foreach (string bigClassId in bigClassList)
            {
                newAspxUrlSb.Remove(0, newAspxUrlSb.Length);
                newAspxUrlSb.Append(aspxUrl);
                if (hasOtherParam)
                    newAspxUrlSb.Append("&");
                else
                    newAspxUrlSb.Append("?");
                newAspxUrlSb.Append(Total.QueryStringBigClassId);
                newAspxUrlSb.Append("=");
                newAspxUrlSb.Append(bigClassId);

                MapAspx2Html(newAspxUrlSb.ToString(), htmlUrl, sitemapUrl);
                if (pageType == PageType.ARTICLE_TYPE || pageType == PageType.MODULE_TYPE)
                {
                    rowCount = articleService.CountArticle(modouleClassId, bigClassId, null);
                }
                else if (pageType == PageType.GUESTBOOK_TYPE)
                {
                    rowCount = guestboookService.CountGuestbook(bigClassId);
                }
                MapAspx2HtmlWithPage(newAspxUrlSb.ToString(), htmlUrl, rowCount, pageSize, sitemapUrl);

                if (withSmall && smallBigClassHash != null && smallBigClassHash.Count > 0)
                {

                    ICollection smallIdKeyList = smallBigClassHash.Keys;
                    foreach (string smallClassId in smallIdKeyList)
                    {
                        if (smallBigClassHash.ContainsKey(smallClassId) && smallBigClassHash[smallClassId].Equals(bigClassId))
                        {
                            newAspxUrlSb.Remove(0, newAspxUrlSb.Length);
                            newAspxUrlSb.Append(aspxUrl);
                            if (hasOtherParam)
                                newAspxUrlSb.Append("&");
                            else
                                newAspxUrlSb.Append("?");
                            newAspxUrlSb.Append(Total.QueryStringBigClassId);
                            newAspxUrlSb.Append("=");
                            newAspxUrlSb.Append(bigClassId);
                            newAspxUrlSb.Append("&");
                            newAspxUrlSb.Append(Total.QueryStringSmallClassId);
                            newAspxUrlSb.Append("=");
                            newAspxUrlSb.Append(smallClassId);

                            MapAspx2Html(newAspxUrlSb.ToString(), htmlUrl, sitemapUrl);
                            if (pageType == PageType.ARTICLE_TYPE || pageType == PageType.MODULE_TYPE)
                            {
                                rowCount = articleService.CountArticle(modouleClassId, bigClassId, smallClassId);
                            }
                            MapAspx2HtmlWithPage(newAspxUrlSb.ToString(), htmlUrl, rowCount, pageSize, sitemapUrl);
                        }
                    }

                }
            }


        }
        private void MapAspx2HtmlForIndexWithModule(string modouleClassId, string aspxUrl, string htmlUrl, SitemapUrl sitemapUrl)
        {


            bool hasOtherParam = false;

            if (aspxUrl.Contains("?"))
                hasOtherParam = true;

            StringBuilder newAspxUrlSb = new StringBuilder();

            newAspxUrlSb.Remove(0, newAspxUrlSb.Length);
            newAspxUrlSb.Append(aspxUrl);
            if (hasOtherParam)
                newAspxUrlSb.Append("&");
            else
                newAspxUrlSb.Append("?");
            newAspxUrlSb.Append(Total.QueryStringModuleClassId);
            newAspxUrlSb.Append("=");
            newAspxUrlSb.Append(modouleClassId);

            MapAspx2Html(newAspxUrlSb.ToString(), htmlUrl, sitemapUrl);




        }
        private SitemapUrl InitSitemapUrl(string id, PageType pageType, DateTime lastModifyDateime)
        {

            SitemapUrl sitemapUrl = new SitemapUrl();
            switch (pageType)
            {

                case PageType.INDEX_TYPE:
                    sitemapUrl.changefreq = FreqEnum.always;
                    sitemapUrl.lastmod = DateTime.Now;
                    sitemapUrl.priority = PriorityType.VERY_HIGH;
                    break;
                case PageType.MODULE_TYPE:
                    sitemapUrl.changefreq = FreqEnum.always;
                    sitemapUrl.lastmod = DateTime.Now;
                    sitemapUrl.priority = PriorityType.MEDIUM;
                    break;
                case PageType.BIG_CLASS_TYPE:
                case PageType.SMALL_CLASS_TYPE:
                    sitemapUrl.changefreq = FreqEnum.always;
                    sitemapUrl.lastmod = DateTime.Now;
                    sitemapUrl.priority = PriorityType.HIGH;
                    break;
                case PageType.INTRO_TYPE:
                case PageType.FEE_TYPE:
                    sitemapUrl.changefreq = FreqEnum.monthly;
                    sitemapUrl.lastmod = lastModifyDateime;
                    sitemapUrl.priority = PriorityType.MEDIUM;
                    break;
                case PageType.GUESTBOOK_TYPE:
                    if (string.IsNullOrEmpty(id))
                    {
                        sitemapUrl.changefreq = FreqEnum.always;
                        sitemapUrl.lastmod = DateTime.Now;
                        sitemapUrl.priority = PriorityType.HIGH;
                    }
                    else
                    {
                        sitemapUrl.changefreq = FreqEnum.daily;
                        sitemapUrl.lastmod = lastModifyDateime;
                        sitemapUrl.priority = PriorityType.VERY_LOW;
                    }

                    break;
                default:
                    sitemapUrl.changefreq = FreqEnum.monthly;
                    sitemapUrl.lastmod = lastModifyDateime;
                    sitemapUrl.priority = PriorityType.LOW;

                    break;
            }
            return sitemapUrl;
        }
        private void UrlToStatic(string id, PageType pageType, DateTime lastModifyDateime)
        {


            string aspxUrl;
            string htmlUrl = rootPath;

            aspxUrl = UtilHtml.GetAspxUrl(id, pageType);

            if (pageType != PageType.INDEX_TYPE)
            {
                htmlUrl += Total.HtmlPath + "\\";
            }
            SitemapUrl sitemapUrl = InitSitemapUrl(id, pageType, lastModifyDateime);

            switch (pageType)
            {
                case PageType.GUESTBOOK_TYPE:

                    MapAspx2Html(aspxUrl, htmlUrl, sitemapUrl);

                    int guestbookCount = guestboookService.CountGuestbook(null);
                    if (string.IsNullOrEmpty(id))
                    {
                        MapAspx2HtmlWithPage(aspxUrl, htmlUrl, guestbookCount, Total.PageSizeGuestbook, sitemapUrl);
                        MapAspx2HtmlWithBigSmallClass(bigClassIdList, smallBigClassIdHash, null, aspxUrl, htmlUrl, pageType, Total.PageSizeGuestbook, false, sitemapUrl);
                    }

                    break;

                case PageType.MODULE_TYPE:

                    MapAspx2Html(aspxUrl, htmlUrl, sitemapUrl);


                    int articleCount = articleService.CountArticle(id, null, null);
                    MapAspx2HtmlWithPage(aspxUrl, htmlUrl, articleCount, Total.PageSizeDefault, sitemapUrl);


                    MapAspx2HtmlWithBigSmallClass(bigClassIdList, smallBigClassIdHash, id, aspxUrl, htmlUrl, pageType, Total.PageSizeDefault, true, sitemapUrl);

                    break;
                case PageType.BIG_CLASS_TYPE:

                    MapAspx2Html(aspxUrl, htmlUrl, sitemapUrl);


                    int bigArticleCount = articleService.CountArticle(null, id, null);
                    MapAspx2HtmlWithPage(aspxUrl, htmlUrl, bigArticleCount, Total.PageSizeDefault, sitemapUrl);

                    break;
                case PageType.INDEX_TYPE:

                    if (String.IsNullOrEmpty(id))
                        MapAspx2Html(aspxUrl, htmlUrl, sitemapUrl);
                    else
                        MapAspx2HtmlForIndexWithModule(id, aspxUrl, htmlUrl, sitemapUrl);


                    break;
                default:

                    MapAspx2Html(aspxUrl, htmlUrl, sitemapUrl);
                    break;
            }


        }
        private void UrlFrameToStatic(PageType pageType)
        {
            UrlFrameToStatic(null, null, pageType);
        }
        private void UrlFrameToStatic(List<string> bigClassList, Hashtable smallBigClassHash, PageType pageType)
        {


            string aspxUrl;
            string htmlUrl = rootPath;

            aspxUrl = UtilHtml.GetAspxUrl(pageType);

            htmlUrl += Total.HtmlPath + "\\";


            switch (pageType)
            {
                case PageType.FRAME_FLZX:

                    MapAspx2Html(aspxUrl, htmlUrl, null);


                    MapAspx2HtmlWithBigSmallClass(bigClassList, smallBigClassHash, null, aspxUrl, htmlUrl, pageType, Total.PageSizeGuestbook, false, null);


                    break;
                case PageType.GUESTBOOK_TYPE:
                    MapAspx2Html(aspxUrl, htmlUrl, null);

                    int guestbookCount = guestboookService.CountGuestbook(null);

                    MapAspx2HtmlWithPage(aspxUrl, htmlUrl, guestbookCount, Total.PageSizeGuestbook, null);
                    MapAspx2HtmlWithBigSmallClass(bigClassIdList, smallBigClassIdHash, null, aspxUrl, htmlUrl, pageType, Total.PageSizeGuestbook, false, null);


                    break;


                default:

                    MapAspx2Html(aspxUrl, htmlUrl, null);
                    break;
            }


        }
        private string MapAspx2Html(string aspxUrl, string htmlUrl)
        {

            return MapAspx2Html(aspxUrl, htmlUrl, null);
        }
        private string MapAspx2Html(string aspxUrl, string htmlUrl, SitemapUrl sitemapUrl)
        {

            StringBuilder fullAspxUrlSb = new StringBuilder(appPath);
            htmlUrl += UtilHtml.ReplaceAspxWithHtml(aspxUrl);
            fullAspxUrlSb.Append(aspxUrl);

            if (!urlFileDictionary.ContainsKey(fullAspxUrlSb.ToString()))
            {
                urlFileDictionary.Add(fullAspxUrlSb.ToString(), htmlUrl);

                if (sitemapUrl != null)
                {
                    SitemapUrl newSitemapUrl = new SitemapUrl(htmlUrl.Replace(rootPath, Total.SiteUrl + "/").Replace("\\", "/"));

                    newSitemapUrl.changefreq = sitemapUrl.changefreq;
                    newSitemapUrl.lastmod = sitemapUrl.lastmod;
                    newSitemapUrl.priority = sitemapUrl.priority;

                    urlSitemapList.Add(newSitemapUrl);
                }
            }
            return htmlUrl;

        }
        private bool WriteHtmlFile(string filePath, string pageContent)
        {
            return WriteHtmlFile(filePath, pageContent, 1);
        }

        private bool WriteHtmlFile(string filePath, string pageContent, int maxRetryTimes)
        {

            int SLEEP_MILLISECONDS = 300;

            FileStream fs = null;
            StreamWriter streamWriter = null;
            bool writeSucess = false;
            int retryTimes = 1;
            do
            {
                try
                {

                    fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write);

                    streamWriter = new StreamWriter(fs, com.hujun64.Total.EncodingDefault);

                    streamWriter.Write(pageContent);
                    streamWriter.Flush();

                    writeSucess = true;


                }
                catch (IOException ex)
                {
                    if (retryTimes < maxRetryTimes)
                    {
                        Thread.Sleep(SLEEP_MILLISECONDS);
                    }
                    else
                    {
                        throw ex;
                    }

                }
                catch (Exception)
                {
                    return writeSucess;
                }
                finally
                {
                    if (streamWriter != null)
                        streamWriter.Close();


                    if (fs != null)
                        fs.Close();

                    retryTimes++;
                }
                if (Total.RefreshSleepTime > 0)
                    Thread.Sleep(Total.RefreshSleepTime);
            }
            while (!writeSucess && retryTimes <= maxRetryTimes);

            return writeSucess;
        }

        delegate bool WriteHtmlFileDelegate(string filePath, string pageContent, int maxRetryTimes);
        public IAsyncResult WriteHtmlFileAsync(string filePath, string pageContent, int maxRetryTimes)
        {

            log.Info("异步更新页面：" + filePath);
            try
            {

                WriteHtmlFileDelegate myDelegate = new WriteHtmlFileDelegate(WriteHtmlFile);
                return myDelegate.BeginInvoke(filePath, pageContent, maxRetryTimes, null, null);

            }
            catch (Exception ex)
            {
                log.Error("异步更新页面出错：", ex);
                UtilMail.SendMailAsync("页面更新出错", ex.Message + "<br/>" + ex.StackTrace, Total.AdminMail, null);

                return null;
            }
        }
        private void UrlToFile()
        {
            int MAX_RETRY_TIMES = 1000;
            StringWriter sw = null;
            string pageContent = "";


            try
            {
                Dictionary<string, string>.KeyCollection urlList = urlFileDictionary.Keys;
                foreach (string url in urlList)
                {
                    //log.Debug("正在生成html文件：" + urlFileDictionary[url]);
                    try
                    {
                        sw = new StringWriter();
                        context.Server.Execute(url, sw, true);

                        pageContent = ConvertPageContent(url, sw.ToString());



                        WriteHtmlFile(urlFileDictionary[url], pageContent);
                    }
                    catch (IOException)
                    {
                        WriteHtmlFileAsync(urlFileDictionary[url], pageContent, MAX_RETRY_TIMES);
                    }
                    catch (Exception ex)
                    {
                        log.Error("生成html文件出错：" + urlFileDictionary[url], ex);
                        UtilMail.SendMailAsync("生成html文件出错：" + urlFileDictionary[url], ex.Message + "<br/>" + ex.StackTrace, Total.AdminMail, null);
                    }
                    if (sw != null)
                        sw.Close();
                }

            }

            finally
            {
                if (sw != null)
                    sw.Close();
            }

        }


    }
}