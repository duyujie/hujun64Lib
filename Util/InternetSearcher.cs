using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Text;


using com.hujun64.po;

namespace com.hujun64
{
    public class InternetSearcher
    {        
      
        public static string GetGoogleUrl(string key)
        {
            string plusString = "+";
            StringBuilder sb = new StringBuilder(Total.AspxUrlSearchGoogle);
            sb.Append(plusString);
            while (key.Contains(" "))
            {
                key = key.Replace(" ", plusString);
            }
            sb.Append(HttpUtility.UrlEncode(key));
            return sb.ToString();
        }


        /***********
             * baidu参数： 
             * wd——查询的关键词(Keyword) 
             * bs——上一次搜索的关键词(Before Search)，估计与相关搜索有关 
             * 组合关键词搜索连接符号“+”
             * 
             * 
             * google参数：
             * q/as_q–查询的关键词(Query)，百度对应的参数为wd 
             * 组合关键词搜索连接符号“+”
             * 
             * Soso refer rule 
             * “w=”  搜索关键词 
             * “bs=” 上次搜索关键词 
             * 组合关键词搜索连接符号“+”
             * 
             * youdao refer rule 
             * “q=” 搜索关键词 
             * 组合关键词搜索连接符号“+”
             * 
             * sougou refer rule 
             * “query=” 搜索关键词 
             * 组合关键词搜索链接符号为“+” 

             * yahoo refer rule 
             * “p=” 搜索关键词 
             * 组合关键词搜索链接符号为“+” 
             * 
             * 
             * ************/
        public static string GetKeyWordsRef(string searchedUrl)
        {

            string[] searchEngineerArray=new string[]{ "baidu","google","soso","sougou","yahoo","youdao"};


            return null;


        }
    }
}