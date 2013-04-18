using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.po;
using com.hujun64.type;
using System.Data;
namespace com.hujun64.logic
{
    public interface IClientService
    {
        List<Client> GetAllClients(bool? isOnlyShanghai);

        bool InsertClient(Client client);
        bool UpdateClient(Client client);
        bool UpdateClientMobileProvince(string mobile_code, string mobile_province);
        bool DeleteClient(string cid);
        int ImportClient(string csvFilename);




        bool InsertVisitedHistory(VisitedHistory visitedHistory);


        ClickIpSummary SummaryClickIp(DateTime dateFrom, DateTime dateTo);
        DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo);
        DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo, int topRows);
       

        List<string> GetQueryIpFromList();
        bool InsertIp(string ipFrom);
        bool UpdateIpProvince(string ipFrom, string ipProvince);
        int FixIpFrom();
        
    }
}
