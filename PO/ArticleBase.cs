using System;
using com.hujun64.type;

namespace com.hujun64.po
{
    /// <summary>
    ///Article 的摘要说明
    /// </summary>
    public class ArticleBase
    {
        
        private bool _enabled = true;



        public string id
        {
            get;
            set;
        }
        public string title
        {
            get;
            set;
        }
        public string author
        {
            get;
            set;
        }
        public string content
        {
            get;
            set;
        }
        public string news_from
        {
            get;
            set;
        }
        public string keywords
        {
           get;
            set;
        }
        public bool enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }
        public bool is_static{
            get;
            set;
        }
        public DateTime addtime{
            get;
            set;
        }
        public DateTime last_mod{
            get;
            set;
        }
        public DateTime last_visited_time{
            get;
            set;
        }

        public int click
        {
            get;
            set;
        }

        public ArticleBase()
        {
           
        }
        
    }
}
