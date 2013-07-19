
using System;

using System.Collections.Generic;
using System.Text;

namespace com.hujun64
{
    /// <summary>
    ///UtilString 的摘要说明
    /// </summary>
    public class UtilString
    {
        private UtilString()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static string GetPureFilename(string fullFilename)
        {

            return GetPureUrl(fullFilename, false);
        }
        public static string GetPureUrl(string url, bool withParam)
        {
            int i = url.LastIndexOf("\\");
            if (i >= 0)
            {
                url = url.Substring(i + 1);
            }
            i = url.LastIndexOf("/");
            if (i >= 0)
            {
                url = url.Substring(i + 1);
            }
            if (!withParam)
            {
                i = url.IndexOf("?");
                if (i >= 0)
                {
                    url = url.Substring(0, i);
                }
            }
            return url;
        }
        public static string ConvertToCountSql(string selectSql)
        {
            StringBuilder countSqlSb = new StringBuilder("select count(1) from (");


            int orderIndex = selectSql.IndexOf("order by");
            if (orderIndex > 0)
            {
                countSqlSb.Append(selectSql.Remove(orderIndex));
            }
            else
            {
                countSqlSb.Append(selectSql);
            }


            countSqlSb.Append(") as lawyerCountTemp");


            return countSqlSb.ToString();
        }

        public static string ConvertAuthorName(string name, string sex)
        {
            string sexMale = "先生";
            string sexFemale = "女士";
            string sexUnknown = "先生/女士";

            StringBuilder nameSex = new StringBuilder();

            if (!string.IsNullOrEmpty(name))
                nameSex.Append(name.Substring(0, 1));


            if (sex == "男")
            {
                nameSex.Append(sexMale);
            }
            else
                if (sex == "女")
                {
                    nameSex.Append(sexFemale);
                }
                else
                {
                    nameSex.Append(sexUnknown);
                }
            return nameSex.ToString();

        }
       
        public static string GetDayTips()
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy年MM月dd日 "));
            sb.Append(Day[Convert.ToInt32(DateTime.Now.DayOfWeek)]);

            return sb.ToString();
        }
        
       
    }
}