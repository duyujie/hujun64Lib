using System.Text;
namespace com.hujun64.po
{
    /// <summary>
    ///MainClass 的摘要说明
    /// </summary>
    public class DropMenuClass:SiteClass
    {
        
        private bool _visible=false;       
        private string _target ="self";

        
        public string target
        {
            get { return _target; }
            set { _target = value; }
        }

        public int boot
        {
            get;
            set;
        }
        public bool visible
        {
            get { return _visible; }
            set { _visible = value; }
        }




        public DropMenuClass()
        {
        }
        
    }
}