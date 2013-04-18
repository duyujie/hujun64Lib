using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using com.hujun64.Dao;
using com.hujun64.po;
using com.hujun64.type;
using com.hujun64.util;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
namespace com.hujun64.logic
{
    internal class ClientService : IClientService
    {
      

        private static object privateStaticObject = new Object();


        public IVisitedHistoryDao visitDao { get; set; }
        public IClientDao clientDao { get; set; }
        public IIpInfoDao ipDao { get; set; }
        public ICommonDao commonDao { get; set; }


       
        public List<Client> GetAllClients(bool? isOnlyShanghai)
        {
            return clientDao.GetAllClients(isOnlyShanghai);
        }
        [Transaction ]
        public bool InsertClient(Client client)
        {

            if (string.IsNullOrEmpty(client.id))
                client.id = GenerateId();

            int success = clientDao.InsertClient(client);

            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool UpdateClient(Client client)
        {
            int success = clientDao.UpdateClient(client);

            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool UpdateClientMobileProvince(string mobile_code, string mobile_province)
        {
            int success = clientDao.UpdateClientMobileProvince(mobile_code, mobile_province);

            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool DeleteClient(string cid)
        {
            int success = clientDao.DeleteClient(cid);

            return success > 0 ? true : false;
        }
        public int ImportClient(string csvFilename)
        {

            List<Client> clientList = UtilImport.importClient(csvFilename);
            int iSuccessed = 0;
            foreach (Client client in clientList)
            {

                iSuccessed += Convert.ToInt32(InsertClient(client));

            }

            return iSuccessed;
        }
         [Transaction(TransactionPropagation.Required)]
        public string GenerateId()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(Total.PrefixVisitId);
            sb.Append(Total.SiteId);
            ICommonService commonService = ServiceFactory.GetCommonService();
            sb.Append(commonService.GetNextSeq("visited").ToString(Total.IdFormatString));

            return sb.ToString();
        }

        [Transaction(TransactionPropagation.Required)]
        public bool InsertVisitedHistory(VisitedHistory visitedHistory)
        {
            if (visitedHistory == null)
                return false;

            if (string.IsNullOrEmpty(visitedHistory.id))
                visitedHistory.id = this.GenerateId();

            int success= visitDao.InsertVisitedHistory(visitedHistory);


            return success > 0 ? true : false;
        }



        
        public List<string> GetQueryIpFromList()
        {
            return ipDao.GetQueryIpFromList();
        }
        [Transaction(TransactionPropagation.Required)]
        public bool InsertIp(string ipFrom)
        {
            int success= ipDao.InsertIp(ipFrom);

            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool UpdateIpProvince(string ipFrom, string ipProvince)
        {
            int success = ipDao.UpdateIpProvince(ipFrom, ipProvince);

            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public int FixIpFrom()
        {
            return ipDao.FixIpFrom();
        }
        
        public ClickIpSummary SummaryClickIp(DateTime dateFrom, DateTime dateTo)
        {
            return visitDao.SummaryClickIp(dateFrom, dateTo);
        }
        
        public DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo)
        {
            return visitDao.SummaryClickSource(dateFrom, dateTo);
        }
        
        public DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo, int topRows)
        {
            return visitDao.SummaryClickSource(dateFrom, dateTo, topRows);
        }
    }
}
