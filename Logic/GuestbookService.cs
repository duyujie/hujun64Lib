using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.Dao;
using com.hujun64.po;
using com.hujun64.util;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
using com.hujun64.type;
namespace com.hujun64.logic
{
    internal class GuestbookService : IGuestbookService
    {
        private static readonly object CachedGuestbookDictLocker = new object();
        
        private static Dictionary<string, Guestbook> CachedGuestbookDict = null;
        private static Dictionary<string, string> CachedTitleKeyDict = null;
        private static Dictionary<string, List<string>> CachedClassGuestbookKeyDict = null;

        private static object privateStaticObject = new Object();
        private static bool IS_CACHE_CONTENT = Total.IsCacheGuestbookContent;

        private IGuestbookDao guestbookDao;
        private IBackupDao backupDao;
        private ICommonDao commonDao;


        public GuestbookService(IGuestbookDao guestbookDao, ICommonDao commonDao, IBackupDao backupDao)
        {
            this.guestbookDao = guestbookDao;
            this.commonDao = commonDao;
            this.backupDao = backupDao;
            Init();
        }
        private void Init()
        {

            if (CachedGuestbookDict == null)
            {
                if (string.IsNullOrEmpty(Total.SiteId))
                    Total.SiteId = commonDao.GetSiteIdByName(Total.SiteName);

                CacheGuestbook();
            }


        }
        public void RefreshCachedGuestbook()
        {
            // Synchronize access to the shared member.
            lock (privateStaticObject)
            {
                CacheGuestbook();

            }
        }
        private void UpdateCachedGuestbook(List<Guestbook> guestbookList)
        {
            foreach (Guestbook guestbook in guestbookList)
            {
                UpdateCachedGuestbook(guestbook);
            }
        }
        private void UpdateCachedGuestbook(Guestbook guestbook)
        {
            if (string.IsNullOrEmpty(guestbook.id))
            {
                //被purge
                RemoveCachedGuestbook(guestbook.id);
            }
            else if (!CachedGuestbookDict.ContainsKey(guestbook.id))
            {
                //新增
                NewCachedGuestbook(guestbook);

            }
            else
            {
                //更新
                CachedGuestbookDict[guestbook.id] = guestbook;
            }

        }
        [Transaction(TransactionPropagation.Required)]
        private void UpdateCachedGuestbook(string guestbookId)
        {
            Guestbook guestbook = guestbookDao.GetGuestbook(guestbookId, IS_CACHE_CONTENT);

            UpdateCachedGuestbook(guestbook);

        }
        private void RemoveCachedGuestbook(string guestbookId)
        {
            if (string.IsNullOrEmpty(guestbookId))
                return;


            lock (CachedGuestbookDictLocker)
            {


                foreach (List<string> idList in CachedClassGuestbookKeyDict.Values)
                {
                    idList.Remove(guestbookId);
                }


                if (CachedTitleKeyDict.ContainsValue(guestbookId))
                {
                    List<string> removeKeyList = new List<string>();
                    foreach (string key in CachedTitleKeyDict.Keys)
                    {
                        if (CachedTitleKeyDict[key] == guestbookId)
                            removeKeyList.Add(key);
                    }
                    foreach (string key in removeKeyList)
                    {
                        CachedTitleKeyDict.Remove(key);
                    }

                }
                if (CachedGuestbookDict.ContainsKey(guestbookId))
                {
                    CachedGuestbookDict.Remove(guestbookId);
                }
            }

        }

        private void CacheGuestbook()
        {

            List<Guestbook> guestbookList = guestbookDao.GetAllGuestbook(IS_CACHE_CONTENT);
            lock (CachedGuestbookDictLocker)
            {
                if (CachedGuestbookDict == null)
                    CachedGuestbookDict = new Dictionary<string, Guestbook>(guestbookList.Count);
                else
                    CachedGuestbookDict.Clear();

                if (CachedTitleKeyDict == null)
                    CachedTitleKeyDict = new Dictionary<string, string>(guestbookList.Count);
                else
                    CachedTitleKeyDict.Clear();

                if (CachedClassGuestbookKeyDict == null)
                    CachedClassGuestbookKeyDict = new Dictionary<string, List<string>>();
                else
                    CachedClassGuestbookKeyDict.Clear();


                foreach (Guestbook guestbook in guestbookList)
                {
                    NewCachedGuestbook(guestbook);
                }

            }

        }


        private void NewCachedGuestbook(Guestbook guestbook)
        {
            if (guestbook == null || string.IsNullOrEmpty(guestbook.id))
                return;
            lock (CachedGuestbookDictLocker)
            {
                CachedGuestbookDict.Add(guestbook.id, guestbook);

                //big class
                CacheClassGuestbookKey(guestbook.big_class_id, guestbook.id);
            }

        }

        private void CacheClassGuestbookKey(string classId, string guestbookId)
        {
            if (string.IsNullOrEmpty(classId) || string.IsNullOrEmpty(guestbookId))
                return;


            List<string> classGuestbookList;
            lock (CachedGuestbookDictLocker)
            {
                if (CachedClassGuestbookKeyDict.ContainsKey(classId))
                {
                    classGuestbookList = CachedClassGuestbookKeyDict[classId];
                }
                else
                {
                    classGuestbookList = new List<string>();
                    CachedClassGuestbookKeyDict.Add(classId, classGuestbookList);
                }

                if (!classGuestbookList.Contains(guestbookId))
                {
                    classGuestbookList.Add(guestbookId);
                }

            }

        }
        public string GeneratePageTitle(Guestbook guestbook)
        {
            if (guestbook == null)
                return "";

            int TITLE_LEN = 27;
            string TITLE_SUFFIX = "...";

            StringBuilder sb = new StringBuilder(guestbook.title);
            sb.Append("-");
            string s = UtilHtml.RemoveHtmlTag(guestbook.content, true);
            int l = Math.Abs(TITLE_LEN - guestbook.title.Length);
            if (s.Length > l)
            {
                sb.Append(s.Substring(0, l));
                sb.Append(TITLE_SUFFIX);
            }
            else
            {
                sb.Append(s);
            }

            return sb.ToString();

        }
        [Transaction(TransactionPropagation.Required)]
        public string GenerateId()
        {
            int nextSeq = guestbookDao.GetMaxGuestbookIdSeq() + 1;

            StringBuilder sb = new StringBuilder();
            sb.Append(Total.PrefixGuestbookId);
            sb.Append(Total.SiteId);
            sb.Append(nextSeq.ToString(Total.IdFormatString));
            return sb.ToString();

        }
        public Dictionary<string, string> GetAllGuestEmailDict(bool isOnlyShanghai)
        {
            Dictionary<string, string> guestEmailDict = new Dictionary<string, string>();
            lock (CachedGuestbookDictLocker)
            {
                foreach (Guestbook guestbook in CachedGuestbookDict.Values)
                {
                    if (!string.IsNullOrEmpty(guestbook.province_from) && guestbook.province_from.Contains("上海"))
                        guestEmailDict.Add(guestbook.id, guestbook.email);
                }
            }
            return guestEmailDict;
        }

        public int CountGuestbook(string bigClassId)
        {
            if (!string.IsNullOrEmpty(bigClassId) && CachedClassGuestbookKeyDict.ContainsKey(bigClassId))
                return CachedClassGuestbookKeyDict[bigClassId].Count;
            else
                return CachedGuestbookDict.Count;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool UpdateGuestbook(Guestbook guestbook)
        {
            if (Total.EnableCGW)
                guestbook = ServiceFactory.GetCgwService().ReplaceGuestbook(guestbook);


            int success = guestbookDao.UpdateGuestbook(guestbook);

            if (success == 1)
                this.UpdateCachedGuestbook(guestbook);

            return success > 0 ? true : false;
        }

        public Dictionary<string, DateTime> GetAllGuestbookIdDict(bool isRefreshAll)
        {
            Dictionary<string, DateTime> guestbookIdTimeDic = new Dictionary<string, DateTime>();

            foreach (string id in CachedGuestbookDict.Keys)
            {
                if (isRefreshAll)
                    guestbookIdTimeDic.Add(id, CachedGuestbookDict[id].last_mod);
                else if (!CachedGuestbookDict[id].is_static)
                    guestbookIdTimeDic.Add(id, CachedGuestbookDict[id].last_mod);

            }

            return guestbookIdTimeDic;
        }
        public List<Guestbook> GetTopGuestbookList(string bigClassId, int count)
        {
            int totalCount = 0;
            return GetTopGuestbookList(bigClassId, 1, count, out totalCount);
        }
        public List<Guestbook> GetTopGuestbookList(string bigClassId, int topBegin, int count, out int totalCount)
        {

            List<Guestbook> guestbookList = new List<Guestbook>();
            lock (CachedGuestbookDictLocker)
            {
                foreach (Guestbook guestbook in CachedGuestbookDict.Values)
                {
                    if (string.IsNullOrEmpty(bigClassId) || guestbook.big_class_id == bigClassId)
                    {
                        guestbookList.Add(guestbook);
                    }

                }
            }
            if (guestbookList.Count < topBegin)
            {
                totalCount = 0;
                return new List<Guestbook>(0);
            }
            else
            {
                guestbookList.Sort(new ComparerGuestbook());

                if (guestbookList.Count < (topBegin - 1 + count))
                    count = guestbookList.Count - topBegin + 1;

                totalCount = guestbookList.Count;
                return guestbookList.GetRange(topBegin - 1, count);
            }


        }

        public Guestbook GetGuestbook(string guestbookId, bool isWithContent)
        {
            if (string.IsNullOrEmpty(guestbookId))
                return new Guestbook();

            guestbookId = guestbookId.Trim();

            if (IS_CACHE_CONTENT || !isWithContent)
            {
                if (!string.IsNullOrEmpty(guestbookId) && CachedGuestbookDict.ContainsKey(guestbookId))
                    return CachedGuestbookDict[guestbookId];

            }

            Guestbook guestbook = guestbookDao.GetGuestbook(guestbookId, isWithContent);
            if (!string.IsNullOrEmpty(guestbook.id) && !CachedGuestbookDict.ContainsKey(guestbookId))
            {
                this.NewCachedGuestbook(guestbook);
            }
            return guestbook;
        }
        public Guestbook GetGuestbook(string guestbookId)
        {
            return GetGuestbook(guestbookId, IS_CACHE_CONTENT);
        }
        public void ReplyNotifyEmail(Guestbook guestbook)
        {
            //通知咨询者
            if (!string.IsNullOrEmpty(guestbook.email) && !string.IsNullOrEmpty(guestbook.reply))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<style type=\"text/css\"> p {text-indent:2em;margin-top:1em;margin-bottom:1em} </style>");
                sb.Append("尊敬的 ");
                sb.Append(guestbook.author);
                sb.Append("：<p>您好！胡律师已经回复了您于");
                sb.Append(guestbook.addtime.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("提交的咨询“");
                sb.Append(guestbook.title);
                sb.Append("”<br /><br /><p>");

                sb.Append("详情请访问： ");
                sb.Append(UtilHtml.BuildHref(Total.CurrentSiteRootUrl + UtilHtml.GetHtmlUrl(guestbook.id, PageType.GUESTBOOK_TYPE)));

                sb.Append("<br />本邮件为系统自动发送，请勿回复！");
                sb.Append("</p>");
                UtilMail.SendMailAsync("法律咨询回复 - " + guestbook.title, sb.ToString(), guestbook.email, null);


            }
        }
        [Transaction(TransactionPropagation.Required)]
        public bool ReplyGuestbook(Guestbook guestbook)
        {

            int success = guestbookDao.ReplyGuestbook(guestbook);
            if (success == 1)
            {


                this.UpdateCachedGuestbook(guestbook);
                //镜像同步
                if (!Total.IsMirrorSite)
                {
                    backupDao.InsertBackupId(guestbook.id, Total.PrefixGuestbookId, "U", Total.SiteId);
                }
            }


            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public int ClickGuestbook(string guestbookId)
        {
            guestbookDao.IncreaseClick(guestbookId);

            return GetGuestbook(guestbookId).click++;

        }
        [Transaction(TransactionPropagation.Required)]
        public bool InsertGuestbook(Guestbook guestbook)
        {

            if (guestbook == null)
                return false;


            if (Total.EnableCGW)
                guestbook = ServiceFactory.GetCgwService().ReplaceGuestbook(guestbook);

            if (string.IsNullOrEmpty(guestbook.id))
                guestbook.id = this.GenerateId();

            guestbook.addtime = DateTime.Now;


            int rowEffected = guestbookDao.InsertGuestbook(guestbook);
            if (rowEffected == 1)
            {
                ICommonService commonService = ServiceFactory.GetCommonService();
                List<Site> siteList = commonService.GetSiteList();
                foreach (Site site in siteList)
                {
                    guestbookDao.InsertGuestbookSite(guestbook.id, site.site_id);
                }
                this.NewCachedGuestbook(guestbook);
                //镜像同步
                if (!Total.IsMirrorSite)
                {
                    backupDao.InsertBackupId(guestbook.id, Total.PrefixGuestbookId, "I", Total.SiteId);
                }
            }
            bool success = rowEffected > 0 ? true : false;



            return success;


        }
        [Transaction(TransactionPropagation.Required)]
        public bool DeleteGuestbook(string guestbookId)
        {
            ICommonService commonService = ServiceFactory.GetCommonService();
            List<Site> siteList = commonService.GetSiteList();
            foreach (Site site in siteList)
            {
                guestbookDao.DeleteGuestbookSite(guestbookId, site.site_id);
            }

            int success = guestbookDao.DeleteGuestbook(guestbookId);
            if (success == 1)
            {
                this.RemoveCachedGuestbook(guestbookId);//镜像同步
                if (!Total.IsMirrorSite)
                {
                    backupDao.InsertBackupId(guestbookId, Total.PrefixGuestbookId, "I", Total.SiteId);
                }
            }
            return success > 0 ? true : false;
        }


        public List<Guestbook> GetGuestbookList(string guestbookId)
        {
            List<Guestbook> guestbookList = new List<Guestbook>(1);
            if (!string.IsNullOrEmpty(guestbookId) && CachedGuestbookDict.ContainsKey(guestbookId))
                guestbookList.Add(this.GetGuestbook(guestbookId));

            return guestbookList;

        }
        [Transaction(TransactionPropagation.Required)]
        public int UpdateAllStatic(bool isStatic, DateTime timestamp)
        {

            int success = guestbookDao.UpdateAllStatic(isStatic, timestamp);

            //更新cache
            lock (CachedGuestbookDictLocker)
            {
                foreach (string id in CachedGuestbookDict.Keys)
                {
                    CachedGuestbookDict[id].is_static = isStatic;
                    CachedGuestbookDict[id].last_mod = timestamp;
                }
            }
            return success;
        }
        [Transaction(TransactionPropagation.Required)]
        public List<string> UpdateListStatic(ICollection<string> idList, bool isStatic, DateTime timestamp)
        {

            List<string> updatedIdList = guestbookDao.UpdateListStatic(idList, isStatic, timestamp);

            //更新cache
            foreach (string id in updatedIdList)
            {
                CachedGuestbookDict[id].is_static = isStatic;
                CachedGuestbookDict[id].last_mod = timestamp;
            }
            return updatedIdList;
        }
        [Transaction(TransactionPropagation.Required)]
        public void GetNeighborGuestbook(string guestbookId, out Guestbook prevGuestbook, out Guestbook nextGuestbook)
        {
            Guestbook guestbook = GetGuestbook(guestbookId);
            List<Guestbook> neighborGuestbookList = GetTopGuestbookList(guestbook.big_class_id, Int32.MaxValue);

            prevGuestbook = null;
            nextGuestbook = null;
            for (int i = 0; i < neighborGuestbookList.Count; i++)
            {
                if (neighborGuestbookList[i].id == guestbookId)
                {
                    if (i > 0)
                        nextGuestbook = neighborGuestbookList[i - 1];


                    if (i < neighborGuestbookList.Count - 1)
                        prevGuestbook = neighborGuestbookList[i + 1];


                    return;

                }
            }
        }
    }
}
