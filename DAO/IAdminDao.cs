namespace com.hujun64.Dao
{
    /// <summary>
    ///IAdminDao ��ժҪ˵��
    /// </summary>
    public interface IAdminDao
    {
        string GetPassword(string username);
        bool ChangePassword(string username, string userpass);

       
    }
}