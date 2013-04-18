using System;
using System.Text;
using System.Web;
using System.Collections.Generic;
using com.hujun64.po;
using com.hujun64.type;
using com.hujun64.util;
using com.hujun64.logic;

namespace com.hujun64.logic
{
    /// <summary>
    ///AsyncTask 的摘要说明
    /// </summary>
    public class AsyncTaskService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("AsyncTask");

        private AsyncTaskService()
        {
        }

        delegate void UpdteSiteDelegate(string refreshType);
        delegate void VisitPageDelegate(PageInfo pageInfo);
        delegate void InsertClientDelegate(Client client);
        delegate void GetIpProvinceDelegate();
        delegate void GetClientMobileProvinceDelegate();

        delegate void PostArticle2Hujun64Delegate(Article article);
        delegate void PostGuestbooke2Hujun64Delegate(Guestbook guestbook);
        delegate void DeleteArticle4Hujun64Delegate(string articleId);
        delegate void DeleteGuestbook4Hujun64Delegate(string guestbookId);

        delegate void PostArticle2ShfclawyerDelegate(Article article);
        delegate void PostGuestbooke2ShfclawyerDelegate(Guestbook guestbook);
        delegate void DeleteArticle4ShfclawyerDelegate(string articleId);
        delegate void DeleteGuestbook4ShfclawyerDelegate(string guestbookId);


        public static IAsyncResult UpdateSiteAsync(string refreshType)
        {

            //log.Info("异步页面内容更新...");
            try
            {
                
                UpdteSiteDelegate myDelegate = new UpdteSiteDelegate(UpdteSite);
                return myDelegate.BeginInvoke(refreshType, null, null);


            }
            catch (Exception ex)
            {
                log.Error("异步更新页面出错", ex);
                throw ex;
            }

        }



        public static IAsyncResult VisitPageAsync(PageInfo pageInfo)
        {


            //log.Info("异步记录页面访问数据...");


            try
            {
                VisitPageDelegate myDelegate = new VisitPageDelegate(VisitPage);
                return myDelegate.BeginInvoke(pageInfo, null, null);

            }
            catch (Exception ex)
            {
                log.Error("异步记录页面访问数据出错", ex);
                throw ex;
            }
        }
        public static IAsyncResult GetIpProvinceAsync()
        {

            //log.Info("异步获取IP归属地...");
            try
            {

                GetIpProvinceDelegate myDelegate = new GetIpProvinceDelegate(GetIpProvince);
                return myDelegate.BeginInvoke(null, null);


            }
            catch (Exception ex)
            {
                log.Error("异步获取IP归属地", ex);
                throw ex;
            }

        }

        public static IAsyncResult InsertClientAsync(Client client)
        {

            //log.Info("异步记录客户信息...");
            try
            {

                InsertClientDelegate myDelegate = new InsertClientDelegate(InsertClient);
                return myDelegate.BeginInvoke(client, null, null);


            }
            catch (Exception ex)
            {
                log.Error("异步记录客户信息", ex);
                throw ex;
            }

        }

        public static IAsyncResult GetClientMobileProvinceAsync()
        {

            //log.Info("异步获取客户手机归属地...");
            try
            {

                GetClientMobileProvinceDelegate myDelegate = new GetClientMobileProvinceDelegate(GetClientMobileProvince);
                return myDelegate.BeginInvoke(null, null);


            }
            catch (Exception ex)
            {
                log.Error("异步获取客户手机归属地", ex);
                throw ex;
            }

        }

        private static void UpdteSite(string refreshType)
        {
            try
            {
                ServiceFactory.RefreshAllCache();

                StringBuilder urlSb = new StringBuilder();

                urlSb.Append(Total.CurrentSiteRootUrl);

                urlSb.Append(Total.AspxUrlAsyncTask);

                Dictionary<string, string> formDataDict = new Dictionary<string, string>();
                formDataDict.Add(Total.QueryStringAction, AsyncTaskActionType.UPDATE_SITE.ToString());
                formDataDict.Add(Total.QueryStringRefreshType,refreshType);

                //Util.ExecHttpWebRequestUrlGet(urlSb.ToString(),Total.PageEncoding);
                UtilHttp.ExecHttpWebRequestUrlPost(urlSb.ToString(), UtilHttp.BuildHttpWebRequestPostData(formDataDict), Total.EncodingDefault);
            }
            catch (Exception ex)
            {
                log.Error("更新页面出错", ex);
                throw ex;
            }
        }



        private static void VisitPage(PageInfo pageInfo)
        {
            try
            {
                string MY_SITE_URL = "hujun64.com";

                IClientService clientService = ServiceFactory.GetClientService();



                bool isNewIp = Convert.ToBoolean(clientService.InsertIp(pageInfo.userIp));

                if (pageInfo.pageId != null)
                {
                    switch (pageInfo.pageType)
                    {
                        case PageType.ARTICLE_TYPE:
                        case PageType.FEE_TYPE:
                        case PageType.INTRO_TYPE:
                            ServiceFactory.GetArticleService().ClickArticle(pageInfo.pageId);                          
                            break;
                        case PageType.GUESTBOOK_TYPE:
                            ServiceFactory.GetGuestbookService().ClickGuestbook(pageInfo.pageId);
                         
                            break;
                        default:
                            break;
                    }
                }


                VisitedHistory visitedHistory = new VisitedHistory();
                
                visitedHistory.page_type = pageInfo.pageType;
                visitedHistory.page_id = pageInfo.pageId;
                visitedHistory.click_source_url = pageInfo.clickSource;
                string clickSourceSite = UtilHtml.GetSite(pageInfo.clickSource);
                if (clickSourceSite.EndsWith(pageInfo.httpHost) || clickSourceSite.EndsWith(MY_SITE_URL))
                {
                    visitedHistory.click_source_site = "";
                }
                else
                {
                    visitedHistory.click_source_site = clickSourceSite;
                }

                visitedHistory.ip_from = pageInfo.userIp;

                clientService.InsertVisitedHistory(visitedHistory);


                //通过webservice查找并更新ip地域
                if (isNewIp)
                {
                    string ipProvince = InternetWebService.GetCountryCityByIp(pageInfo.userIp);
                    clientService.UpdateIpProvince(pageInfo.userIp, ipProvince);
                }
            }
            catch (Exception ex)
            {
                log.Error("记录客户信息出错", ex);
                throw ex;
            }
        }


        private static void GetIpProvince()
        {
            try
            {
                IClientService clientService = ServiceFactory.GetClientService();

                clientService.FixIpFrom();

                List<string> ipList = clientService.GetQueryIpFromList();
                if (ipList != null && ipList.Count > 0)
                {
                    foreach (string ip in ipList)
                    {
                        clientService.UpdateIpProvince(ip, InternetWebService.GetCountryCityByIp(ip));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("获取IP归属地出错", ex);
                throw ex;
            }
        }

        private static void InsertClient(Client client)
        {

            IClientService clientService = ServiceFactory.GetClientService();


            clientService.InsertClient(client);

            GetClientMobileProvince();
        }

        private static void GetClientMobileProvince()
        {
            try
            {
                string IgnoreErrorMsg = "免费用户超过查询数量";
                IClientService clientService = ServiceFactory.GetClientService();

                List<Client> clientList = clientService.GetAllClients(null);
                foreach (Client nullClient in clientList)
                {
                    string mobile_province = InternetWebService.GetCountryCityByMobile(nullClient.mobile_code);
                    if (mobile_province.Contains(IgnoreErrorMsg))
                        break;

                    if (!string.IsNullOrEmpty(mobile_province))
                    {
                        if (mobile_province.StartsWith(nullClient.mobile_code))
                        {
                            clientService.UpdateClientMobileProvince(nullClient.mobile_code, mobile_province.Substring(nullClient.mobile_code.Length + 1));
                        }
                        else
                        {
                            clientService.UpdateClientMobileProvince(nullClient.mobile_code, mobile_province);
                            clientService.DeleteClient(nullClient.id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("获取手机好归属地出错", ex);
                throw ex;
            }
        }



        public static IAsyncResult PostArticle2Hujun64Async(Article article)
        {
            try
            {

                PostArticle2Hujun64Delegate myDelegate = new PostArticle2Hujun64Delegate(PostArticle2Hujun64);
                return myDelegate.BeginInvoke(article, null, null);


            }
            catch (Exception ex)
            {
                log.Error("文章同步到hujun64.com出错", ex);
                throw ex;
            }
        }
        private static void PostArticle2Hujun64(Article article)
        {
            InternetWebService.PostArticle2Hujun64(article);
        }





        public static IAsyncResult PostGuestbook2Hujun64Async(Guestbook guestbook)
        {
            try
            {

                PostGuestbooke2Hujun64Delegate myDelegate = new PostGuestbooke2Hujun64Delegate(PostGuestbook2Hujun64);
                return myDelegate.BeginInvoke(guestbook, null, null);


            }
            catch (Exception ex)
            {
                log.Error("留言同步到hujun64.com出错", ex);
                throw ex;
            }
        }
        private static void PostGuestbook2Hujun64(Guestbook guestbook)
        {
            InternetWebService.PostGuestbook2Hujun64(guestbook);
        }
        public static IAsyncResult DeleteArticle4Hujun64Async(string articleId)
        {
            try
            {

                DeleteArticle4Hujun64Delegate myDelegate = new DeleteArticle4Hujun64Delegate(DeleteArticle4Hujun64);
                return myDelegate.BeginInvoke(articleId, null, null);


            }
            catch (Exception ex)
            {
                log.Error("删除hujun64.com文章出错", ex);
                throw ex;
            }
        }
        private static void DeleteArticle4Hujun64(string articleId)
        {
            InternetWebService.DeleteArticle4Hujun64(articleId);
        }
        public static IAsyncResult DeleteGuestbook4Hujun64Async(string guestbookId)
        {
            try
            {

                DeleteGuestbook4Hujun64Delegate myDelegate = new DeleteGuestbook4Hujun64Delegate(DeleteGuestbook4Hujun64);
                return myDelegate.BeginInvoke(guestbookId, null, null);


            }
            catch (Exception ex)
            {
                log.Error("删除hujun64.com留言出错", ex);
                throw ex;
            }
        }
        private static void DeleteGuestbook4Hujun64(string guestbookId)
        {
            InternetWebService.DeleteGuestbook4Hujun64(guestbookId);
        }
        public static IAsyncResult PostArticle2ShfclawyerAsync(Article article)
        {
            try
            {

                PostArticle2ShfclawyerDelegate myDelegate = new PostArticle2ShfclawyerDelegate(PostArticle2Shfclawyer);
                return myDelegate.BeginInvoke(article, null, null);


            }
            catch (Exception ex)
            {
                log.Error("文章同步到shfclawyer.com出错", ex);
                throw ex;
            }
        }
        private static void PostArticle2Shfclawyer(Article article)
        {
            InternetWebService.PostArticle2Shfclawyer(article);
        }
        public static IAsyncResult PostGuestbook2ShfclawyerAsync(Guestbook guestbook)
        {
            try
            {

                PostGuestbooke2ShfclawyerDelegate myDelegate = new PostGuestbooke2ShfclawyerDelegate(PostGuestbook2Shfclawyer);
                return myDelegate.BeginInvoke(guestbook, null, null);


            }
            catch (Exception ex)
            {
                log.Error("留言同步到shfclawyer.com出错", ex);
                throw ex;
            }
        }
        private static void PostGuestbook2Shfclawyer(Guestbook guestbook)
        {
            InternetWebService.PostGuestbook2Shfclawyer(guestbook);
        }
        public static IAsyncResult DeleteArticle4ShfclawyerAsync(string articleId)
        {
            try
            {

                DeleteArticle4ShfclawyerDelegate myDelegate = new DeleteArticle4ShfclawyerDelegate(DeleteArticle4Shfclawyer);
                return myDelegate.BeginInvoke(articleId, null, null);


            }
            catch (Exception ex)
            {
                log.Error("删除hujun64.com文章出错", ex);
                throw ex;  
            }
        }
        private static void DeleteArticle4Shfclawyer(string articleId)
        {
            InternetWebService.DeleteArticle4Shfclawyer(articleId);
        }
        public static IAsyncResult DeleteGuestbook4ShfclawyerAsync(string guestbookId)
        {
            try
            {

                DeleteGuestbook4ShfclawyerDelegate myDelegate = new DeleteGuestbook4ShfclawyerDelegate(DeleteGuestbook4Shfclawyer);
                return myDelegate.BeginInvoke(guestbookId, null, null);


            }
            catch (Exception ex)
            {
                log.Error("删除hujun64.com留言出错", ex);
                throw ex;
            }
        }
        private static void DeleteGuestbook4Shfclawyer(string guestbookId)
        {
            InternetWebService.DeleteGuestbook4Shfclawyer(guestbookId);
        }
    }
}