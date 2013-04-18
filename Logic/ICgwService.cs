using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.po;
using com.hujun64.type;

namespace com.hujun64.logic
{
    public interface ICgwService
    {
        List<CgwWords> GetAllWordsList();
        int InsertOrUpdateCgwWords(CgwWords words);
        int DeleteCgwWords(string wordsId);

        Article ReplaceArticle(Article article);
        Guestbook ReplaceGuestbook(Guestbook guestbook);
        string Replace(string content);
        void RefreshCacheCgw();
    }
}
