using System;
using com.hujun64.type;
using System.Collections.Generic;
namespace com.hujun64.po
{
    /// <summary>
    ///Article 的摘要说明
    /// </summary>
    public class Article : ArticleBase
    {
        private string _class_id="";
        private string _big_class_id = "";
        private string _module_class_id = "";
        private string _ref_id="";
        private string _ref_by_list="";
        private bool _is_all_class=false;        
        private int _sort_seq;
        private List<string> _site_list = null;
        



        public string class_id {
            get { return _class_id; }
            set { _class_id=value; }
        }
        public string big_class_id
        {
            get { return _big_class_id; }
            set { _big_class_id = value; }
        }
        public string module_class_id
        {
            get { return _module_class_id; }
            set { _module_class_id = value; }
        }


       

        public bool is_all_class
        {
            get { return _is_all_class; }
            set { _is_all_class = value; }
        }
        public string ref_id
        {
            get { return _ref_id; }
            set { _ref_id = value; }
        }
        public string ref_by_list
        {
            get { return _ref_by_list; }
            set { _ref_by_list = value; }
        }
        
        public int sort_seq
        {
            get { return _sort_seq; }
            set { _sort_seq = value; }
        }
       
        public Article()
        {
            this.news_from = "本站原创";
           
        }
        public List<string> site_list
        {
            get
            {
                if (_site_list == null)
                    return new List<string>();
                else
                    return _site_list;
            }
            set { _site_list = value; }
        }
        public PageType page_type{
            get{
                if (enabled == false)
                    return PageType.DELETED_ARTICLE;
                else
                if(module_class_id=="21")
                    return PageType.INTRO_TYPE;
                else
                if(module_class_id=="38")
                    return PageType.FEE_TYPE;
                else
                    return PageType.ARTICLE_TYPE;

            }
           
        }
        public ArticlePicture articlePicture
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.id;
        }
    }
}
