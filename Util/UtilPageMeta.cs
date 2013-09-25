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
    public class UtilPageMeta
    {
        private static List<PageMeta> metaList = new List<PageMeta>();
        private static readonly object Locker = new object();
        private UtilPageMeta()
        {

        }
        public static PageMeta GetPageMeta(string metaModuleName)
        {
            string xmlPath = HttpContext.Current.Server.MapPath("~/") + "/" + Total.PageMetaXml;
            List<PageMeta> list = GetPageMetaList(xmlPath);
            if (list!=null&&list.Count > 0)
            {
                lock (Locker)
                {
                    foreach (PageMeta meta in list)
                    {
                        if (meta == null || meta.moduleName == null)
                            break;
                        else
                            if (meta.moduleName.Equals(metaModuleName))
                                return meta;
                    }
                }
            }
            return GetDefaultPageMeta();
        }
        private static PageMeta GetDefaultPageMeta()
        {
            PageMeta defaultMeta = new PageMeta();
            defaultMeta.moduleName = Total.SiteName;
            defaultMeta.title = Total.Title; ;
            defaultMeta.keywords = Total.Keywords;
            defaultMeta.description = Total.Description;

            return defaultMeta;
        }

        public static List<PageMeta> GetPageMetaList(string meataXmlPath, bool forceRead = false)
        {
            if (!forceRead && metaList.Count > 0)
                return metaList;

            DataSet dsxml = new DataSet();
            dsxml.ReadXml(meataXmlPath);

            lock (Locker)
            {
                foreach (DataTable table in dsxml.Tables)
                {
                    if (table.TableName.Equals("meta"))
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            PageMeta meta = new PageMeta();

                            meta.moduleName = row["moduleName"].ToString();
                            meta.title = row["title"].ToString();
                            meta.keywords = row["keywords"].ToString();
                            meta.description = row["description"].ToString();

                            if (metaList.Contains(meta))
                                metaList.Remove(meta);

                            metaList.Add(meta);
                        }


                    }

                }
            }
            dsxml = null;
            return metaList;
        }

    }


}