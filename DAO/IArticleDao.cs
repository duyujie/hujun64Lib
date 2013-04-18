using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using com.hujun64.po;
using Wuqi.Webdiyer;
namespace com.hujun64.Dao
{
    /// <summary>
    ///IArticleDao 的摘要说明
    /// </summary>
    public interface IArticleDao
    {

        SearchResult SearchArticle(string key, string searchType);

        int CountArticleSite(string articleId);
        List<string> GetUnstaticBigClassIdList();
        List<string> GetUnstaticModuleClassIdList();
        List<string> GetUnstaticSmallClassIdList();

        bool ExistsArticlePicture(string articlePictureId);
        bool ExistsId(string articleId);
        bool ExistsRefId(string articleId);
        bool ExistsArticleSite(string articleId,string siteId);
        bool ExistsArticleRefSite(string articleRefId, string siteId);
        bool ExistsTitle(string articleTitle, bool isEnabled);
        void IncreaseClick(string articleId);
        int CountArticle(string moduleClassId, string bigClassId, string smallClassId);
        bool IsArticleRef(string articleId);

        List<string> GetAllAuthorsList();
        List<ArticleBase> GetAllArticlesIndex();
        List<Article> GetAllArticle(bool isWithContent);
        List<ArticlePicture> GetAllArticlePicture();
        List<Article> GetAllDeletedArticle(bool isWithContent);
        Dictionary<string, List<string>> GetArticleSiteDict();
        List<Lawyer> GetAllLawyer();

        int GetMaxArticleIdSeq();
        int GetMaxArticleSortSeq();
        Article GetArticle(string articleId, bool isWithContent);
        Article GetArticleByTitle(string articleTitle, bool isWithContent);
        Article GetArticleByModueSeq(string moduleClassId, int sortSeq);
        Article GetDeletedArticle(string articleId, bool isWithContent);
        ArticlePicture GetArticlePicture(string articleId);

        Lawyer GetLawyerInfoByArticleId(string articleId);

    
        Dictionary<string, DateTime> GetAllArticleIdDict(bool isRefreshAll);
        Dictionary<string, DateTime> GetAllIntroIdDict(bool isRefreshAll);
        Dictionary<string, DateTime> GetAllFeeIdDict(bool isRefreshAll);


        int InsertArticleSite(string articleId, string siteId);
        int UpdateArticleSite(string articleId, IList<string> siteIdList);
        int InsertArticleRefSite(string articleId, string siteId);
        int UpdateArticleRefSite(string articleId, IList<string> siteIdList);
        int InsertArticlePicture(ArticlePicture articlePicture);
        int UpdateArticlePicture(ArticlePicture articlePicture);
        int DeleteArticlePicture(string articlePictureId);

        int InsertArticle(Article article);
        int InsertArticleRef(Article articleRef);
        int UpdateArticleBigClass(string articleId, string bigClassId);
        int UpdateArticle(Article article);
        int UpdateArticleRef(Article article);
        int UpdateAllStatic(bool isStatic, DateTime timestamp);
        bool DeleteArticle(Article article);
        bool DeleteArticleRef(string articleId);
        int PurgeArticle(string articleId);
        int DeleteArticleSite(string articleId, string siteId);
        int DeleteArticleRefSite(string articleId, string siteId);
        int UpdateSortSeq(string keyId, int sortSeq);

        int InsertLawyerInfo(Lawyer lawyer);
        int UpdateLawyerInfo(Lawyer lawyer);
        int DeleteLawyerInfo(string lawyerId);


        int GetMinMaxSortSeqOfModule(string moduleClassId, bool isMax);

       

    }
}