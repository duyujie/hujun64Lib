using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using com.hujun64.po;
 
namespace com.hujun64.Dao
{
    /// <summary>
    ///IClientDao 的摘要说明
    /// </summary>
    public interface IClientDao
    {
        List<Client> GetAllClients(bool? isOnlyShanghai);

        int InsertClient(Client client);
        int UpdateClient(Client client);
        int UpdateClientMobileProvince(string mobile_code, string mobile_province);
        int DeleteClient(string cid);
           

    }
}