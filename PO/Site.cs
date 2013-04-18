using System;
using System.Text;
using com.hujun64.type;
using com.hujun64.util;
namespace com.hujun64.po
{
    /// <summary>
    ///Site 的摘要说明
    /// </summary>
    public class Site 
    {
        private string _site_id;
        private string _site_name;
        private string _site_url;
       


        public string site_id
        {
            get { return _site_id; }
            set { _site_id = value; }
        }
        public string site_name
        {
            get { return _site_name; }
            set { _site_name = value; }
        }
        public string site_url
        {
            get { return _site_url; }
            set { _site_url = value; }
        }

        public Site()
        {
          
        }
        
    }
}