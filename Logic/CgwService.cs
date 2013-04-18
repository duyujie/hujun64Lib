using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
using com.hujun64.po;
using com.hujun64.Dao;
namespace com.hujun64.logic
{
    /// <summary>
    ///CGW 的摘要说明
    /// </summary>
    public class CgwService : ICgwService
    {
        private static List<CgwWords> CachedCgwWordsList = null;

        private ICommonDao commonDao;
        private static object privateStaticObject = new Object();
        public CgwService(ICommonDao commonDao)
        {
            this.commonDao = commonDao;

            Init();

        }

        private void Init()
        {
            if (CachedCgwWordsList == null)
            {
                CacheCgwWords();
            }
        }
        private void CacheCgwWords()
        {
            CachedCgwWordsList = commonDao.GetCgwWordsList();
        }
        public List<CgwWords> GetAllWordsList()
        {
            return CachedCgwWordsList;
        }
        public Article ReplaceArticle(Article article)
        {
            if (article == null)
                return article;

            article.title = Replace(article.title);
            article.content = Replace(article.content);
            article.keywords = Replace(article.keywords);

            return article;
        }
        public Guestbook ReplaceGuestbook(Guestbook guestbook)
        {
            if (guestbook == null)
                return guestbook;

            guestbook.title = Replace(guestbook.title);
            guestbook.content = Replace(guestbook.content);
            guestbook.keywords = Replace(guestbook.keywords);

            return guestbook;
        }
        public string Replace(string content)
        {
            StringBuilder contentSb = new StringBuilder(content);
            foreach (CgwWords words in CachedCgwWordsList)
            {
                if (contentSb.ToString().Contains(words.original_words))
                {
                    contentSb.Replace(words.original_words, words.new_words);
                }
            }
            return contentSb.ToString();
        }
        public void RefreshCacheCgw()
        {
            // Synchronize access to the shared member.
            lock (privateStaticObject)
            {
                CacheCgwWords();
            }
        }
        [Transaction(TransactionPropagation.Required)]
        public string GenerateId()
        {
            int nextSeq = commonDao.GetNextSeq("CGW");

            StringBuilder sb = new StringBuilder();
            sb.Append(Total.PrefixCgwWordsId);
            sb.Append(Total.SiteId);
            sb.Append(nextSeq.ToString(Total.IdFormatString));
            return sb.ToString();

        }
         [Transaction(TransactionPropagation.Required)]
        public int InsertOrUpdateCgwWords(CgwWords words)
        {
            if (words == null)
                return 0;
            else
            {
                foreach (CgwWords cgwWords in CachedCgwWordsList)
                {
                    if (cgwWords.original_words.Equals(words.original_words))
                    {
                        return commonDao.UpdateCgwWords(words);
                    }
                }


                if (string.IsNullOrEmpty(words.id))                
                    words.id = GenerateId();

                return  commonDao.InsertCgwWords(words);
            }
        }
        
         [Transaction(TransactionPropagation.Required)]
        public int DeleteCgwWords(string wordsId)
        {
            if (!string.IsNullOrEmpty(wordsId))
                return commonDao.DeleteCgwWords(wordsId);
            else
                return 0;
        }
    }
}