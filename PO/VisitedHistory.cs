using System;
using com.hujun64.type;
namespace com.hujun64.po
{
    /// <summary>
    ///VisitedHistory ��ժҪ˵��
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
            //TODO: �ڴ˴���ӹ��캯���߼�
            //
        }
       
    }
}