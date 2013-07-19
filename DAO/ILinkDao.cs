using System;
using System.Collections.Generic;
using com.hujun64.po;
using com.hujun64.type;
namespace com.hujun64.Dao
{
    /// <summary>
    ///ILinkDao 的摘要说明
    /// </summary>
    public interface ILinkDao 
    {
        
        Link GetLink(string linkId);
        List<Link> GetAllLink();

        bool ExistsLink(string siteUrl);
        int InsertLink(Link link);
        int UpdateLink(Link link);
        int DeleteLink(string lid);
       
        int UpdateAllStatic(bool isStatic, DateTime timestamp);
        List<string> UpdateListStatic(ICollection<string> linkIdList,bool isStatic, DateTime timestamp);
    }
}