using System;
using System.Text;
using com.hujun64.type;

namespace com.hujun64.util
{
    /// <summary>
    ///Sitemap 的摘要说明
    /// </summary>
    public class PageMeta
    {
        public string moduleName;
        public string title;
        public string keywords;
        public string description;
      

        public PageMeta()
        {
        }
         
 
        public override bool Equals(object right)
        {


            if (right == null)

                return false;

            if (object.ReferenceEquals(this, right))

                return true;


            if (this.GetType() != right.GetType())

                return false;


            return CompareFooMembers(

              this, right as PageMeta);

        }

        public bool CompareFooMembers(PageMeta a, PageMeta b)
        {
            if (a.moduleName.Equals(b.moduleName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return moduleName.GetHashCode();
        }

    }
}
