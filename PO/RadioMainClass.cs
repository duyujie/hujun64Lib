using System.Text;
namespace com.hujun64.po
{
    /// <summary>
    ///MainClass ��ժҪ˵��
    /// </summary>
    public class RadioMainClass:MainClass
    {
        private bool _checked=false;
      
        public bool radio_checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

      
    }
}