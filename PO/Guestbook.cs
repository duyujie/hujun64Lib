using System;
using com.hujun64.type;

namespace com.hujun64.po
{
    /// <summary>
    ///Guestbook 的摘要说明
    /// </summary>
    public class Guestbook : ArticleBase
    {
        private string _contact = "";
        private string _email="";
        private string _reply = "";        
      
        private readonly PageType _page_type = PageType.GUESTBOOK_TYPE;


        public string contact{
            get { return _contact; }
            set { _contact=value; }
        }
        public string email{
            get { return _email; }
            set { _email=value; }
        }
        public string reply{
            get { return _reply==null?"":_reply; }
            set { _reply=value; }
        }
        public string big_class_id
        {
            get;
            set;
        }
        public string sex{
            get;
            set;
        }       
        public string ip_from{
            get;
            set;
        }
        public string province_from{
            get;
            set;
        }
        public DateTime replytime{
            get;
            set;
        }
        public PageType page_type{
            get { return _page_type; }
            
        }

        public Guestbook()
        {
            this.news_from = "在线咨询";
        }
    }
}