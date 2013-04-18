using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.Dao;
using com.hujun64.po;
using com.hujun64.type;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
namespace com.hujun64.logic
{
    internal class AdminService : IAdminService
    {


        public IAdminDao adminDao { get; set; }

        internal AdminService()
        {
        }
        
        public string GetPassword(string username)
        {
            return adminDao.GetPassword(username);
        }

        public bool Islogin(string username, string userpass)
        {
            string dbPass = this.GetPassword(username);
            if (!string.IsNullOrEmpty(dbPass) && dbPass == userpass)
                return true;
            else
                return false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool ChangePassword(string username, string userpass)
        {
            return adminDao.ChangePassword(username, userpass);
        }

        




    }
}
