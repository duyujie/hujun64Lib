using System;
using System.Text;
using com.hujun64.type;
using com.hujun64.util;
namespace com.hujun64.po
{
    /// <summary>
    ///Site ��ժҪ˵��
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
            SiteClass other = (SiteClass)obj;
            if (!(Object.Equals(site_id, other.site_id) && Object.Equals(mainClass, other.mainClass))) return false;  //����������������ʲô�����Ǹ��ı�����������object�ľ�̬equals���������жϣ�����ֵ���͵����equals������������������ģ����ÿ����ã�����ֵ���ͱ����͸ı���

            return true;
        }
        //����д��equals�����ǣ�һ��Ҫ��==���͡���=��������д
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