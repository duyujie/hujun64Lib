using System;
using com.hujun64.type;
namespace com.hujun64.po
{
    /// <summary>
    ///VisitedHistory 的摘要说明
    /// </summary>
    public class VisitedHistory 
    {
        public string id;
        public PageType page_type;
        public string page_id;
        public string ip_from;      
        public DateTime visited_time;
        public string click_source_site;
        public string click_source_url;
        public VisitedHistory()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
       
    }
}