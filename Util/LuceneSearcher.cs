using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Text;
using System.IO;

using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Store;
using FSDirectory = Lucene.Net.Store.FSDirectory;
using Lucene.Net.Util;

using IKAnalyzerNet;

using com.hujun64.po;
using com.hujun64.util;
namespace com.hujun64
{
    public class LuceneSearcher
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("LucenceSearcher");
        private static string IndexStorePath = null;

        private const string IndexArticleId = "id";
        private const string IndexArticleAuthor = "author";
        private const string IndexArticleTitle = "title";
        private const string IndexArticleFrom = "news_from";
        private const string IndexArticleContent = "content";
        private const string IndexArticleCreateDate = "addtime";

        //private static LucenceSearcher instance = new LucenceSearcher();
        private LuceneSearcher()
        {

        }
        public static void SetIndexStorePath(string indexStorePath)
        {
            IndexStorePath = indexStorePath;
        }
        private static void SetDefaultIndexStorePath()
        {
            HttpContext context=HttpContext.Current;
            if (context != null)
            {
                IndexStorePath = context.Server.MapPath(Total.IndexPath);
            }
            else
            {
                IndexStorePath = Total.PhysicalRootPath + Total.IndexPath.Replace("~", "").Replace("/", "\\");
                IndexStorePath.Replace("\\\\", "\\");
            }
            
        }
        public static void WriteIndex(ArticleBase articleIndex, bool isCreate)
        {

            List<ArticleBase> articleIndexList = new List<ArticleBase>(1);
            articleIndexList.Add(articleIndex);
            WriteIndex(articleIndexList, isCreate);
        }

      

        /********************* 
         * For version 2.9.2
         * *******************/
        //建立索引
        public static void WriteIndex(List<ArticleBase> articleIndexList, bool isCreate)
        {
            if (IndexStorePath == null)
            {
                SetDefaultIndexStorePath();
            }

            System.IO.DirectoryInfo INDEX_DIR = new System.IO.DirectoryInfo(IndexStorePath);

            IndexWriter writer = null;

            try
            {


                writer = new IndexWriter(FSDirectory.Open(INDEX_DIR), new IKAnalyzer(), isCreate, IndexWriter.MaxFieldLength.UNLIMITED);

                StringBuilder contentSb = new StringBuilder();
                //建立索引字段
                foreach (ArticleBase artcile in articleIndexList)
                {
                    Document doc = new Document();

                    doc.Add(new Field(IndexArticleId, artcile.id, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储，不索引
                    doc.Add(new Field(IndexArticleAuthor, artcile.author, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field(IndexArticleTitle, artcile.title, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field(IndexArticleFrom, artcile.news_from, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    contentSb.Length = 0;
                    contentSb.Append(artcile.author); contentSb.Append(" ");
                    contentSb.Append(artcile.title); contentSb.Append(" ");
                    contentSb.Append(artcile.content);
                    doc.Add(new Field(IndexArticleContent, contentSb.ToString(), Field.Store.NO, Field.Index.ANALYZED));//不存储，索引，indexcontent实现了title和content，也就是标题和内容的索引
                    doc.Add(new Field(IndexArticleCreateDate, artcile.addtime.ToString("yyyy-MM-dd HH:mm:ss"), Field.Store.YES, Field.Index.NOT_ANALYZED));

                    writer.AddDocument(doc);
                }



                writer.Optimize();



            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                throw e;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }
        


        /******************************
         *  For version 2.9.2
         * *****************************/
        public static SearchResult SearchArticle(string keywords)
        {
            if(string.IsNullOrEmpty(keywords))
                return null;

            if (IndexStorePath == null)
            {
                SetDefaultIndexStorePath();
            }

            System.IO.DirectoryInfo INDEX_DIR = new System.IO.DirectoryInfo(IndexStorePath);

            IndexSearcher searcher = null;
            IndexReader reader = null;
            try
            {

                reader = IndexReader.Open(FSDirectory.Open(INDEX_DIR), true);
                searcher = new IndexSearcher(reader);


                string[] keywordsArray = GetKeyWordsSplitBySpace(keywords.Trim(), new IKAnalyzer()).Split(' ');

                QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, IndexArticleContent, new IKAnalyzer());
               
             

                

                BooleanQuery booleanQuery = new BooleanQuery();
                foreach (string key in keywordsArray)
                {
                    if (string.IsNullOrEmpty(key))
                        continue;

                    Query query = queryParser.Parse(key);
                    booleanQuery.Add(query, BooleanClause.Occur.MUST);
                  
                }



             TopScoreDocCollector collector = TopScoreDocCollector.create(searcher.MaxDoc(), false);
             searcher.Search(booleanQuery, collector);

             ScoreDoc[] hits = collector.TopDocs().scoreDocs;
 
            if(collector.GetTotalHits()>0){
              DataTable table = new DataTable("article");
                    table.Columns.Add(IndexArticleId, typeof(string));
                    table.Columns.Add(IndexArticleAuthor, typeof(string));
                    table.Columns.Add(IndexArticleTitle, typeof(string));
                    table.Columns.Add(IndexArticleFrom, typeof(string));
                    table.Columns.Add(IndexArticleCreateDate, typeof(DateTime));
 
             for (Int32 i = 0; i < collector.GetTotalHits(); i++)

             {



                 Document doc = searcher.Doc(hits[i].doc);

                        DataRow row = table.NewRow();

                        row[IndexArticleId] = doc.Get(IndexArticleId);
                        row[IndexArticleAuthor] = doc.Get(IndexArticleAuthor);
                        row[IndexArticleTitle] = doc.Get(IndexArticleTitle);
                        row[IndexArticleFrom] = doc.Get(IndexArticleFrom);
                        row[IndexArticleCreateDate] = DateTime.Parse(doc.Get(IndexArticleCreateDate));

                        table.Rows.Add(row);




                    }
                    DataSet ds = new DataSet();

                    ds.Tables.Add(table);

                    SearchResult sr = new SearchResult(ds, collector.GetTotalHits());
                    return sr;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (searcher != null)
                    searcher.Close();


            }

        }

        static public string GetKeyWordsSplitBySpace(string keywords,Analyzer analyzer)
        {
            StringBuilder sb = new StringBuilder();
           System.IO.TextReader reader=null;
           try
           {
               reader = new System.IO.StringReader(keywords);
               TokenStream ts = analyzer.TokenStream("key", reader);

               for (Lucene.Net.Analysis.Token t = ts.Next(); t != null; t = ts.Next())
               {
                   sb.Append(t.TermText()); sb.Append(" ");
                 
               }



               return sb.ToString();


           }
           finally
           {
               if (reader != null)
                   reader.Close();
           }


        }
       
    }
}