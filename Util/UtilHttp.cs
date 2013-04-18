using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using com.hujun64.type;
namespace com.hujun64.util
{
    /// <summary>
    ///UtilHttpProxy 的摘要说明
    /// </summary>
    public class UtilHttp
    {

        private static IWebProxy WebProxy=null;
        private static object lockObject = new object();


        private UtilHttp(){
        }
        public static IWebProxy GetWebProxyInstance()
        {
            lock (lockObject)
            {
                if (WebProxy == null)
                {
                    if (string.IsNullOrEmpty(Total.WSProxyUserName))
                        WebProxy = new WebProxy(Total.WSProxyHost, false);
                    else
                        WebProxy = new WebProxy(Total.WSProxyHost, true);


                    if (string.IsNullOrEmpty(Total.WSProxyUserDomain))
                        WebProxy.Credentials = new NetworkCredential(Total.WSProxyUserName, Total.WSProxyPassword);
                    else
                        WebProxy.Credentials = new NetworkCredential(Total.WSProxyUserName, Total.WSProxyPassword, Total.WSProxyUserDomain);
                }
            }
            return WebProxy;
        }

      
        public static string ExecHttpWebRequestUrlGet(string httpWebRequestUrl, Encoding encoding)
        {

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(httpWebRequestUrl);
            Thread.Sleep(1 * 1000);
            myHttpWebRequest.Timeout = System.Threading.Timeout.Infinite;
            myHttpWebRequest.KeepAlive = false;
            myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;

            if (Total.EnableWSProxy)
                myHttpWebRequest.Proxy = UtilHttp.GetWebProxyInstance();

            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.UserAgent = Total.HttpHeaderUserAgent;

            return GetHttpResponse(myHttpWebRequest, encoding);

        }

        private static string GetHttpResponse(HttpWebRequest httpWebRequest, Encoding encoding)
        {
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();


            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流
            StreamReader sr = new StreamReader(receiveStream, encoding);
            string res = sr.ReadToEnd();
            receiveStream.Close();
            myHttpWebResponse.Close();
            return res;
        }

        //string lcPostData =
        // "Name=" + HttpUtility.UrlEncode("Rick Strahl") +
        // "&Company=" + HttpUtility.UrlEncode("West Wind ");

        public static string BuildHttpWebRequestPostData(Dictionary<string, string> postDict)
        {
            StringBuilder postSb = new StringBuilder();
            int i = 0;
            foreach (string key in postDict.Keys)
            {
                if (i > 0)
                    postSb.Append("&");

                postSb.Append(key);
                postSb.Append("=");
                postSb.Append(HttpUtility.UrlEncode(postDict[key]));

                i++;
            }
            return postSb.ToString();
        }
        public static string ExecHttpWebRequestUrlPost(string httpWebRequestUrl, string postData, Encoding encoding)
        {

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(httpWebRequestUrl);
            Thread.Sleep(1 * 1000);
            myHttpWebRequest.Timeout = System.Threading.Timeout.Infinite;
            myHttpWebRequest.KeepAlive = false;
            myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;

            if (Total.EnableWSProxy)
                myHttpWebRequest.Proxy = UtilHttp.GetWebProxyInstance();

            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.UserAgent = Total.HttpHeaderUserAgent;

            // *** Send any POST data
            byte[] postBuffer = encoding.GetBytes(postData);

            myHttpWebRequest.ContentLength = postBuffer.Length;

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = postBuffer.Length;


            Stream newStream = myHttpWebRequest.GetRequestStream();

            newStream.Write(postBuffer, 0, postBuffer.Length);

            newStream.Close();
            string res = GetHttpResponse(myHttpWebRequest, encoding);
            return res;
        }

        public static void CheckSqlHacker(HttpRequest request, HttpResponse response)
        {
            string url = request.ServerVariables["QUERY_STRING"].Trim().ToLower();
            if (string.IsNullOrEmpty(url))
                return;


            if (url.Contains(";") || url.Contains("script") || url.Contains("select%20") || url.Contains("exec%20") || url.Contains("%20or%20") || url.Contains("%20and%20"))
            {
                string SqlHackerWarning = "检测到url中有黑客攻击手段，请立刻停止该行为，否则我站将采取法律行动！";
                response.Write(SqlHackerWarning);
                response.End();

                log4net.ILog log = log4net.LogManager.GetLogger("UtilHttp");
                log.Warn("检测到url中有黑客攻击手段！ IP地址为" + request.UserHostAddress);
            }
        }
        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用 
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    //strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\'); 
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        public static string GetApplicationPath(HttpContext context)
        {
            string applicationPath = context.Request.ApplicationPath;
            if (applicationPath == "")
                applicationPath = "/";
            if (!applicationPath.EndsWith("/"))
                applicationPath += "/";

            return applicationPath;

        }
        public static string GetApplicationPath(HttpContext context, bool withDomainPort)
        {
            string applicationPath = GetApplicationPath(context);
            if (withDomainPort)
            {
                return "http://" + context.Request.Url.Authority + applicationPath;
            }
            return applicationPath;

        }

       public static ResponseCompressionType GetCompressionMode(HttpRequest request)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
                return ResponseCompressionType.None;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            if (acceptEncoding.Contains("GZIP"))
                return ResponseCompressionType.GZip;
            else if (acceptEncoding.Contains("DEFLATE"))
                return ResponseCompressionType.Deflate;
            else
                return ResponseCompressionType.None;
        }
       public static string GetPath(string url)
       {
           var hash = Hash(url);
           string fold = HttpContext.Current.Server.MapPath("~/Temp/");
           return string.Concat(fold, hash);
       }

       public static string Hash(string url)
       {
           url = url.ToUpperInvariant();
           var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
           var bs = md5.ComputeHash(Encoding.ASCII.GetBytes(url));
           var s = new StringBuilder();
           foreach (var b in bs)
           {
               s.Append(b.ToString("x2").ToLower());
           }
           return s.ToString();
       }
    }
}
