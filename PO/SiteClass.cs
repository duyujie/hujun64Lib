using System;
using System.Text;
using com.hujun64.type;
using com.hujun64.util;
namespace com.hujun64.po
{
    /// <summary>
    ///Site 的摘要说明
    /// </summary>
    public class SiteClass
    {

        public string site_id
        {
            get;
            set;
        }
        public MainClass mainClass
        {
            get;
            set;
        }
        public string category
        {
            get;
            set;
        }
        public int sort_seq
        {
            get;
            set;
        }
        private string _template_url = "";
        private string _img_url = "";
        public string template_url
        {
            get { return _template_url; }
            set { _template_url = value; }
        }
        public string img_url
        {
            get { return _img_url; }
            set { _img_url = value; }
        }
        public override bool Equals(object obj)
        {
            //当基类实现了equals时，首先要调用基类的equals方法，然后再进行别的，如果基类没有重写equals方法，那么这个判断可以去掉
            if (!base.Equals(obj)) return false;
            //因为this不肯能null，所以如果obj为null，将不可能相等
            if (obj == null)
            {
                return false;
            }
            //如果两个对象的类型不一样，将不可能相等
            if (this.GetType() != obj.GetType()) return false;
            //
            SiteClass other = (SiteClass)obj;
            if (!(Object.Equals(site_id, other.site_id) && Object.Equals(mainClass, other.mainClass))) return false;  //引用类型中无论是什么类型那个的变量都可以用object的静态equals方法进行判断，但是值类型的类的equals方法在这里是有区别的，引用可以用，但是值类型变量就改变了

            return true;
        }
        //当重写了equals方法是，一定要“==”和“！=”进行重写
        public static Boolean operator ==(SiteClass o1, SiteClass o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(SiteClass o1, SiteClass o2)
        {
            return !(o1 == o2);
        }
        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + site_id.GetHashCode();
            hash = (hash * 7) + mainClass.GetHashCode();
            hash = (hash * 7) + category.GetHashCode();
            hash = (hash * 7) + sort_seq.GetHashCode();
            hash = (hash * 7) + template_url.GetHashCode();
            hash = (hash * 7) + img_url.GetHashCode();
            return hash;
        }
        public void Clone(SiteClass siteClass)
        {
            if (this.mainClass == null)
                mainClass = new MainClass();
            this.mainClass.Clone(siteClass.mainClass);
            this.category = siteClass.category;
            this.site_id = siteClass.site_id;
            this.sort_seq = siteClass.sort_seq;
            this.template_url = siteClass.template_url;
            this.img_url = siteClass.img_url;
          

        }


    }
}