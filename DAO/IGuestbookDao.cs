using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using com.hujun64.po;

namespace com.hujun64.Dao
{
    /// <summary>
    ///IGuestbookDao 的摘要说明
    /// </summary>
    public interface IGuestbookDao
    {
        List<Guestbook> GetAllGuestbook(bool isWithContent);
        Guestbook GetGuestbook(string guestbookId, bool isWithContent);
        string GetGuestbookTitle(string guestbookId);
        int CountGuestbook(string bigClassId);
        Dictionary<string, DateTime> GetAllGuestbookIdDict(bool isRefreshAll);
        int GetMaxGuestbookIdSeq();

        bool ExistsId(string guestbookId);
        bool ExistsGuestbookSite(string guestbookId,string siteId);
        bool ExistsTitle(string title);
        Dictionary<string, string> GetAllGuestEmailDict(bool isOnlyShanghai);


        int ReplyGuestbook(Guestbook guestbook);
        int InsertGuestbookSite(string guestbookId, string siteId);
        void IncreaseClick(string guestbookId);
        int InsertGuestbook(Guestbook guestbook);
        int UpdateGuestbook(Guestbook guestbook);
        int DeleteGuestbook(string guestbookId);
        int DeleteGuestbookSite(string guestbookId, string siteId);
        List<string> UpdateListStatic(ICollection<string> idList, bool isStatic, DateTime timestamp);
        int UpdateAllStatic(bool isStatic, DateTime timestamp);



    }
}