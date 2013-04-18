using System;
using System.Text;
using System.Collections.Generic;
namespace com.hujun64.po
{
    /// <summary>
    ///MainClass ��ժҪ˵��
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
            //������ʵ����equalsʱ������Ҫ���û����equals������Ȼ���ٽ��б�ģ��������û����дequals��������ô����жϿ���ȥ��
            if (!base.Equals(obj)) return false;
            //��Ϊthis������null���������objΪnull�������������
            if (obj == null)
            {
                return false;
            }
            //���������������Ͳ�һ���������������
            if (this.GetType() != obj.GetType()) return false;
            //
            MainClass other = (MainClass)obj;
            if (!Object.Equals(id, other.id)) return false;  //����������������ʲô�����Ǹ��ı�����������object�ľ�̬equals���������жϣ�����ֵ���͵����equals������������������ģ����ÿ����ã�����ֵ���ͱ����͸ı���

            return true;
        }
        //����д��equals�����ǣ�һ��Ҫ��==���͡���=��������д
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