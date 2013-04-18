using System;
using System.Text;
using com.hujun64.type;
using com.hujun64.util;
namespace com.hujun64.po
{
    /// <summary>
    ///Link 的摘要说明
    /// </summary>
    public class Link
    {



        public string link_id
        {
            get;
            set;
        }
        public string site_id
        {
            get;
            set;
        }
        public string link_site_name
        {
            get;
            set;
        }
        public string link_site_url
        {
            get;
            set;
        }
        public string link_site_logo
        {
            get;
            set;
        }
        public string link_description
        {
            get;
            set;
        }
        public string my_url
        {
            get;
            set;
        }
        public bool enabled
        {
            get;
            set;
        }
        public bool is_static
        {
            get;
            set;
        }
        public int sort_seq
        {
            get;
            set;
        }
        public ApproveStatus approve_status
        {
            get;
            set;
        }
        public DateTime addtime
        {
            get;
            set;
        }
        public DateTime last_mod
        {
            get;
            set;
        }
        public DateTime approve_time
        {
            get;
            set;
        }

        public Link()
        {

        }

    }
}