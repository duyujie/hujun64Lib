using System.Text;
using com.hujun64.type;
namespace com.hujun64.logic
{
    /// <summary>
    ///PageInfo 的摘要说明
    /// </summary>
    public class PageInfo
    {
        public string title;
        public string locationHref;
        public PageType pageType;
        public string pageId;
        public string clickSource;
        public string userIp;
        public string httpHost;

        public PageInfo()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public override string ToString()
        {
            StringBuilder sb=new StringBuilder(locationHref);
            sb.Append(" -> ");
            sb.Append(title);
            return sb.ToString();
        }
    }
}