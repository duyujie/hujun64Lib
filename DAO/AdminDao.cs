using System;
using System.Data;
using System.Data.SqlClient;
using Spring.Data.Core;
using Spring.Data.Common;
namespace com.hujun64.Dao
{
    /// <summary>
    /// login 的摘要说明。
    /// </summary>
    internal class AdminDao : AdoDaoSupport, IAdminDao
    {

        public bool ChangePassword(string username, string userpass)
        {
            string sql = "update admin set upass=@userpass where uname=@username";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("username", username);
            dbParameters.AddWithValue("userpass", userpass);

            return Convert.ToBoolean(AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters));

        }
        public string GetPassword(string username)
        {
            string password = null;
            string sql = "select upass from admin where uname=@username";

            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("username", username);

            object objResult = AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters);
            if (objResult != null && !Convert.IsDBNull(objResult))
            {
                password = Convert.ToString(objResult);
            }
            return password;


        }

       
    }
}
