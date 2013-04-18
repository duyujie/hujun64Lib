using System;
using System.Text;
using com.hujun64.type;

namespace com.hujun64.util
{
    /// <summary>
    ///Sitemap 的摘要说明
    /// </summary>
    public class SitemapUrl
    {
        public FreqEnum changefreq;
        public DateTime lastmod;
        public string loc;
        public float priority = PriorityType.VERY_LOW;

        public SitemapUrl()
        {
        }
        public SitemapUrl(string location)
        {
            this.loc = location;
        }

        public string ToUrlNodeXml()
        {
            StringBuilder xmlSb = new StringBuilder();
            xmlSb.Append("\t<url>\n");

            xmlSb.Append("\t\t<loc>");
            xmlSb.Append(loc);
            xmlSb.Append("</loc>\n");

            if (lastmod==null || lastmod.Equals(DateTime.MinValue))
                lastmod = DateTime.Now;
            xmlSb.Append("\t\t<lastmod>");            
            xmlSb.Append(lastmod.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            xmlSb.Append("</lastmod>\n");

            xmlSb.Append("\t\t<changefreq>");
            xmlSb.Append(changefreq);
            xmlSb.Append("</changefreq>\n");

            xmlSb.Append("\t\t<priority>");
            xmlSb.Append(priority);
            xmlSb.Append("</priority>\n");

            xmlSb.Append("\t</url>\n");
            return xmlSb.ToString();
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

              this, right as SitemapUrl);

        }

        public bool CompareFooMembers(SitemapUrl a, SitemapUrl b)
        {
            if (a.loc.Equals( b.loc))
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
            return loc.GetHashCode();
        }

    }
}
