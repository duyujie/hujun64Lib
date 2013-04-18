using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.po;
namespace com.hujun64.logic
{
    public interface IGuestbookService
    {
        bool ReplyGuestbook(Guestbook guestbook);
        void ReplyNotifyEmail(Guestbook guestbook);
        int ClickGuestbook(string guestbookId);
        Guestbook GetGuestbook(string guestbookId);
        Guestbook GetGuestbook(string guestbookId,bool isWithContent);    
        int CountGuestbook(string bigClassId);
        Dictionary<string, DateTime> GetAllGuestbookIdDict(bool isRefreshAll);
        void GetNeighborGuestbook(string guestbookId,out Guestbook prevGuestbook,out Guestbook nextGuestbook);
        
        Dictionary<string, string> GetAllGuestEmailDict(bool isOnlyShanghai);

        List<Guestbook> GetGuestbookList(string guestbookId);
        List<Guestbook> GetTopGuestbookList(string bigClassId, int count);
        List<Guestbook> GetTopGuestbookList(string bigClassId, int topBegin, int count, out int totalCount);
        void RefreshCachedGuestbook();

        string GeneratePageTitle(Guestbook guestbook);
        string GenerateId();
        bool InsertGuestbook(Guestbook guestbook);
        bool UpdateGuestbook(Guestbook guestbook);
        bool DeleteGuestbook(string gid);
        List<string> UpdateListStatic(ICollection<string> idList, bool isStatic, DateTime timestamp);
        int UpdateAllStatic(bool isStatic, DateTime timestamp);


       

    }
}
