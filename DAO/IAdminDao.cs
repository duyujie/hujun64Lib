namespace com.hujun64.Dao
{
    /// <summary>
    ///IAdminDao 的摘要说明
    /// </summary>
    public interface IAdminDao
    {
        string GetPassword(string username);
        bool ChangePassword(string username, string userpass);

       
    }
}