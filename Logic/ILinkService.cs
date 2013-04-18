using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.po;
using com.hujun64.type;

namespace com.hujun64.logic
{
    public interface ILinkService
    {
      
        Link GetLink(string lid);
        List<Link> GetTopLink(ApproveStatus approveStatus, int count);
        List<Link> GetTopLogoLink(ApproveStatus approveStatus, int count);
        void RefreshCachedLink();
        bool InsertLink(Link link);
        bool UpdateLink(Link link);
        bool DeleteLink(string linkId);
        bool ApproveLink(string linkId, ApproveStatus approveStatus);
        int UpdateAllStatic(bool isStatic, DateTime timestamp);
        Dictionary<string, DateTime> GetAllLinkIdDict(bool isRefreshAll);
    }
}
