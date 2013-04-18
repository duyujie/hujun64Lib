using System;
using System.Collections.Generic;
using com.hujun64.Dao;
using com.hujun64.po;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
namespace com.hujun64.logic
{

    /// <summary>
    ///UtilBackup 的摘要说明
    /// </summary>
    internal class BackupService : IBackupService
    {
        public IBackupDao backupDao { get; set; }

        public IArticleDao articleDao { get; set; }
        public IArticleDao mirrorArticleDao { get; set; }
        public IGuestbookDao guestbookDao { get; set; }
        public IGuestbookDao mirrorGuestbookDao { get; set; }
        public ICommonDao commonDao { get; set; }
        public ICommonDao mirrorCommonDao { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BackupService");


        //备份新增的文章和留言到镜像/备份数据库
        [Transaction(TransactionPropagation.Required)]
        public void BackupArticleGuestbook()
        {
            if (!Total.IsSyncMirror || Total.IsMirrorSite)
                return;


            if (backupDao.IsNeedBackup() && ServiceFactory.GetCommonService().CheckSiteStatus(Total.MonitorUrl))
            {
                List<BackupPool> poolList = backupDao.GetAllBackupPool();
                foreach (BackupPool pool in poolList)
                {

                    if (pool.type == Total.PrefixArticleId)
                    {
                        this.BackupArticle(pool);
                    }
                    else if (pool.type == Total.PrefixGuestbookId)
                    {
                        this.BackupGuestbook(pool);
                    }
                    backupDao.ClearBackupPool(pool.id, pool.site_id);
                }



                this.BackupGlobalSeq();
            }
        }
        [Transaction(TransactionPropagation.Required)]
        private void MirrorUpdateArticle(Article article)
        {
            MirrorInsertOrUpdateArticlePicture(article.articlePicture);
            mirrorArticleDao.UpdateArticle(article);
        }
        [Transaction(TransactionPropagation.Required)]
        private void MirrorInsertOrUpdateArticlePicture(ArticlePicture articlePicture)
        {
            if (articlePicture != null)
            {
                if (!mirrorArticleDao.ExistsArticlePicture(articlePicture.id))
                    mirrorArticleDao.InsertArticlePicture(articlePicture);
                else
                    mirrorArticleDao.UpdateArticlePicture(articlePicture);
            }
        }
        [Transaction(TransactionPropagation.Required)]
        private void MirrorInsertArticle(Article article, string siteId)
        {
            if (article != null && !string.IsNullOrEmpty(article.id))
            {
                MirrorInsertOrUpdateArticlePicture(article.articlePicture);

                if (string.IsNullOrEmpty(article.ref_id))
                {
                    if (!mirrorArticleDao.ExistsId(article.id))
                        mirrorArticleDao.InsertArticle(article);
                    if (!mirrorArticleDao.ExistsArticleSite(article.id, siteId))
                        mirrorArticleDao.InsertArticleSite(article.id, siteId);
                }
                else
                {
                    if (!mirrorArticleDao.ExistsRefId(article.id))
                        mirrorArticleDao.InsertArticleRef(article);
                    if (!mirrorArticleDao.ExistsArticleRefSite(article.id, siteId))
                        mirrorArticleDao.InsertArticleRefSite(article.id, siteId);
                }
            }
        }
        [Transaction(TransactionPropagation.Required)]
        private void BackupArticle(BackupPool pool)
        {
            try
            {
                if (pool.todo.Equals("I"))
                {
                    Article article = articleDao.GetArticle(pool.id, true);
                    MirrorInsertArticle(article, pool.site_id);
                }
                else if (pool.todo.Equals("U"))
                {
                    Article article = articleDao.GetArticle(pool.id, true);
                    if (!string.IsNullOrEmpty(article.id))
                    {
                        if (string.IsNullOrEmpty(article.ref_id))
                        {
                            if (!mirrorArticleDao.ExistsId(article.id) || !mirrorArticleDao.ExistsArticleSite(article.id, pool.site_id))
                                MirrorInsertArticle(article, pool.site_id);
                            else
                                MirrorUpdateArticle(article);
                        }
                        else
                        {
                            if (!mirrorArticleDao.ExistsId(article.id) || !mirrorArticleDao.ExistsArticleRefSite(article.id, pool.site_id))
                                MirrorInsertArticle(article, pool.site_id);
                            else
                                MirrorUpdateArticle(article);
                        }
                    }
                }
                else if (pool.todo.Equals("D"))
                {
                    Article mirrorArticle = mirrorArticleDao.GetArticle(pool.id, false);
                    if (string.IsNullOrEmpty(mirrorArticle.ref_id))
                    {
                        mirrorArticleDao.DeleteArticleSite(mirrorArticle.id, pool.site_id);
                        mirrorArticleDao.DeleteArticle(mirrorArticle);
                    }
                    else
                    {
                        mirrorArticleDao.DeleteArticleRefSite(mirrorArticle.id, pool.site_id);
                        mirrorArticleDao.DeleteArticleRef(pool.id);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Backup guestbook error!", ex);
                throw ex;
            }
        }
        [Transaction(TransactionPropagation.Required)]
        private void BackupGuestbook(BackupPool pool)
        {
            try
            {

                if (pool.todo.Equals("I"))
                {
                    Guestbook guestbook = guestbookDao.GetGuestbook(pool.id, true);
                    if (guestbook != null && !string.IsNullOrEmpty(guestbook.id))
                    {
                        if (!mirrorGuestbookDao.ExistsId(guestbook.id))
                            mirrorGuestbookDao.InsertGuestbook(guestbook);
                        if (!mirrorGuestbookDao.ExistsGuestbookSite(guestbook.id, pool.site_id))
                            mirrorGuestbookDao.InsertGuestbookSite(pool.id, pool.site_id);
                    }
                }
                else if (pool.todo.Equals("U"))
                {
                    Guestbook guestbook = guestbookDao.GetGuestbook(pool.id, true);
                    if (guestbook != null && !string.IsNullOrEmpty(guestbook.id))
                    {
                        if (!mirrorGuestbookDao.ExistsId(guestbook.id))
                            mirrorGuestbookDao.InsertGuestbook(guestbook);
                        else
                            mirrorGuestbookDao.UpdateGuestbook(guestbook);

                        if (!mirrorGuestbookDao.ExistsGuestbookSite(guestbook.id, pool.site_id))
                            mirrorGuestbookDao.InsertGuestbookSite(pool.id, pool.site_id);


                    }
                }
                else if (pool.todo.Equals("D"))
                {
                    Guestbook mirrorGuestbook = mirrorGuestbookDao.GetGuestbook(pool.id, false);
                    if (mirrorGuestbook != null && !string.IsNullOrEmpty(mirrorGuestbook.id))
                    {
                        mirrorGuestbookDao.DeleteGuestbookSite(pool.id, pool.site_id);
                        mirrorGuestbookDao.DeleteGuestbook(pool.id);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Backup article error!", ex);
                throw ex;
            }
        }
        [Transaction(TransactionPropagation.Required)]
        private void BackupGlobalSeq()
        {
            try
            {
                string[] backupClassNameArray = new string[] { "article", "article_sort", "ArticleDelete", "guestbook" };
                List<GlobalSeq> globalSeqList = commonDao.GetAllGlobalSeq();
                List<GlobalSeq> backupGlobalSeqList = new List<GlobalSeq>();
                Dictionary<string, int> seqDict = new Dictionary<string, int>();
                foreach (GlobalSeq globalSeq in globalSeqList)
                {
                    foreach (string backupClassName in backupClassNameArray)
                    {
                        if (globalSeq.class_name == backupClassName)
                        {
                            backupGlobalSeqList.Add(globalSeq);

                        }
                    }
                }

                mirrorCommonDao.UpdateGlobalSeq(backupGlobalSeqList);
            }
            catch (Exception ex)
            {
                log.Error("Backup global_seq error!", ex);
                throw ex;
            }
        }

        
    }
}