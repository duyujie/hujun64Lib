using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using com.hujun64.po;
using com.hujun64.util;
using com.hujun64.type;
using Spring.Data.Core;
using Spring.Data.Common;

namespace com.hujun64.Dao
{
    /// <summary>
    ///ArticleDao 的摘要说明
    /// </summary>
    public class ArticleDao : AdoDaoSupport, IArticleDao
    {

        public int GetMinMaxSortSeqOfModule(string moduleClassId, bool isMax)
        {


            StringBuilder sb = new StringBuilder("select ");
            if (isMax)
            {
                sb.Append("isnull(max(sort_seq),-1)");
            }
            else
            {
                sb.Append("isnull(min(sort_seq),-1)");
            }
            sb.Append(" from view_article where module_class_id=@module_class_id and site_id=@site_id");

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("module_class_id", moduleClassId);
            dbParameters.AddWithValue("site_id", Total.SiteId);
            return (int)AdoTemplate.ExecuteScalar(CommandType.Text, sb.ToString(), dbParameters);





        }
        public int GetMaxArticleIdSeq()
        {
            string sql = "select max(convert(int,substring(id,3,len(id)))) from  view_article_all";

            return (int)AdoTemplate.ExecuteScalar(CommandType.Text, sql);
        }
        public int GetMaxArticleSortSeq()
        {
            string sql = "select isnull(max(sort_seq),0) from  view_article_all";

            return (int)AdoTemplate.ExecuteScalar(CommandType.Text, sql);
        }
        public Article GetArticleByModueSeq(string moduleClassId, int sortSeq)
        {
            if (string.IsNullOrEmpty(moduleClassId))
                return new Article();



            Article article = null;


            string sql = "select * from view_article where module_class_id=@module_class_id and sort_seq=@sort_seq and site_id=@site_id";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("module_class_id", moduleClassId);
            dbParameters.AddWithValue("site_id", Total.SiteId);
            dbParameters.AddWithValue("sort_seq", sortSeq);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);




            if (ds.Tables[0].Rows.Count > 0)
            {
                article = CloneFromDataRow(ds.Tables[0].Rows[0], true);

            }

            if (article == null)
                article = new Article();

            return article;



        }
        public bool DeleteArticleRef(string articleId)
        {

            string sql = "delete from site_article_ref where article_id=@articleId delete from article_ref where id=@articleId";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);


            return Convert.ToBoolean(AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters));

        }
        public bool DeleteArticle(Article article)
        {
            if (article == null || string.IsNullOrEmpty(article.id))
                return false;


            string sql = "update article set enabled=0,title=@newTitle where id=@articleId;";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", article.id);
            dbParameters.AddWithValue("newTitle", article.title);

            return Convert.ToBoolean(AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters));

        }
        public int CountArticleSite(string articleId)
        {

            string sql = "select count(1) from site_article where article_id=@articleId";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);

            return (int)AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters);



        }
        public int PurgeArticle(string articleId)
        {
            string sql = "delete from article where id=@articleId and enabled=0";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int DeleteArticleSite(string articleId, string siteId)
        {

            string sql = "delete from site_article where article_id=@articleId and site_id=@siteId";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);
            dbParameters.AddWithValue("siteId", siteId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);


        }
        public int DeleteArticleRefSite(string articleId, string siteId)
        {

            string sql = "delete from site_article_ref where article_id=@articleId and site_id=@siteId";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);
            dbParameters.AddWithValue("siteId", siteId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);


        }
        public Dictionary<string, DateTime> GetAllIntroIdDict(bool isRefreshAll)
        {
            Dictionary<string, DateTime> introArticleIdDict = new Dictionary<string, DateTime>();



            string sql = "select distinct id,last_mod from view_intro_article where site_id=@site_id";
            if (!isRefreshAll)
            {
                sql += " and is_static=0";
            }

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                introArticleIdDict.Add(row["id"].ToString(), (DateTime)row["last_mod"]);
            }

            return introArticleIdDict;

        }
        public Dictionary<string, DateTime> GetAllFeeIdDict(bool isRefreshAll)
        {
            Dictionary<string, DateTime> introArticleIdDict = new Dictionary<string, DateTime>();


            string sql = "select distinct id,last_mod from view_fee_article where site_id=@site_id";
            if (!isRefreshAll)
            {
                sql += " and is_static=0";
            }

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            foreach (DataRow row in ds.Tables[0].Rows)
            {

                introArticleIdDict.Add(row["id"].ToString(), (DateTime)row["last_mod"]);
            }

            return introArticleIdDict;

        }
        public Dictionary<string, DateTime> GetAllArticleIdDict(bool isRefreshAll)
        {
            Dictionary<string, DateTime> articleIdDict = new Dictionary<string, DateTime>();


            string sql = "select distinct id,last_mod from view_pure_article where site_id=@site_id";
            if (!isRefreshAll)
            {
                sql += " and is_static=0";
            }
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            foreach (DataRow row in ds.Tables[0].Rows)
            {

                articleIdDict.Add(row["id"].ToString(), (DateTime)row["last_mod"]);
            }

            return articleIdDict;

        }

        public List<Article> GetAllArticle(bool isWithContent)
        {


            string sql;

            if (isWithContent)
            {
                sql = "select * from view_article where site_id=@site_id order by sort_seq desc";
            }
            else
            {
                sql = "select id,author,title,keywords,class_id,big_class_id,module_class_id,is_all_class,news_from,addtime,is_static,enabled,sort_seq,ref_id,last_mod,enabled,click from view_article where site_id=@site_id  order by sort_seq desc";
            }


            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);


            List<Article> articleList = new List<Article>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Article article = CloneFromDataRow(row, isWithContent);

                articleList.Add(article);

            }

            return articleList;
        }
        public Article GetArticle(string articleId, bool isWithContent)
        {
            if (string.IsNullOrEmpty(articleId))
                return new Article();

            Article article = null;

            string sql;
            if (isWithContent)
            {
                sql = "select * from view_article where id=@articleId";
            }
            else
            {
                sql = "select id,author,title,keywords,class_id,big_class_id,module_class_id,is_all_class,news_from,addtime,is_static,enabled,sort_seq,ref_id,last_mod,enabled,click,pic_id,pic_url,pic_alt from view_article  where id=@articleId";
            }



            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);



            if (ds.Tables[0].Rows.Count > 0)
            {

                article = CloneFromDataRow(ds.Tables[0].Rows[0], isWithContent);

            }


            if (article == null)
                article = new Article();


            if (string.IsNullOrEmpty(article.big_class_id) && !string.IsNullOrEmpty(article.class_id))
            {
                article.big_class_id = logic.ServiceFactory.GetMainClassService().GetBigClassBySmall(article.class_id).id;
            }

            return article;
        }
        public ArticlePicture GetArticlePicture(string pictureId)
        {
            string sql = "select * from article_picture where id=@pictureId";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("pictureId", pictureId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);


            ArticlePicture articlePicture = new ArticlePicture();


            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                articlePicture.id = row["id"].ToString();
                articlePicture.pic_url = row["pic_url"].ToString();
                articlePicture.pic_alt = row["pic_alt"].ToString();

            }
            return articlePicture;
        }
        public List<ArticlePicture> GetAllArticlePicture()
        {
            List<ArticlePicture> articlePictureList = new List<ArticlePicture>();


            string sql = "select * from article_picture";

            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sql);


            foreach (DataRow row in ds.Tables[0].Rows)
            {

                ArticlePicture articlePicture = new ArticlePicture();
                articlePicture.id = row["id"].ToString();
                articlePicture.pic_url = row["pic_url"].ToString();
                articlePicture.pic_alt = row["pic_alt"].ToString();

                articlePictureList.Add(articlePicture);
            }




            return articlePictureList;

        }
        public List<Lawyer> GetAllLawyer()
        {
            List<Lawyer> lawyerList = new List<Lawyer>();


            string sql = "select * from lawyer_info order by sort_seq";

            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sql);


            foreach (DataRow row in ds.Tables[0].Rows)
            {

                Lawyer lawyer = new Lawyer();
                lawyer.id = row["id"].ToString();
                lawyer.lawyer_name = row["lawyer_name"].ToString();
                lawyer.category = row["category"].ToString();
                lawyer.article_id = row["article_id"].ToString();

                lawyerList.Add(lawyer);
            }




            return lawyerList;

        }
        public List<Article> GetAllDeletedArticle(bool isWithContent)
        {
            List<Article> articleList = new List<Article>();


            string sql;
            if (isWithContent)
            {
                sql = "select *,ref_id='' from view_article_all where  enabled=0";
            }
            else
            {
                sql = "select distinct id,author,title,keywords,class_id,big_class_id,module_class_id,is_all_class,news_from,addtime,is_static,enabled,sort_seq,ref_id='',last_mod,enabled,click,pic_id,pic_url,pic_alt from view_article_all  where enabled=0";
            }




            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sql);


            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Article article = CloneFromDataRow(row, isWithContent);
                if (!articleList.Contains(article))
                    articleList.Add(article);

            }




            return articleList;

        }
        public Article GetDeletedArticle(string articleId, bool isWithContent)
        {
            if (string.IsNullOrEmpty(articleId))
                return new Article();



            Article article = null;
            string sql;
            if (isWithContent)
            {
                sql = "select *,ref_id='' from view_article_all where id=@articleId and enabled=0";
            }
            else
            {
                sql = "select id,author,title,keywords,class_id,big_class_id,module_class_id,is_all_class,news_from,addtime,is_static,enabled,sort_seq,ref_id='',last_mod,enabled,click,pic_id,pic_url,pic_alt from view_article_all  where id=@articleId and enabled=0";
            }
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);


            if (ds.Tables[0].Rows.Count > 0)
            {

                article = CloneFromDataRow(ds.Tables[0].Rows[0], isWithContent);

            }

            if (article == null)
                article = new Article();

            return article;

        }

        public int UpdateArticleSite(string articleId, IList<string> siteIdList)
        {

            StringBuilder sqlSb = new StringBuilder();
            sqlSb.Append(" delete from site_article where article_id=@article_id and site_id not in (''");
            foreach (string siteId in siteIdList)
            {
                sqlSb.Append(",'");
                sqlSb.Append(siteId);
                sqlSb.Append("'");
            }
            sqlSb.Append(")");
            foreach (string siteId in siteIdList)
            {
                sqlSb.Append(" insert into site_article (article_id,site_id) select @article_id,'");
                sqlSb.Append(siteId);
                sqlSb.Append("' where not exists (select 1 from site_article where article_id=@article_id and site_id='");
                sqlSb.Append(siteId);
                sqlSb.Append("')");
            }
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("article_id", articleId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlSb.ToString(), dbParameters);


        }
        public int UpdateArticleRefSite(string articleId, IList<string> siteIdList)
        {
            StringBuilder sqlSb = new StringBuilder();
            sqlSb.Append(" delete from site_article_ref where article_id=@article_id and site_id not in (''");
            foreach (string siteId in siteIdList)
            {
                sqlSb.Append(",'");
                sqlSb.Append(siteId);
                sqlSb.Append("'");
            }
            sqlSb.Append(")");
            foreach (string siteId in siteIdList)
            {
                sqlSb.Append(" insert into site_article_ref (article_id,site_id) select @article_id,'");
                sqlSb.Append(siteId);
                sqlSb.Append("' where not exists (select 1 from site_article_ref where article_id=@article_id and site_id='");
                sqlSb.Append(siteId);
                sqlSb.Append("')");
            }
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("article_id", articleId);
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlSb.ToString(), dbParameters);



        }
        public int InsertArticleSite(string articleId, string siteId)
        {

            string sql = "insert into site_article (site_id,article_id) select @site_id,@article_id  where not exists (select 1 from site_article where article_id=@article_id and site_id=@site_id)";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("article_id", articleId);
            dbParameters.AddWithValue("site_id", siteId);
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);



        }
        public int InsertArticleRefSite(string articleId, string siteId)
        {

            string sql = "insert into site_article_ref (site_id,article_id) select @site_id,@article_id  where not exists (select 1 from site_article where article_id=@article_id and site_id=@site_id)";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("article_id", articleId);
            dbParameters.AddWithValue("site_id", siteId);
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

        public int InsertArticle(Article article)
        {

            string sql = "insert into article (id,class_id,title,keywords,content,author,news_from,big_class_id,module_class_id,is_all_class,addtime,sort_seq,click,enabled,last_mod,last_visited_time,pic_id,is_static) values (@article_id,@class_id,@title,@keywords,@content,@author,@news_from,@big_class_id,@module_class_id,@is_all_class,@addtime,@sort_seq,0,1,@last_mod,@last_visited_time,@pic_id,0)";



            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("article_id", article.id);

            if (string.IsNullOrEmpty(article.class_id))
            {
                dbParameters.AddWithValue("class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("class_id", article.class_id);
            }
            if (string.IsNullOrEmpty(article.big_class_id))
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", article.big_class_id);
            }
            if (string.IsNullOrEmpty(article.module_class_id))
            {
                dbParameters.AddWithValue("module_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("module_class_id", article.module_class_id);
            }
            dbParameters.AddWithValue("is_all_class", Convert.ToInt32(article.is_all_class));
            dbParameters.AddWithValue("title", article.title);
            dbParameters.AddWithValue("keywords", article.keywords);
            dbParameters.AddWithValue("content", article.content);
            dbParameters.AddWithValue("author", article.author);
            dbParameters.AddWithValue("news_from", article.news_from);
            dbParameters.AddWithValue("sort_seq", article.sort_seq);



            if (article.addtime == null || article.addtime == DateTime.MinValue)
            {
                dbParameters.AddWithValue("addtime", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("addtime", article.addtime);
            }

            if (article.last_mod == null || article.last_mod == DateTime.MinValue)
            {
                dbParameters.AddWithValue("last_mod", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("last_mod", article.last_mod);
            }

            if (article.last_visited_time == null || article.last_visited_time == DateTime.MinValue)
            {
                dbParameters.AddWithValue("last_visited_time", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("last_visited_time", article.last_visited_time);
            }
            if (article.articlePicture != null)
            {
                dbParameters.AddWithValue("pic_id", article.articlePicture.id);
            }
            else
            {
                dbParameters.AddWithValue("pic_id", DBNull.Value);
            }


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int InsertLawyerInfo(Lawyer lawyer)
        {
            string sql = "insert into lawyer_info (id,lawyer_name,category,article_id,sort_seq) values (@id,@lawyer_name,@category,@article_id,@sort_seq)";


            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("id", lawyer.id);
            dbParameters.AddWithValue("lawyer_name", lawyer.lawyer_name);
            dbParameters.AddWithValue("category", lawyer.category);
            dbParameters.AddWithValue("article_id", lawyer.article_id);
            dbParameters.AddWithValue("sort_seq", lawyer.sort_seq);


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }
        public int UpdateLawyerInfo(Lawyer lawyer)
        {
            string sql = "update lawyer_info set lawyer_name=@lawyer_name,category=@category,sort_seq=@sort_seq where article_id=@article_id";


            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("lawyer_name", lawyer.lawyer_name);
            dbParameters.AddWithValue("category", lawyer.category);
            dbParameters.AddWithValue("article_id", lawyer.article_id);
            dbParameters.AddWithValue("sort_seq", lawyer.sort_seq);


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }

        public Lawyer GetLawyerInfoByArticleId(string articleId)
        {
            string sql = "select * from lawyer_info where article_id=@article_id";


            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("article_id", articleId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);


            Lawyer lawyer = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                lawyer = new Lawyer();
                lawyer.id = row["id"].ToString();
                lawyer.lawyer_name = row["lawyer_name"].ToString();
                lawyer.category = row["category"].ToString();
                lawyer.article_id = row["article_id"].ToString();
                lawyer.sort_seq = (int)row["sort_seq"];

            }

            return lawyer;
        }
        public int DeleteLawyerInfo(string lawyerId)
        {
            string sql = "delete from lawyer_info where id=@id";


            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("id", lawyerId);


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }
        public int UpdateSortSeq(string articleId, int sortSeq)
        {

            bool isArticleRef = IsArticleRef(articleId);




            string sql;

            if (isArticleRef)
            {
                sql = "update article_ref set sort_seq=@sort_seq where id=@articleId";
            }
            else
            {
                sql = "update article set sort_seq=@sort_seq where id=@articleId";
            }
            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("articleId", articleId);

            dbParameters.AddWithValue("sort_seq", sortSeq);


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);


        }
        public int UpdateArticleRef(Article article)
        {


            string sql = "update article_ref set ref_id=@ref_id,class_id=@class_id,big_class_id=@big_class_id,module_class_id=@module_class_id,is_all_class=@is_all_class,addtime=@addtime,is_static=@is_static,enabled=@enabled,sort_seq=@sort_seq where id=@articleId";

            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("articleId", article.id);
            if (article.class_id == "" || article.class_id == null)
            {
                dbParameters.AddWithValue("class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("class_id", article.class_id);
            }
            if (article.big_class_id == "" || article.big_class_id == null)
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", article.big_class_id);
            }
            if (article.module_class_id == "" || article.module_class_id == null)
            {
                dbParameters.AddWithValue("module_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("module_class_id", article.module_class_id);
            }
            dbParameters.AddWithValue("is_all_class", Convert.ToInt32(article.is_all_class));
            dbParameters.AddWithValue("ref_id", article.ref_id);
            dbParameters.AddWithValue("is_static", Convert.ToInt32(article.is_static));
            dbParameters.AddWithValue("enabled", Convert.ToInt32(article.enabled));
            if (DateTime.MinValue.Equals(article.addtime))
            {
                dbParameters.AddWithValue("addtime", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("addtime", article.addtime);
            }
            dbParameters.AddWithValue("sort_seq", article.sort_seq);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int UpdateArticleBigClass(string articleId, string bigClassId)
        {


            string sql = "update article set  big_class_id=@big_class_id where id=@articleId";


            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);

            if (string.IsNullOrEmpty(bigClassId))
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", bigClassId);
            }


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int UpdateArticle(Article article)
        {



            string sql = "update article set class_id=@class_id,big_class_id=@big_class_id,module_class_id=@module_class_id,is_all_class=@is_all_class,title=@title,keywords=@keywords,author=@author,news_from=@news_from,content=@content,addtime=@addtime,is_static=@is_static,enabled=@enabled,sort_seq=@sort_seq,last_mod=getdate(),pic_id=@pic_id where id=@articleId";


            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", article.id);
            if (string.IsNullOrEmpty(article.class_id))
            {
                dbParameters.AddWithValue("class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("class_id", article.class_id);
            }
            if (string.IsNullOrEmpty(article.big_class_id))
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", article.big_class_id);
            }
            if (string.IsNullOrEmpty(article.module_class_id))
            {
                dbParameters.AddWithValue("module_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("module_class_id", article.module_class_id);
            }
            dbParameters.AddWithValue("is_all_class", Convert.ToInt32(article.is_all_class));
            dbParameters.AddWithValue("title", article.title);
            dbParameters.AddWithValue("keywords", article.keywords);
            dbParameters.AddWithValue("content", article.content);
            dbParameters.AddWithValue("author", article.author);
            dbParameters.AddWithValue("news_from", article.news_from);
            dbParameters.AddWithValue("is_static", Convert.ToInt32(article.is_static));
            dbParameters.AddWithValue("enabled", Convert.ToInt32(article.enabled));
            if (DateTime.MinValue.Equals(article.addtime))
            {
                dbParameters.AddWithValue("addtime", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("addtime", article.addtime);
            }
            dbParameters.AddWithValue("sort_seq", article.sort_seq);
            if (article.articlePicture != null)
            {
                dbParameters.AddWithValue("pic_id", article.articlePicture.id);
            }
            else
            {
                dbParameters.AddWithValue("pic_id", DBNull.Value);
            }


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

        public void IncreaseClick(string articleId)
        {

            if (string.IsNullOrEmpty(articleId))
                return;



            StringBuilder sqlSb = new StringBuilder("update ");
            bool isArticleRef = IsArticleRef(articleId);
            if (isArticleRef)
            {
                sqlSb.Append(" article_ref");
            }
            else
            {
                sqlSb.Append(" article");
            }
            sqlSb.Append(" set click=click+1,last_visited_time=getdate() where id=@articleId;");



            IDbParameters dbParameters = CreateDbParameters();



            dbParameters.AddWithValue("articleId", articleId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlSb.ToString(), dbParameters);


        }
        public bool IsArticleRef(string articleId)
        {
            bool isRef = false;


            if (string.IsNullOrEmpty(articleId))
                return false;


            string sql = "select count(1) from article_ref where id=@articleId";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);


            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isRef = true;

            }

            return isRef;



        }
        public bool ExistsId(string articleId)
        {
            bool isExits = false;

            string sql = "select count(1) from article where id=@articleId";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;
        }
        public bool ExistsRefId(string articleRefId)
        {
            bool isExits = false;

            string sql = "select count(1) from article_ref where id=@articleRefId";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleRefId", articleRefId);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;
        }
        public bool ExistsArticlePicture(string articlePictureId)
        {
            bool isExits = false;

            string sql = "select count(1) from article_picture where id=@articlePictureId";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articlePictureId", articlePictureId);


            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;
        }
        public bool ExistsArticleSite(string articleId, string siteId)
        {
            bool isExits = false;

            string sql = "select count(1) from site_article where article_id=@articleId and site_id=@siteId";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleId", articleId);
            dbParameters.AddWithValue("siteId", siteId);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;
        }
        public bool ExistsArticleRefSite(string articleRefId, string siteId)
        {
            bool isExits = false;

            string sql = "select count(1) from site_article_ref where article_id=@articleRefId and site_id=@siteId";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleRefId", articleRefId);
            dbParameters.AddWithValue("siteId", siteId);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;
        }

        public bool ExistsTitle(string articleTitle, bool isEnabled)
        {
            bool isExits = false;
            int enabled = 0;

            if (isEnabled)
            {
                enabled = 1;
            }
            if (articleTitle == null)
                articleTitle = "";




            string sql = "select count(1) from article where enabled=@enabled and title=@articleTitle";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleTitle", articleTitle);
            dbParameters.AddWithValue("enabled", enabled);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }


            return isExits;

        }

        public List<string> GetAllAuthorsList()
        {
            List<string> authorsList = new List<string>();
            string sql = "select distinct author from view_article";

            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                authorsList.Add(row[0].ToString());
            }

            return authorsList;
        }

        public List<ArticleBase> GetAllArticlesIndex()
        {

            string sql = "select id,author,title,news_from,addtime,url,content,reply from view_article_guestbook where site_id=@site_id and is_static=1";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);


            List<ArticleBase> articleIndexList = new List<ArticleBase>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ArticleBase articleIndex = new ArticleBase();

                articleIndex.id = row["id"].ToString();

                articleIndex.title = row["title"].ToString();

                articleIndex.author = row["author"].ToString();
                articleIndex.news_from = row["news_from"].ToString();

                articleIndex.content = row["content"].ToString() + row["reply"].ToString();
                articleIndex.addtime = (DateTime)row["addtime"];

                articleIndexList.Add(articleIndex);

            }


            return articleIndexList;


        }
        public Article GetArticleByTitle(string articleTitle, bool isWithContent)
        {
            if (string.IsNullOrEmpty(articleTitle))
                return new Article();


            Article article = new Article();
            string sql;

            if (isWithContent)
            {
                sql = "select * from view_article where ref_id is null and title=@articleTitle and site_id=@site_id";
            }
            else
            {
                sql = "select id,author,title,keywords,class_id,big_class_id,module_class_id,is_all_class,news_from,addtime,is_static,sort_seq,ref_id,last_mod,enabled,click,pic_id from view_article where ref_id is null and title=@articleTitle and site_id=@site_id";
            }
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("articleTitle", articleTitle);
            dbParameters.AddWithValue("site_id", Total.SiteId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);


            if (ds.Tables[0].Rows.Count > 0)
            {

                article = CloneFromDataRow(ds.Tables[0].Rows[0], isWithContent);
            }

            if (article == null)
                article = new Article();

            return article;

        }
        private Article CloneFromDataRow(DataRow row, bool isWithContent)
        {
            Article article = new Article();
            article.id = row["id"].ToString();
            article.class_id = row["class_id"].ToString();
            article.title = row["title"].ToString();
            if (isWithContent)
            {
                article.content = row["content"].ToString();
            }
            article.author = row["author"].ToString();
            article.news_from = row["news_from"].ToString();
            article.big_class_id = row["big_class_id"].ToString();
            article.module_class_id = row["module_class_id"].ToString();
            article.is_all_class = Convert.ToBoolean(row["is_all_class"]);
            article.addtime = (DateTime)row["addtime"];
            article.last_mod = (DateTime)row["last_mod"];
            article.keywords = row["keywords"].ToString();
            article.is_static = Convert.ToBoolean(row["is_static"]);
            article.sort_seq = Convert.ToInt32(row["sort_seq"]);
            article.click = Convert.ToInt32(row["click"]);
            article.ref_id = row["ref_id"].ToString();
            article.enabled = Convert.ToBoolean(row["enabled"]);

            if (row["pic_id"] != DBNull.Value && row["pic_id"] != null)
            {
                ArticlePicture picture = new ArticlePicture();
                picture.id = row["pic_id"].ToString();
                picture.pic_url = row["pic_url"].ToString();
                picture.pic_alt = row["pic_alt"].ToString();

                article.articlePicture = picture;
            }




            return article;
        }
        private Article CloneFromReader(SqlDataReader reader, bool isWithContent)
        {
            Article article = new Article();
            article.id = reader["id"].ToString();
            article.class_id = reader["class_id"].ToString();
            article.title = reader["title"].ToString();
            if (isWithContent)
            {
                article.content = reader["content"].ToString();
            }
            article.author = reader["author"].ToString();
            article.news_from = reader["news_from"].ToString();
            article.big_class_id = reader["big_class_id"].ToString();
            article.module_class_id = reader["module_class_id"].ToString();
            article.is_all_class = Convert.ToBoolean(reader["is_all_class"]);
            article.addtime = (DateTime)reader["addtime"];
            article.last_mod = (DateTime)reader["last_mod"];
            article.keywords = reader["keywords"].ToString();
            article.is_static = Convert.ToBoolean(reader["is_static"]);
            article.sort_seq = Convert.ToInt32(reader["sort_seq"]);
            article.click = Convert.ToInt32(reader["click"]);
            article.ref_id = reader["ref_id"].ToString();
            article.enabled = Convert.ToBoolean(reader["enabled"]);




            return article;
        }


        public SearchResult SearchArticle(string key, string searchType)
        {

            string likeSymbol = "%";
            StringBuilder keyParamSb = new StringBuilder();
            keyParamSb.Append(key.Trim());
            keyParamSb.Replace(",", " ");
            keyParamSb.Replace("，", " ");
            keyParamSb.Replace(";", " ");
            keyParamSb.Replace("；", " ");
            while (keyParamSb.ToString().IndexOf("  ") >= 0)
            {
                keyParamSb.Replace("  ", " ");
            }
            string[] keyParamArray = keyParamSb.ToString().Split(' ');



            string selectedCols = " id,title,news_from,addtime,url ";
            StringBuilder sqlSb = new StringBuilder("select ");
            sqlSb.Append(selectedCols);
            sqlSb.Append(" from view_article_guestbook where is_static=1 and site_id=@site_id");


            int i = 0;
            for (i = 0; i < keyParamArray.Length; i++)
            {
                if (searchType == SearchType.TITLE || searchType == SearchType.AUTHOR)
                {
                    sqlSb.Append(" and (");
                    sqlSb.Append(searchType);
                    sqlSb.Append(" like @keywords");
                    sqlSb.Append(i);
                    sqlSb.Append(") ");

                }
                else
                {
                    sqlSb.Append(" and (title like @keywords");
                    sqlSb.Append(i);
                    sqlSb.Append(" or author like @keywords");
                    sqlSb.Append(i);
                    sqlSb.Append(") ");
                }


            }


            sqlSb.Append(" order by addtime desc");
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);

            i = 0;
            foreach (string keyParam in keyParamArray)
            {
                dbParameters.AddWithValue("keywords" + i, likeSymbol + keyParam.Trim() + likeSymbol);
                i++;
            }



            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sqlSb.ToString(), dbParameters);

            SearchResult sr = new SearchResult(ds);




            return sr;


        }




        public int CountArticle(string moduleClassId, string bigClassId, string smallClassId)
        {
            int count = 0;



            StringBuilder sqlSb = new StringBuilder("select count(1) from view_article where enabled=1  and site_id=@site_id");
            if (!string.IsNullOrEmpty(moduleClassId))
                sqlSb.Append(" and module_class_id = @moduleClassId ");

            if (!string.IsNullOrEmpty(bigClassId) || !string.IsNullOrEmpty(smallClassId))
            {
                sqlSb.Append(" and (is_all_class=1 or (is_all_class=0 ");

                if (!string.IsNullOrEmpty(bigClassId))
                    sqlSb.Append(" and big_class_id = @bigClassId ");

                if (!string.IsNullOrEmpty(smallClassId))
                    sqlSb.Append(" and class_id = @smallClassId ");

                sqlSb.Append("))");
            }
            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("site_id", Total.SiteId);

            if (!string.IsNullOrEmpty(moduleClassId))
                dbParameters.AddWithValue("moduleClassId", moduleClassId);

            if (!string.IsNullOrEmpty(bigClassId))
                dbParameters.AddWithValue("bigClassId", bigClassId);

            if (!string.IsNullOrEmpty(smallClassId))
                dbParameters.AddWithValue("smallClassId", smallClassId);



            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sqlSb.ToString(), dbParameters);

            if (ds.Tables[0].Rows.Count > 0)
            {

                count = (int)ds.Tables[0].Rows[0][0];
            }



            return count;
        }




        public int UpdateAllStatic(bool isStatic, DateTime timestamp)
        {
            string sql = @"update article set is_static=@isStatic,last_mod=@timestamp  where is_static <> @isStatic and addtime<=@timestamp;
                            update article_ref set is_static=@isStatic  where is_static <> @isStatic and addtime<=@timestamp;";

            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("isStatic", Convert.ToInt32(isStatic));
            dbParameters.AddWithValue("timestamp", timestamp);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);


        }
        public Dictionary<string, List<string>> GetArticleSiteDict()
        {
            string sql = "select article_id,site_id from site_article order by article_id";


            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sql);
            Dictionary<string, List<string>> siteArticleDict = new Dictionary<string, List<string>>();
            List<string> siteIdList = new List<string>();
            string oldArticleId = "";
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (oldArticleId != row["article_id"].ToString())
                {
                    if (siteIdList.Count > 0)
                        siteArticleDict.Add(oldArticleId, siteIdList);

                    oldArticleId = row["article_id"].ToString();
                    siteIdList = new List<string>();
                    siteIdList.Add(row["site_id"].ToString());

                }
                else
                {
                    siteIdList.Add(row["site_id"].ToString());
                }


            }
            if (siteIdList.Count > 0)
                siteArticleDict.Add(oldArticleId, siteIdList);


            return siteArticleDict;

        }
        public List<string> GetUnstaticSmallClassIdList()
        {
            List<string> smallClassIdList = new List<string>();


            string sql = "select distinct class_id from view_article where is_static=0 and enabled=1 and site_id=@site_id";

            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("site_id", Total.SiteId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["class_id"] != DBNull.Value && !string.IsNullOrEmpty(row["class_id"].ToString()))
                    smallClassIdList.Add(row["class_id"].ToString());
            }

            return smallClassIdList;

        }
        public List<string> GetUnstaticModuleClassIdList()
        {

            List<string> moduleClassIdList = new List<string>();


            string
           sql = "select distinct module_class_id from view_article where is_static=0  and enabled=1 and site_id=@site_id";
            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("site_id", Total.SiteId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                moduleClassIdList.Add(row[0].ToString());
            }

            return moduleClassIdList;

        }
        public List<string> GetUnstaticBigClassIdList()
        {
            List<string> bigClassIdList = new List<string>();

            string sql = "select distinct big_class_id from view_article where is_static=0 and enabled=1 and site_id=@site_id union select distinct big_class_id from view_guestbook where is_static=0 and site_id=@site_id ";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["big_class_id"] != DBNull.Value && !string.IsNullOrEmpty(row["big_class_id"].ToString()))
                    bigClassIdList.Add(row["big_class_id"].ToString());
            }

            return bigClassIdList;

        }
        public int InsertArticleRef(Article articleRef)
        {


            string sql = " insert into article_ref (id,ref_id,class_id,big_class_id,module_class_id,sort_seq,addtime,click,enabled) values (@id,@ref_id,@class_id,@big_class_id,@module_class_id,@sort_seq,@addtime,0,1)";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("id", articleRef.id);
            dbParameters.AddWithValue("ref_id", articleRef.ref_id);

            if (string.IsNullOrEmpty(articleRef.class_id))
            {
                dbParameters.AddWithValue("class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("class_id", articleRef.class_id);
            }
            if (string.IsNullOrEmpty(articleRef.big_class_id))
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", articleRef.big_class_id);
            }
            if (string.IsNullOrEmpty(articleRef.module_class_id))
            {
                dbParameters.AddWithValue("module_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("module_class_id", articleRef.module_class_id);
            }
            if (DateTime.MinValue.Equals(articleRef.addtime))
            {
                dbParameters.AddWithValue("addtime", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("addtime", articleRef.addtime);
            }
            dbParameters.AddWithValue("sort_seq", articleRef.sort_seq);


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

        public int InsertArticlePicture(ArticlePicture articlePicture)
        {
            string sql = " insert into article_picture (id,pic_url,pic_alt) values (@id,@pic_url,@pic_alt)";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("id", articlePicture.id);
            dbParameters.AddWithValue("pic_url", articlePicture.pic_url);
            dbParameters.AddWithValue("pic_alt", articlePicture.pic_alt);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }
        public int UpdateArticlePicture(ArticlePicture articlePicture)
        {
            string sql = " update article_picture set pic_url=@pic_url,pic_alt=@pic_alt where id=@id";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("id", articlePicture.id);
            dbParameters.AddWithValue("pic_url", articlePicture.pic_url);
            dbParameters.AddWithValue("pic_alt", articlePicture.pic_alt);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }
        public int DeleteArticlePicture(string articlePictureId)
        {
            string sql = " delete from article_picture where id=@id";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("id", articlePictureId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }



    }

}