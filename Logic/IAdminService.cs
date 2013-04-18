using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.po;
using com.hujun64.type;

namespace com.hujun64.logic
{
    public interface IAdminService
    {
        string GetPassword(string username);
        bool Islogin(string username, string userpass);
        bool ChangePassword(string username, string userpass);

       
    }
}
