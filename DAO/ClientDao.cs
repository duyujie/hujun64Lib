using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using com.hujun64.po;
using com.hujun64.util;
using com.hujun64.type;
using Spring.Data.Core;
using Spring.Data.Common;
namespace com.hujun64.Dao
{
    /// <summary>
    ///ClientDao 的摘要说明
    /// </summary>
    public class ClientDao : AdoDaoSupport, IClientDao
    {


        //参数为null时表示只取地域为空的客户记录
        public List<Client> GetAllClients(bool? isOnlyShanghai)
        {
            StringBuilder sqlSb = new StringBuilder("select * from client where status<>");
            sqlSb.Append(Convert.ToInt32(ClientStatus.DELETED));
            if (isOnlyShanghai == null)
            {
                sqlSb.Append(" and mobile_province is null or mobile_province =''");
            }
            else if (isOnlyShanghai == true)
            {
                sqlSb.Append(" and mobile_province like '上海%'");
            }



            List<Client> clientList = new List<Client>();
            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sqlSb.ToString());
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                clientList.Add(CloneFromDataRow(row));
            }
            return clientList;


        }
        private Client CloneFromReader(SqlDataReader reader)
        {
            Client client = new Client();
            client.id = reader["id"].ToString();
            client.client_name = reader["client_name"].ToString();
            client.mobile_code = reader["mobile_code"].ToString();
            client.mobile_province = reader["mobile_province"].ToString();
            client.status = (ClientStatus)reader["status"];
            client.addtime = (DateTime)reader["addtime"];


            return client;
        }
        private Client CloneFromDataRow(DataRow row)
        {
            Client client = new Client();
            client.id = row["id"].ToString();
            client.client_name = row["client_name"].ToString();
            client.mobile_code = row["mobile_code"].ToString();
            client.mobile_province = row["mobile_province"].ToString();
            client.status = (ClientStatus)row["status"];
            client.addtime = (DateTime)row["addtime"];


            return client;
        }
        public int InsertClient(Client client)
        {



            string sql = "insert into client (id,client_name,mobile_code,status,addtime) select @id,@client_name,@mobile_code,@status,getdate() where not exists (select 1 from client where mobile_code=@mobile_code)";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("id", client.id);
            dbParameters.AddWithValue("client_name", client.client_name);
            dbParameters.AddWithValue("mobile_code", client.mobile_code);
            dbParameters.AddWithValue("status", client.status);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

        public int UpdateClient(Client client)
        {
            string sql = "update client set client_name=@client_name,mobile_code=@mobile_code,mobile_province=@mobile_province,status=@status where id=@id";
            IDbParameters dbParameters = CreateDbParameters();



            dbParameters.AddWithValue("id", client.id);
            dbParameters.AddWithValue("client_name", client.client_name);
            dbParameters.AddWithValue("mobile_code", client.mobile_code);
            dbParameters.AddWithValue("mobile_province", client.mobile_province);
            dbParameters.AddWithValue("status", client.status);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);


        }
        public int UpdateClientMobileProvince(string mobile_code, string mobile_province)
        {
            string sql = "update client set mobile_province=@mobile_province where mobile_code=@mobile_code";
            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("mobile_code", mobile_code);
            dbParameters.AddWithValue("mobile_province", mobile_province);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int DeleteClient(string cid)
        {
            string sql = "update client set status=@status where id=@id";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("id", cid);
            dbParameters.AddWithValue("status", ClientStatus.DELETED);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

    }
}