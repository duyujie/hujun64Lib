using System;
using Spring.Context;
using Spring.Context.Support;
/***************
 * 事务传播行为类型说明
PROPAGATION_REQUIRED
如果当前没有事务，就新建一个事务，如果已经存在一个事务中，加入到这个事务中。这是最常见的选择。
PROPAGATION_SUPPORTS
支持当前事务，如果当前没有事务，就以非事务方式执行。
PROPAGATION_MANDATORY
使用当前的事务，如果当前没有事务，就抛出异常。
PROPAGATION_REQUIRES_NEW
新建事务，如果当前存在事务，把当前事务挂起。
PROPAGATION_NOT_SUPPORTED
以非事务方式执行操作，如果当前存在事务，就把当前事务挂起。
PROPAGATION_NEVER
以非事务方式执行，如果当前存在事务，则抛出异常。
PROPAGATION_NESTED
如果当前存在事务，则在嵌套事务内执行。如果当前没有事务，则执行与PROPAGATION_REQUIRED类似的操作
 *
 ******************************************************************************************* */
namespace com.hujun64.logic
{
    /// <summary>
    ///DaoFactory 的摘要说明
    /// </summary>
    public class ServiceFactory
    {


        private ServiceFactory()
        {
        }


        public static IMainClassService GetMainClassService()
        {

            IApplicationContext context = ContextRegistry.GetContext();

            IMainClassService mainSvc = (IMainClassService)context.GetObject("classService");


            return mainSvc;

        }


        public static IArticleService GetArticleService()
        {

            IApplicationContext context = ContextRegistry.GetContext();

            IArticleService articleSvc = (IArticleService)context.GetObject("articleService");


            return articleSvc;

        }

        public static IGuestbookService GetGuestbookService()
        {

            IApplicationContext context = ContextRegistry.GetContext();

            IGuestbookService guestbookSvc = (IGuestbookService)context.GetObject("guestbookService");


            return guestbookSvc;

        }


        public static ILinkService GetLinkService()
        {

            IApplicationContext context = ContextRegistry.GetContext();

            ILinkService linkSvc = (ILinkService)context.GetObject("linkService");

            return linkSvc;

        }



        public static IClientService GetClientService()
        {
            IApplicationContext context = ContextRegistry.GetContext();

            IClientService clientSvc = (IClientService)context.GetObject("clientService");

            return clientSvc;

        }


        public static ICommonService GetCommonService()
        {
            IApplicationContext context = ContextRegistry.GetContext();

            ICommonService commonSvc = (ICommonService)context.GetObject("commonService");


            return commonSvc;

        }


        public static IAdminService GetAdminService()
        {
            IApplicationContext context = ContextRegistry.GetContext();

            IAdminService adminSvc = (IAdminService)context.GetObject("adminService");


            return adminSvc;

        }


        public static IBackupService GetBackupService()
        {
            IApplicationContext context = ContextRegistry.GetContext();

            IBackupService backupSvc = (IBackupService)context.GetObject("backupService");

            return backupSvc;

        }
        public static ICgwService GetCgwService()
        {
            IApplicationContext context = ContextRegistry.GetContext();

            ICgwService cgwSvc = (ICgwService)context.GetObject("cgwService");

            return cgwSvc;

        }
        public static void RefreshAllCache()
        {
            GetMainClassService().RefreshCachedClass();
            GetArticleService().RefreshCachedArticle();
            GetGuestbookService().RefreshCachedGuestbook();
            GetLinkService().RefreshCachedLink();
            GetCgwService().RefreshCacheCgw();

        }
    }
}