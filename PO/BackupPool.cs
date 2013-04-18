using System;
using com.hujun64.type;
using System.Collections.Generic;
namespace com.hujun64.po
{
    /// <summary>
    ///BackupPool 的摘要说明
    /// </summary>
    public class BackupPool
    {
        private string _id = "";
        private string _todo = "";
        private string _type = "";
        private string _site_id = "";



        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string todo
        {
            get { return _todo; }
            set { _todo = value; }
        }
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }



        public string site_id
        {
            get { return _site_id; }
            set { _site_id = value; }
        }


    }
}
