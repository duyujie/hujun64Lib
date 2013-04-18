using System;
using System.Text;
using System.Collections.Generic;
namespace com.hujun64.po
{
    /// <summary>
    ///MainClass 的摘要说明
    /// </summary>
    public class MainClass
    {
        private string _id = "";
        private string _class_name = "";
        private string _class_parent = "";

        private bool _enabled = true;


        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string class_name
        {
            get { return _class_name; }
            set { _class_name = value; }
        }
        public string class_parent
        {
            get { return _class_parent; }
            set { _class_parent = value; }
        }


        public bool enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }


        private List<SiteClass> _siteClassList = new List<SiteClass>();

        public List<SiteClass> siteClassList
        {
            get
            {
                return _siteClassList;
            }
            set
            {
                _siteClassList = siteClassList;
            }
        }

        public void Clone(MainClass mainClass)
        {

            this.id = mainClass.id;
            this.class_name = mainClass.class_name;
            this.class_parent = mainClass.class_parent;


            this.enabled = mainClass.enabled;

            this.siteClassList.Clear();
            foreach (SiteClass siteClass in mainClass.siteClassList)
            {
                siteClassList.Add(siteClass);
            }

        }

        public MainClass()
        {
        }
        public bool IsSiteReldated(string siteId)
        {
            if (siteClassList == null)
                return false;

            foreach (SiteClass sc in siteClassList)
            {
                if (sc.site_id.Equals(siteId))
                    return true;
            }
            return false;
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
            MainClass other = (MainClass)obj;
            if (!Object.Equals(id, other.id)) return false;  //引用类型中无论是什么类型那个的变量都可以用object的静态equals方法进行判断，但是值类型的类的equals方法在这里是有区别的，引用可以用，但是值类型变量就改变了

            return true;
        }
        //当重写了equals方法是，一定要“==”和“！=”进行重写
        public static Boolean operator ==(MainClass o1, MainClass o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(MainClass o1, MainClass o2)
        {
            return !(o1 == o2);
        }
        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + id.GetHashCode();
            hash = (hash * 7) + class_parent.GetHashCode();

            hash = (hash * 7) + enabled.GetHashCode();


            if (siteClassList != null)
                hash = (hash * 7) + siteClassList.GetHashCode();




            return hash;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string splitSymbol = "-";
            sb.Append(id); sb.Append(splitSymbol);
            sb.Append(class_name); sb.Append(splitSymbol);
            sb.Append(class_parent); sb.Append(splitSymbol);


            sb.Append(enabled); sb.Append(splitSymbol);

            sb.Append(siteClassList); sb.Append(splitSymbol);

            return sb.ToString();
        }
    }
}