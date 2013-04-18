using System;
using Spring.Context;
using Spring.Context.Support;
/***************
 * ���񴫲���Ϊ����˵��
PROPAGATION_REQUIRED
�����ǰû�����񣬾��½�һ����������Ѿ�����һ�������У����뵽��������С����������ѡ��
PROPAGATION_SUPPORTS
֧�ֵ�ǰ���������ǰû�����񣬾��Է�����ʽִ�С�
PROPAGATION_MANDATORY
ʹ�õ�ǰ�����������ǰû�����񣬾��׳��쳣��
PROPAGATION_REQUIRES_NEW
�½����������ǰ�������񣬰ѵ�ǰ�������
PROPAGATION_NOT_SUPPORTED
�Է�����ʽִ�в����������ǰ�������񣬾Ͱѵ�ǰ�������
PROPAGATION_NEVER
�Է�����ʽִ�У������ǰ�����������׳��쳣��
PROPAGATION_NESTED
�����ǰ������������Ƕ��������ִ�С������ǰû��������ִ����PROPAGATION_REQUIRED���ƵĲ���
 *
 ******************************************************************************************* */
namespace com.hujun64.logic
{
    /// <summary>
    ///DaoFactory ��ժҪ˵��
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