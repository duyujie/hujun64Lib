using System;

using System.Text;

namespace com.hujun64.po
{
    public class Lawyer
    {
        public string id
        {
            get;
            set;
        }
        public string lawyer_name
        {
            get;
            set;
        }
        public string category
        {
            get;
            set;
        }
        public string article_id
        {
            get;
            set;
        }
        public int sort_seq
        {
            get;
            set;
        }
        public Article introArticle
        {
            get;
            set;
        }
    }
}
