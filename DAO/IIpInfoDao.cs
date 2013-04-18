using System.Collections.Generic;

namespace com.hujun64.Dao
{
    /// <summary>
    ///IGuestbookDao 的摘要说明
    /// </summary>
    public interface IIpInfoDao
    {
        List<string> GetQueryIpFromList();
        int InsertIp(string ipFrom);
        int UpdateIpProvince(string ipFrom, string ipProvince);
        int FixIpFrom();

    }
}