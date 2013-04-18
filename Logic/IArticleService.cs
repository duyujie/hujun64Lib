using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.po;

namespace com.hujun64.logic
{
    public interface IArticleService
    {
       
        List<Article> GetArticleList(string articleId);
        List<Article> GetArticleList(string articleId,bool isWithContent);
        List<Article> GetArticleListLikeTitle(string articleTitle);
        List<Article> GetTopArticleByModuleName(string moduleName, int count);
        List<Article> GetTopArticleByModuleId(string moduleId, int count);
        List<Article> GetTopArticle(string bigClassId, string smallClassId, string moduleClassId, int topBegin, int count, out int totalCount, bool ingoreNull);
        List<Article> GetTopArticle(string bigClassId, string smallClassId, string moduleClassId, int topBegin, int count, out int totalCount);
        List<Article> GetTopArticle(string bigClassId, string smallClassId, string moduleClassId, int topBegin, int count);
        List<Article> GetTopArticleByBigClassName(string bigClassName, int count);
        List<Article> GetTopArticleByBigClassId(string bigClassId, int count);
        List<Article> GetTopPictureArticle(string classId,int topBegin, int count);
        List<Lawyer> GetAllLawyer();

        List<Article> GetDeletedArticleList(bool isWithContent);
        Dictionary<string, List<string>> GetArticleSiteDict();
        SearchResult SearchArticle(string key, string searchType);
        

        Article GetRefArticle(string articleId);
        string GenerateId();
        int ClickArticle(string articleId);
        bool ExistsTitle(string articleTitle, bool isEnabled);
     
        int CountArticle(string moduleClassId, string bigClassId, string smallClassId);
        bool IsArticleRef(string articleId);

        Article GetArticleByTitle(string articleTitle);
        List<string> GetAllAuthorsList();
        List<ArticleBase> GetAllArticlesIndex();
        List<Article> GetAllArticle(bool isWithContent);
        void RefreshCachedArticle();
        Article GetArticle(string articleId);
        Article GetArticle(string articleId, bool isWithContent);
        Article GetDeletedArticle(string articleId, bool isWithContent);
        Article GetArticleByTitle(string articleTitle, bool isWithContent);
        Article GetArticleByModueSeq(string moduleClassId, int sortSeq);

       

        int GetMinSortSeqOfModule(string moduleClassId);
        int GetMaxSortSeqOfModule(string moduleClassId);

        void GetNeighborArticle(string articleId, out Article prevArticle, out Article nextArticle);

        Dictionary<string, DateTime> GetAllArticleIdDict(bool isRefreshAll);
        Dictionary<string, DateTime> GetAllIntroIdDict(bool isRefreshAll);
        Dictionary<string, DateTime> GetAllFeeIdDict(bool isRefreshAll);


        bool InsertArticle(Article article);
        bool UpdateArticle(Article article);
        int UpdateAllStatic(bool isStatic, DateTime timestamp);
        bool DeleteArticle(string articleId);
        bool PurgeArticle(string articleId, string siteId);
        bool PurgeArticle(string articleId, IList<string> siteIdList);
        bool UpdateSortSeq(string articleId, int sortSeq);

        bool InsertArticleRef(Article articleRef);

        bool InsertOrUpdateLawyerInfo(Lawyer lawyer);

    }
}
