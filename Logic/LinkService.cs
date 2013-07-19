using System;
using System.Collections.Generic; 
using System.Text;
using com.hujun64.Dao;
using com.hujun64.po;
using com.hujun64.type;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
namespace com.hujun64.logic
{
    internal class LinkService : ILinkService
    {
        private static Dictionary<string, Link> CachedLinkDict = null;
      

        private static object privateStaticObject = new Object();


        private ILinkDao linkDao;
        private ICommonDao commonDao;


        public LinkService(ILinkDao linkDao, ICommonDao commonDao)
        {
            this.linkDao = linkDao;
            this.commonDao = commonDao;
            Init();
        }
        private void Init()
        {
            if (CachedLinkDict == null)
            {
                CacheLink();
            }
        }
        public void RefreshCachedLink()
        {
            // Synchronize access to the shared member.
            lock (privateStaticObject)
            {              
                CacheLink();

            }

        }
        private void UpdateCachedLink(List<Link> linkList)
        {
            foreach (Link link in linkList)
            {
                UpdateCachedLink(link);
            }
        }
        private void UpdateCachedLink(Link link)
        {
            if (string.IsNullOrEmpty(link.link_id))
            {
                //被purge
                RemoveCachedLink(link.link_id);
            }
            else if (!CachedLinkDict.ContainsKey(link.link_id))
            {
                //新增
                NewCachedLink(link);

            }
            else
            {
                //更新
                CachedLinkDict[link.link_id] = link;
            }

        }
        [Transaction(TransactionPropagation.Required)]
        private void UpdateCachedLink(string linkId)
        {
            Link link = linkDao.GetLink(linkId);

            UpdateCachedLink(link);

        }

        private void RemoveCachedLink(string linkId)
        {
            if (string.IsNullOrEmpty(linkId))
                return;


            
            if (CachedLinkDict.ContainsKey(linkId))
            {
                CachedLinkDict.Remove(linkId);
            }


        }
        private void CacheLink()
        {

            List<Link> linkList = linkDao.GetAllLink();

            if (CachedLinkDict == null)
                CachedLinkDict = new Dictionary<string, Link>(linkList.Count);
            else
                CachedLinkDict.Clear();
           


            foreach (Link link in linkList)
            {
                NewCachedLink(link);
            }


        }


        private void NewCachedLink(Link link)
        {
            if (link == null || string.IsNullOrEmpty(link.link_id))
                return;

            CachedLinkDict.Add(link.link_id, link);
           
        }

        private void CacheClassLinkKey(string classId, string linkId)
        {
            if (string.IsNullOrEmpty(classId) || string.IsNullOrEmpty(linkId))
                return;


        }
        public Link GetLink(string linkId)
        {
            if (string.IsNullOrEmpty(linkId))
                return new Link();



            if (!string.IsNullOrEmpty(linkId) && CachedLinkDict.ContainsKey(linkId))
                return CachedLinkDict[linkId];
            else
            {
                Link link = linkDao.GetLink(linkId);
                if (link != null && !string.IsNullOrEmpty(link.link_id))
                {
                    this.NewCachedLink(link);
                }
                return link;
            }

        }
        [Transaction(TransactionPropagation.Required)]
        public string GenerateId()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("L");
            sb.Append(Total.SiteId);
            ICommonService commonService = ServiceFactory.GetCommonService();
            sb.Append(commonService.GetNextSeq("link").ToString(Total.IdFormatString));
        
            return sb.ToString();
        }
        [Transaction(TransactionPropagation.Required)]
        public bool InsertLink(Link link)
        {
            
            if (link == null)
                return false;

            if (linkDao.ExistsLink(link.link_site_url))
                throw new Exception("已经有相同网站名称或地址在申请中，请更换后重试！");

            if (string.IsNullOrEmpty(link.link_id))
                link.link_id = this.GenerateId();


            if (link.sort_seq <= 0)
            {
                ICommonService commonService = ServiceFactory.GetCommonService();
                link.sort_seq = commonService.GetNextSeq("link_sort");
            }



            link.addtime = DateTime.Now;


            int success = linkDao.InsertLink(link);
            if (success == 1)
                this.NewCachedLink(link);
            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool UpdateLink(Link link)
        {
            int success = linkDao.UpdateLink(link);

            if (success == 1)
                this.UpdateCachedLink(link);

            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool DeleteLink(string linkId)
        {

            int success = linkDao.DeleteLink(linkId);
            if (success == 1)
                this.RemoveCachedLink(linkId);
            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool ApproveLink(string linkId, ApproveStatus approveStatus)
        {
            Link link = this.GetLink(linkId);
            link.approve_status = approveStatus;
            link.last_mod = DateTime.Now;
            link.is_static = false;
            link.approve_time = DateTime.Now;
            return this.UpdateLink(link);
        }
        [Transaction(TransactionPropagation.Required)]
        public int UpdateAllStatic(bool isStatic, DateTime timestamp)
        {
            int success = linkDao.UpdateAllStatic(isStatic, timestamp);

            //更新cache
            foreach (string id in CachedLinkDict.Keys)
            {
                CachedLinkDict[id].is_static = isStatic;
                CachedLinkDict[id].last_mod = timestamp;
            }
            return success;
        }
        [Transaction(TransactionPropagation.Required)]
        public List<string> UpdateListStatic(ICollection<string> idList, bool isStatic, DateTime timestamp)
        {
            List<string> updatedIdList = linkDao.UpdateListStatic(idList, isStatic, timestamp);

            //更新cache
            foreach (string id in updatedIdList)
            {
                CachedLinkDict[id].is_static = isStatic;
                CachedLinkDict[id].last_mod = timestamp;
            }
            return updatedIdList;
        }
        public Dictionary<string, DateTime> GetAllLinkIdDict(bool isRefreshAll)
        {
            Dictionary<string, DateTime> linkIdTimeDic = new Dictionary<string, DateTime>();

            foreach (string id in CachedLinkDict.Keys)
            {
                if (isRefreshAll)
                    linkIdTimeDic.Add(id, CachedLinkDict[id].last_mod);
                else if (!CachedLinkDict[id].is_static)
                    linkIdTimeDic.Add(id, CachedLinkDict[id].last_mod);

            }

            return linkIdTimeDic;
        }

        public List<Link> GetTopLink(ApproveStatus approveStatus, int count)
        {
            List<Link> linkList = new List<Link>();
            foreach (Link link in CachedLinkDict.Values)
            {
                if (link.site_id==Total.SiteId && link.approve_status == approveStatus)
                {
                    linkList.Add(link);
                }
            }

            linkList.Sort(new ComparerLink());

            if (linkList.Count < count)
                count = linkList.Count;


            return linkList.GetRange(0, count);



        }

        public List<Link> GetTopLogoLink(ApproveStatus approveStatus, int count)
        {
            List<Link> linkList = new List<Link>();
            foreach (Link link in CachedLinkDict.Values)
            {
                if (link.site_id == Total.SiteId && link.approve_status == approveStatus && !string.IsNullOrEmpty(link.link_site_logo))
                {
                    linkList.Add(link);
                }
            }

            linkList.Sort(new ComparerLink());
            if (linkList.Count < count)
                count = linkList.Count;

            return linkList.GetRange(0, count);
        }
    }
}
