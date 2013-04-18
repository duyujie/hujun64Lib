using System;
namespace com.hujun64.po
{
    /// <summary>
    ///ArticlePicture 的摘要说明
    /// </summary>
    public class ArticlePicture
    {
        private string _pic_url;
        public string id
        {
            get;
            set;
        }
        public string pic_url
        {
            get { return _pic_url == null ? "" : _pic_url; }
            set { _pic_url = value; }
        }

        public string pic_alt
        {
            get;
            set;
        }



    }
}
