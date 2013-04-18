using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using com.hujun64.Dao;
using com.hujun64.po;
using com.hujun64.util;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
namespace com.hujun64.logic
{
    internal class CommonService : ICommonService
    {
        public ICommonDao commonDao { get; set; }

        [Transaction]
        public DataSet GetDataSet(string sql)
        {
            return commonDao.GetDataSet(sql);
        }
        [Transaction(TransactionPropagation.Required)]
        public DataView GetDataView(string sql)
        {
            return commonDao.GetDataView(sql);
        }
        [Transaction(TransactionPropagation.Required)]
        public int ExecuteNonQuery(string sql)
        {
            return commonDao.ExecuteNonQuery(sql);
        }
        [Transaction(TransactionPropagation.Required)]
        public object ExecuteScalar(string sql)
        {
            return commonDao.ExecuteScalar(sql);
        }


        public DateTime GetSysdate()
        {
            return commonDao.GetSysdate();
        }
        [Transaction(TransactionPropagation.Required)]
        public int GetNextSeq(string className)
        {
            int seq = commonDao.GetNextSeq(className);
            if (seq == 0)
            {
                commonDao.NewSeq(className);
                seq++;
            }

            return seq;
        }

        public List<Site> GetSiteList()
        {
            return commonDao.GetSiteList();
        }


        public List<GlobalSeq> GetAllGlobalSeq()
        {
            return commonDao.GetAllGlobalSeq();
        }

        public string GetGlobalConfigValue(string configName)
        {
            return commonDao.GetGlobalConfig(configName, Total.SiteId).config_value;
        }
        [Transaction(TransactionPropagation.Required)]
        public void SetGlobalConfigValue(string configName, string configValue)
        {
            GlobalConfig config = commonDao.GetGlobalConfig(configName, Total.SiteId);
            if (string.IsNullOrEmpty(config.config_value))
            {
                config.site_id = Total.SiteId;
                config.config_name = configName;
                config.config_value = configValue;
                commonDao.InsertGlobalConfig(config);
            }
            else
            {
                config.config_value = configValue;
                commonDao.UpdateGlobalConfig(config);
            }
        }
        public string GetSiteIdByName(string siteName)
        {
            return commonDao.GetSiteIdByName(siteName);
        }
        [Transaction(TransactionPropagation.Required)]
        public int PurgeLog(int dayExpires)
        {
            return commonDao.PurgeLog(dayExpires);
        }
        [Transaction(TransactionPropagation.Required)]
        public int ImportIpLib(string ipFilename)
        {
            return commonDao.ImportIpLib(ipFilename);
        }
        [Transaction(TransactionPropagation.Required)]
        public int MaintainGlobalConfig(GlobalConfig globalConfig)
        {
            return commonDao.MaintainGlobalConfig(globalConfig);
        }
        [Transaction(TransactionPropagation.Required)]
        public int UpdateGlobalSeq(List<GlobalSeq> globalSeqList)
        {
            return commonDao.UpdateGlobalSeq(globalSeqList);
        }


        public DataSet GetPagerDataSet(string sql, int pageIndex, int pageSize, out int totalCount)
        {
            return commonDao.GetPagerDataSet(sql, pageIndex, pageSize, out totalCount);
        }
        public bool CheckSiteStatus(string siteUrl)
        {
            bool siteStatus = false;
            const int SLEEP_SECONEDS = 10, MAX_LOOP_TIMES = 3;
            int loopTimes = 0;

            while (loopTimes < MAX_LOOP_TIMES)
            {
                try
                {
                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
                    HttpWebResponse webResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    if (webResponse.StatusCode == HttpStatusCode.OK)
                        siteStatus = true;
                    else
                        siteStatus = false;
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("UtilBackup").Error("监控网站状态异常", ex);
                }
                if (siteStatus)
                    break;
                else
                {
                    loopTimes++;

                    System.Threading.Thread.Sleep(SLEEP_SECONEDS * 1000);
                }
            }


            bool isNormally = CheckSiteStatus(Total.MonitorUrl, siteStatus);

            if (!isNormally)
            {
                string subject = Total.MonitorUrl;
                if (!siteStatus)
                    subject += "无法访问！";
                else
                    subject += "恢复访问！";

                UtilMail.SendMailAsync(subject, subject + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "检测自" + Total.SiteUrl, Total.AdminMail, null);
            }

            return siteStatus;
        }

        /// <summary>
        /// 检查网站状态
        /// </summary>
        /// <param name="site_url">网站地址</param>
        /// <param name="currentStatus">状态</param>
        /// <returns>返回false表示状态已经改变，true表示状态未改变</returns>
        [Transaction(TransactionPropagation.Required)]
        public bool CheckSiteStatus(string siteUrl, bool currentStatus)
        {
            bool? oldStatus = commonDao.GetSiteStatus(siteUrl);

            if (oldStatus == null)
            {
                commonDao.InsertSiteStatus(siteUrl, currentStatus);
                return true;

            }
            else if (oldStatus == currentStatus)
            {
                return true;
            }
            else
            {
                commonDao.UpdateSiteStatus(siteUrl, currentStatus);
                return false;
            }

        }
    }
}
