using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using com.hujun64.type;
using com.hujun64.util;

namespace com.hujun64.util
{
    /// <summary>
    ///UtilSitemap 的摘要说明
    /// </summary>
    public class UtilSitemap
    {
              
       
        private string rootPath = HttpContext.Current.Server.MapPath("~/");

        private UtilSitemap()
        {
            
        }

        private static string BuildXmlHeader4Google()
        {
            StringBuilder sitemapSb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            sitemapSb.Append("<urlset xmlns=\"http://www.google.com/schemas/sitemap/0.84\"\n");
            sitemapSb.Append("\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\n");
            sitemapSb.Append("\txsi:schemaLocation=\"http://www.google.com/schemas/sitemap/0.84\n");
            sitemapSb.Append("\thttp://www.google.com/schemas/sitemap/0.84/sitemap.xsd\">\n");


            return sitemapSb.ToString();
        }
        private static string BuildXmlFooter4Google()
        {
            return "</urlset>";
        }
        public static void NewSitemapXml(string sitemapPath, List<SitemapUrl> sitemapUrlList, SitemapType sitemapType)
        {
            if (sitemapType == SitemapType.Google)
                NewSitemap4Google(sitemapPath, sitemapUrlList);
        }
        private static void NewSitemap4Google(string sitemapPath, List<SitemapUrl> sitemapUrlList)
        {
           

            if (!File.Exists(sitemapPath))
            {
                FileStream fs = File.Create(sitemapPath);
                fs.Close();

            }
            StringBuilder sitemapSb = new StringBuilder();

            //xml header
            sitemapSb.Append(BuildXmlHeader4Google());

            //xml body
            SitemapUrl siteSitemapUrl = new SitemapUrl();
            siteSitemapUrl.changefreq = FreqEnum.always;
            siteSitemapUrl.lastmod = DateTime.Now;
            siteSitemapUrl.loc = Total.SiteUrl;
            siteSitemapUrl.priority = PriorityType.VERY_HIGH;

            sitemapSb.Append(siteSitemapUrl.ToUrlNodeXml());

            foreach (SitemapUrl sitemapUrl in sitemapUrlList)
            {
                sitemapSb.Append(sitemapUrl.ToUrlNodeXml());

            }


            //xml footer
            sitemapSb.Append(BuildXmlFooter4Google());


            UtilFile.WriteStringFile(sitemapSb.ToString(), sitemapPath, false, Encoding.UTF8);


        }
        public static void UpdateSitemapTxt(string rootPath, IList<string> urlList, bool isCreate=false)
        {
            string sitemapPath = rootPath + "/sitemap.txt";
            sitemapPath=sitemapPath.Replace("//", "/");

            if (!File.Exists(sitemapPath) || isCreate)
            {
                FileStream fs = File.Create(sitemapPath);
                fs.Close();

            }



            string siteUrl = Total.SiteUrl + "/";



            string contentString = UtilFile.ReadTextFile(sitemapPath);
            StringBuilder newContent = new StringBuilder(contentString);
            StringBuilder htmlUrlSb = new StringBuilder();


            foreach (string fullHtmlUrl in urlList)
            {
                htmlUrlSb.Remove(0, htmlUrlSb.Length);
                htmlUrlSb.Append(fullHtmlUrl);
                htmlUrlSb.Replace(rootPath, siteUrl);
                htmlUrlSb.Replace("\\", "/");
                if (!contentString.Contains(htmlUrlSb.ToString()))
                {
                    newContent.Append(htmlUrlSb);
                    newContent.Append("\r\n");
                }
            }

            UtilFile.WriteStringFile(newContent.ToString(), sitemapPath);
        }
        public static void UpdateSitemapXml(string sitemapPath, List<SitemapUrl> sitemapUrlList,SitemapType sitemapType,bool isCreate=false)
        {
            if (sitemapType == SitemapType.Google)
            {
                if(isCreate)
                    NewSitemap4Google(sitemapPath, sitemapUrlList);
                else
                    UpdateSitemap4Google(sitemapPath, sitemapUrlList, false);
            }
            
        }
        public static void DeleteSitemapXml(string sitemapPath,string filePath, SitemapType sitemapType)
        {
            if (sitemapType == SitemapType.Google){
                
                  List<SitemapUrl> deleteSitemapUrl=new List<SitemapUrl>();
                  SitemapUrl sitemapUrl = new SitemapUrl(filePath.Replace(HttpContext.Current.Server.MapPath("~/"), Total.SiteUrl + "/").Replace("\\", "/"));
                  deleteSitemapUrl.Add(sitemapUrl);

                  UpdateSitemap4Google(sitemapPath, deleteSitemapUrl, true);
            }

        }
      
        private static void UpdateSitemap4Google(string sitemapPath, List<SitemapUrl> sitemapUrlList,bool isDelete)
        {
            DataSet dsxml = new DataSet();
            dsxml.ReadXml(sitemapPath);
           
                foreach (SitemapUrl sitemapUrl in sitemapUrlList)
                {
                    bool isExists = false;
                    foreach (DataTable table in dsxml.Tables)
                    {
                        if (table.TableName.Equals("url"))
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                //loc,lastmod,changereq,priority
                                if (sitemapUrl.loc.Equals(row["loc"].ToString()))
                                {
                                    isExists = true;

                                    if (isDelete)
                                    {
                                        row.Delete();
                                    }
                                    else
                                    {                                        
                                        SetDataRow4Google(row, sitemapUrl);
                                       
                                    }
                                    break;
                                }
                            }
                            //新的url，需要新增节点
                            if (!isExists)
                            {
                                DataRow row = table.Rows.Add();
                                SetDataRow4Google(row, sitemapUrl);

                            }
                        }
                    }

            }
            dsxml.AcceptChanges();
            dsxml.WriteXml(sitemapPath);
            dsxml.Clear();
        }
        private static DataRow SetDataRow4Google(DataRow row, SitemapUrl sitemapUrl)
          {
              row["loc"] = sitemapUrl.loc;
              row["lastmod"] = sitemapUrl.lastmod.ToString("yyyy-MM-ddTHH:mm:sszzz");
              row["changefreq"] = sitemapUrl.changefreq;
              row["priority"] = sitemapUrl.priority;

              return row;
          }
    }

  
}