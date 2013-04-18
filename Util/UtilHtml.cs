using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web;
using com.hujun64.logic;
using com.hujun64.po;
using com.hujun64.type;
namespace com.hujun64.util
{
    /// <summary>
    ///UtilHtml 的摘要说明
    /// </summary>
    public class UtilHtml
    {
        private static readonly int MAX_IMG_TITLE_LEN = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MAX_IMG_TITLE_LEN"]);
        private static readonly int MAX_NO_IMG_TITLE_LEN = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MAX_NO_IMG_TITLE_LEN"]);
        private static readonly int MAX_LEFT_TITLE_LEN = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MAX_LEFT_TITLE_LEN"]);
        private static readonly int NEW_IMG_LEN = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NEW_IMG_LEN"]);
        private static readonly int MAX_DESC_LEN = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MAX_DESC_LEN"]);
        public static readonly string EnterBreak = "\r\n";
        public static readonly string HtmlSpace = "&nbsp;";
        public static readonly string HtmlEnterBreak = "<br />";
        public static readonly string HtmlDoubleSpace = "&nbsp;&nbsp;";
        private static readonly string TitlePostfix = "…";


        private UtilHtml()
        {
        }
        public static string GetSite(string url)
        {
            if (url == null)
                return "";

            Regex urlRegExp = new Regex(@"https?://(.*?)+?/", RegexOptions.IgnoreCase);
            if (urlRegExp.IsMatch(url))
            {
                string matchedUrl = urlRegExp.Match(url).Value;
                url = matchedUrl.Substring(0, matchedUrl.Length - 1);
            }


            if (url.IndexOf("://") > 0)
            {
                url = url.Substring(url.IndexOf("://") + 3);
            }
            if (url.IndexOf("/") > 0)
            {
                return url.Substring(0, url.IndexOf("/"));
            }
            else
            {
                return url;
            }

        }
        public static Dictionary<string, string> GetCookieDict(string cookieString)
        {


            string[] cookieArray = cookieString.Split(';');
            Dictionary<string, string> cookieDict = new Dictionary<string, string>(cookieArray.Length);

            foreach (string cookie in cookieArray)
            {
                string[] cookieNameValue = cookie.Split('=');
                cookieDict.Add(cookieNameValue[0].Trim(), cookieNameValue[1].Trim());
            }

            return cookieDict;
        }
        public static string ExtractMetaKeywords(Article article)
        {
            if (article == null)
                return "";

            if(!string.IsNullOrEmpty(article.keywords))
                return article.keywords;

            return ExtractMetaDesc(article);

        }

        public static string ExtractMetaDesc(Article article)
        {
            if (article == null)
                return "";


            StringBuilder sb = new StringBuilder();
            sb.Append("文章标题：");
            sb.Append(article.title);


            if (!string.IsNullOrEmpty(article.content))
            {
                sb.Append("；内容描述：");
                sb.Append(TitleSubstring(RemoveHtmlTag(article.content), MAX_DESC_LEN));
            }
            sb.Append("；关键字：");


            if (string.IsNullOrEmpty(article.keywords))
            {
                sb.Append(article.keywords);
                sb.Append("|");
            }
            sb.Append(Total.Keywords);
            sb.Append("；作者：");
            if (!string.IsNullOrEmpty(article.author))
            {
                sb.Append(article.author);
            }
            else
            {
                sb.Append("匿名");
            }

            sb.Append("；日期：");
            sb.Append(article.addtime);
            sb.Append("；来源：");
            if (!string.IsNullOrEmpty(article.news_from))
            {
                sb.Append(article.news_from);
            }
            else
            {
                sb.Append("网络");
            }


            return sb.ToString();

        }

        public static string ExtractMetaDesc(Guestbook guestbook)
        {
            if (guestbook == null)
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("咨询标题：");
            sb.Append(guestbook.title);



            if (!string.IsNullOrEmpty(guestbook.reply))
            {
                if (!string.IsNullOrEmpty(guestbook.content))
                {
                    sb.Append("； 咨询描述：");
                    sb.Append(TitleSubstring(RemoveHtmlTag(guestbook.content), MAX_DESC_LEN / 2));
                }
                if (!string.IsNullOrEmpty(guestbook.reply))
                {
                    sb.Append("；律师回复：");
                    sb.Append(TitleSubstring(RemoveHtmlTag(guestbook.reply), MAX_DESC_LEN / 2));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(guestbook.content))
                {
                    sb.Append("；咨询描述：");
                    sb.Append(TitleSubstring(RemoveHtmlTag(guestbook.content), MAX_DESC_LEN));
                }
            }

            sb.Append("；关键字：");

            if (string.IsNullOrEmpty(guestbook.keywords))
            {
                sb.Append(guestbook.keywords);
                sb.Append("|");

            }
            sb.Append(Total.Keywords);
            sb.Append("；提问者：");
            sb.Append(UtilString.ConvertAuthorName(guestbook.author, guestbook.sex));
            sb.Append("；日期：");
            sb.Append(guestbook.addtime);
            return sb.ToString();

        }

        /**/
        /// <summary>
        /// 去除所有的Html标签
        /// </summary>
        /// <param name="htmlContent">待转化的字符串</param>
        /// <returns>经过转化的字符串</returns>

        public static string RemoveHtmlTag(string htmlContent)
        {
            return RemoveHtmlTag(htmlContent, false);

        }
        public static string RemoveHtmlTag(string htmlContent, bool reduceSpace)
        {
            if (string.IsNullOrEmpty(htmlContent))
                return htmlContent;

            StringBuilder contentSb = new StringBuilder(htmlContent);
            contentSb.Replace("&nbsp;", " ");

            Regex objRegExp = new Regex("<(.|)+?>");

            while (objRegExp.IsMatch(htmlContent))
            {
                contentSb.Remove(0, contentSb.Length);
                contentSb.Append(objRegExp.Replace(htmlContent, ""));
                contentSb.Replace("<", "&lt;");
                contentSb.Replace(">", "&gt;");
                htmlContent = contentSb.ToString();
            }

            if (reduceSpace)
                return ReduceSpace(contentSb.ToString().Trim());
            else
                return contentSb.ToString().Trim();
        }

        ////把所有空格变为一个空格            
        public static string ReduceSpace(string s)
        {
            Regex r = new Regex(@"\s+");
            return r.Replace(s, " ");
        }
        public static string RemoveEnterBreak(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            else
                return s.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

        }
        public static string ConvertAspxWithHtml(string urlAspx)
        {
            if (urlAspx == null)
                return null;

            StringBuilder htmlUrlSb = new StringBuilder(urlAspx);

            bool keepAspx = false;


            if (urlAspx.Equals(Total.AspxUrlGuestAsk) || urlAspx.Contains(Total.AspxUrlSearch) ||
                urlAspx.Contains(Total.AspxUrlStat) || urlAspx.Contains(Total.AspxUrlStatClick) ||
                urlAspx.Equals(Total.AspxUrlLinkApp) || urlAspx.Contains(Total.AspxUrlWebService))
            {
                keepAspx = true;
            }

            if (!keepAspx)
            {
                htmlUrlSb.Remove(0, htmlUrlSb.Length);
                htmlUrlSb.Append(ReplaceAspxWithHtml(urlAspx));
            }
            if (htmlUrlSb.ToString().StartsWith("/"))
            {
                htmlUrlSb.Replace(Total.ApplicationPath, "");
            }

            if (!keepAspx)
            {
                if (!urlAspx.StartsWith(Total.AspxUrlIndex))
                {
                    if (htmlUrlSb.ToString().StartsWith("/"))
                    {
                        htmlUrlSb.Insert(0, Total.HtmlPath);
                    }
                    else
                    {
                        htmlUrlSb.Insert(0, Total.HtmlPath + "/");
                    }
                }
            }


          

            if (Total.IsFullSiteurl)
                htmlUrlSb.Insert(0, Total.CurrentSiteRootUrl);
            else
                htmlUrlSb.Insert(0, "/");

            return htmlUrlSb.ToString();
        }
        public static string TrimHtml(string contentHtml)
        {
            contentHtml = contentHtml.Trim(HtmlSpace.ToCharArray());
            return contentHtml.Trim(EnterBreak.ToCharArray());

        }
        public static string TrimHtml(StringBuilder contentHtmlSb)
        {
            string contentHtml = contentHtmlSb.ToString().Trim(HtmlSpace.ToCharArray());
            contentHtml = contentHtml.Trim(EnterBreak.ToCharArray());
            contentHtmlSb.Remove(0, contentHtmlSb.Length);
            contentHtmlSb.Append(contentHtml);
            return contentHtml.ToString();

        }
        public static string FormatTextToHtml(string contentText)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(contentText);
            sb.Replace(EnterBreak, HtmlEnterBreak);
            sb.Replace(" ", HtmlDoubleSpace);

            return sb.ToString();
        }
        public static string FormatTextToHtmlWithParagraph(string contentText, bool removeAllSpace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(contentText);
            sb.Replace("\t", "");
            sb.Replace("　", "");
            if (removeAllSpace)
            {
                sb.Replace(" ", "");
                sb.Replace("&nbsp;", "");
            }
            sb.Replace(EnterBreak + EnterBreak, "</p><p>");
            sb.Replace(EnterBreak, HtmlEnterBreak + HtmlDoubleSpace + HtmlDoubleSpace);
            sb.Replace("<p></p>", "");
            sb.Append("</p>");
            return sb.ToString();
        }
        public static string AppendSpaceToHtmlItem(string contentHtml)
        {

            StringBuilder contentSb = new StringBuilder();
            Regex htmlRegExp = new Regex(@"(>s*?)+?第.*?(条|章|节)", RegexOptions.IgnoreCase);

            int beginIndex = 0;
            Match match = htmlRegExp.Match(contentHtml);
            while (match.Success)
            {

                contentSb.Append(contentHtml.Substring(beginIndex, match.Index - beginIndex));
                contentSb.Append(match.Value);

                beginIndex = match.Index + match.Length;
                if (!contentHtml.Substring(beginIndex).StartsWith(HtmlSpace))
                    contentSb.Append(HtmlSpace);


                match = match.NextMatch();

            }
            contentSb.Append(contentHtml.Substring(beginIndex));

            return contentSb.ToString();
        }
        public static string FormatHtmlToText(string contentHtml)
        {
            contentHtml = contentHtml.Replace(EnterBreak, "");
            StringBuilder contentSb = new StringBuilder(contentHtml);
            Regex htmlRegExp = new Regex(@"<(p.|p|p\s*?.*?)+?>", RegexOptions.IgnoreCase);


            while (htmlRegExp.IsMatch(contentHtml))
            {
                contentSb.Remove(0, contentSb.Length);
                contentSb.Append(htmlRegExp.Replace(contentHtml, EnterBreak + EnterBreak));
                contentHtml = contentSb.ToString();
            }
            htmlRegExp = new Regex(@"<(\s*?/p|/p|/\s*?p)>", RegexOptions.IgnoreCase);
            while (htmlRegExp.IsMatch(contentHtml))
            {
                contentSb.Remove(0, contentSb.Length);
                contentSb.Append(htmlRegExp.Replace(contentHtml, ""));
                contentHtml = contentSb.ToString();
            }
            htmlRegExp = new Regex(@"<(br.|br|br\s*?.*?)+?>", RegexOptions.IgnoreCase);
            while (htmlRegExp.IsMatch(contentHtml))
            {
                contentSb.Remove(0, contentSb.Length);
                contentSb.Append(htmlRegExp.Replace(contentHtml, EnterBreak));
                contentHtml = contentSb.ToString();
            }

            contentSb.Replace(HtmlDoubleSpace, " ");
            contentSb.Replace(HtmlSpace, " ");
            return contentSb.ToString();
        }
        public static StringBuilder TrimPairTag(StringBuilder contentSb, string startTag, string endTag)
        {
            if (contentSb == null)
                return contentSb;

            TrimHtml(contentSb);

            while (contentSb.ToString().StartsWith(startTag))
            {
                contentSb.Remove(0, startTag.Length);
                if (contentSb.ToString().EndsWith(endTag))
                {
                    contentSb.Remove(contentSb.Length - endTag.Length, endTag.Length);
                }
                TrimHtml(contentSb);
            }
            return contentSb;
        }
        public static string FormatArticleContent(string contentHtml)
        {
            return FormatTextToHtmlWithParagraph(FormatHtmlToText(contentHtml).Trim(), false);
        }
        public static string FormatArticleContent(string contentHtml, bool isDefaultStyle)
        {
            StringBuilder defaultStartTagSb = new StringBuilder("<div class=\"");
            defaultStartTagSb.Append(Total.DivDefaultClassContent);
            defaultStartTagSb.Append("\" id=\"");
            defaultStartTagSb.Append(Total.DivIdContent);
            defaultStartTagSb.Append("\">");

            StringBuilder definedStartTagSb = new StringBuilder("<div class=\"");
            definedStartTagSb.Append(Total.DivDefinedClassContent);
            definedStartTagSb.Append("\" id=\"");
            definedStartTagSb.Append(Total.DivIdContent);
            definedStartTagSb.Append("\">");

            string endTag = "</div>";

            StringBuilder sb = new StringBuilder();
            if (isDefaultStyle)
            {
                sb.Append(AppendSpaceToHtmlItem(FormatTextToHtmlWithParagraph(RemoveHtmlTag(FormatHtmlToText(contentHtml)).Trim(), false)));
                TrimPairTag(sb, defaultStartTagSb.ToString(), endTag);
                TrimPairTag(sb, definedStartTagSb.ToString(), endTag);

                sb.Insert(0, defaultStartTagSb);
                sb.Append(endTag);

                return sb.ToString();
            }
            else
            {
                sb.Append(contentHtml);
                TrimPairTag(sb, defaultStartTagSb.ToString(), endTag);
                TrimPairTag(sb, definedStartTagSb.ToString(), endTag);

                sb.Insert(0, definedStartTagSb);
                sb.Append(endTag);

                return sb.ToString();
            }

        }
        public static string FormatKeywords(String keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return "";

            StringBuilder keywordsSb = new StringBuilder(keywords.Trim());

            keywordsSb.Replace("，", ",");
            keywordsSb.Replace("、", ",");
            keywordsSb.Replace(" ", ",");
            keywordsSb.Replace(";", ",");
            keywordsSb.Replace("-", ",");
            while (keywordsSb.ToString().Contains(",,"))
            {
                keywordsSb.Replace(",,", ",");
            }
            return keywordsSb.ToString();
        }
        public static string ReplaceAspxWithHtml(string urlAspx)
        {
            if (urlAspx == null)
                return "";

            string splitSymbol = "_";
            bool isAspx = false;
            if (urlAspx.Contains(".aspx"))
                isAspx = true;

            StringBuilder url = new StringBuilder(urlAspx);

            String locUrl="";
            if (url.ToString().Contains("#"))
            {
                int locIndex=url.ToString().IndexOf("#");
                locUrl = url.ToString().Substring(locIndex);
                url.Remove(locIndex, locUrl.Length);
            }
            url.Replace(".aspx", "");
            url.Replace("?", splitSymbol);
            url.Replace("=", splitSymbol);
            url.Replace("&", splitSymbol);
            if (isAspx)
            {
                int anchorIndex=url.ToString().LastIndexOf('#');
                if (anchorIndex > 0)
                    url.Insert(anchorIndex, ".html");
                else
                    url.Append(".html");
            }

            url.Append(locUrl);
            return url.ToString();
        }





        public static PageInfo GetPageInfoOfRequet(HttpRequest request)
        {


            string idPrefix = "id_";
            string urlSuffix = ".html";

            Regex idRegExp = new Regex(idPrefix + @"\w?\d{1,}");
            PageInfo pageInfo = new PageInfo();


            pageInfo.clickSource = request.QueryString[Total.QueryStringReferrer];
            pageInfo.userIp = request.UserHostAddress;
            pageInfo.httpHost = request.ServerVariables["HTTP_HOST"];

            string url = request.UrlReferrer.PathAndQuery;


            //不包含后缀的纯粹文件名
            string pureHtmlFilename = ReplaceAspxWithHtml(UtilString.GetPureUrl(url, true)).Replace(urlSuffix, "");

            Match match = idRegExp.Match(pureHtmlFilename);
            string idParam = match.ToString();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            while (!string.IsNullOrEmpty(idParam))
            {
                if (i > 0)
                    sb.Append(",");
                sb.Append(idParam.Substring(idPrefix.Length));


                match = match.NextMatch();
                idParam = match.ToString();
                i++;
            }
            pageInfo.pageId = sb.ToString();



            if (pureHtmlFilename.StartsWith(Total.HtmlUrlIndex.Replace(urlSuffix, "")))
            {
                pageInfo.pageType = PageType.INDEX_TYPE;
            }
            else
                if (pureHtmlFilename.StartsWith(Total.HtmlUrlShowdetail.Replace(urlSuffix, "")))
                {

                    pageInfo.pageType = PageType.ARTICLE_TYPE;
                }
                else
                    if (pureHtmlFilename.StartsWith(Total.HtmlUrlGuestbook.Replace(urlSuffix, "")))
                    {
                        pageInfo.pageType = PageType.GUESTBOOK_TYPE;
                    }
                    else
                        if (pureHtmlFilename.StartsWith(Total.HtmlUrlIntro.Replace(urlSuffix, "")))
                        {
                            pageInfo.pageType = PageType.INTRO_TYPE;
                        }
                        else
                            if (pureHtmlFilename.StartsWith(Total.HtmlUrlFee.Replace(urlSuffix, "")))
                            {
                                pageInfo.pageType = PageType.FEE_TYPE;
                            }

                            else
                                if (pureHtmlFilename.StartsWith(Total.HtmlUrlModule.Replace(urlSuffix, "")))
                                {
                                    pageInfo.pageType = PageType.MODULE_TYPE;

                                }
                                else
                                    if (pureHtmlFilename.StartsWith(Total.HtmlUrlBigClass.Replace(urlSuffix, "")))
                                    {
                                        pageInfo.pageType = PageType.BIG_CLASS_TYPE;

                                    }
                                    else
                                        if (pureHtmlFilename.StartsWith(Total.HtmlUrlSmallClass.Replace(urlSuffix, "")))
                                        {
                                            pageInfo.pageType = PageType.SMALL_CLASS_TYPE;
                                        }
                                        else
                                            if (pureHtmlFilename.StartsWith(Total.HtmlUrlGuestAsk.Replace(urlSuffix, "")))
                                            {
                                                pageInfo.pageType = PageType.GUESTASK_TYPE;
                                            }
                                            else
                                                if (pureHtmlFilename.StartsWith(Total.HtmlUrlBuilding.Replace(urlSuffix, "")))
                                                {
                                                    pageInfo.pageType = PageType.BUILDING_TYPE;
                                                }
                                                else
                                                    if (pureHtmlFilename.StartsWith(Total.HtmlUrlLinkAll.Replace(urlSuffix, "")))
                                                    {
                                                        pageInfo.pageType = PageType.LINK_ALL_TYPE;

                                                    }
                                                    else
                                                        if (pureHtmlFilename.StartsWith(Total.HtmlUrlLinkApp.Replace(urlSuffix, "")))
                                                        {
                                                            pageInfo.pageType = PageType.LINK_APP_TYPE;
                                                        }
                                                        else
                                                            if (pureHtmlFilename.StartsWith(Total.HtmlUrlWebService.Replace(urlSuffix, "")))
                                                            {
                                                                pageInfo.pageType = PageType.WEBSERVICE_TYPE;
                                                            }
                                                            else
                                                            {
                                                                pageInfo.pageType = PageType.INDEX_TYPE;
                                                            }

            if (string.IsNullOrEmpty(pageInfo.pageId))
                pageInfo.pageId = logic.ServiceFactory.GetMainClassService().GetCachedClassByPageType(pageInfo.pageType).id;

            return pageInfo;
        }
        public static string GetHtmlUrl(string id, PageType pageType)
        {
            string aspxUrl = GetAspxUrl(id, pageType);
            if (pageType == PageType.INDEX_TYPE)
                return ReplaceAspxWithHtml(aspxUrl);
            else
                return Total.HtmlPath + "/" + ReplaceAspxWithHtml(aspxUrl);
        }
        public static string GetAspxUrl(PageType pageType)
        {
            return GetAspxUrl(null, pageType);
        }
        public static string GetAspxUrl(string id, PageType pageType)
        {
            if (pageType == PageType.DYNAMIC_TYPE)
            {
                if (string.IsNullOrEmpty(id))
                {
                    return "";
                }
                else if (id.StartsWith(Total.PrefixArticleId))
                {
                    pageType = PageType.ARTICLE_TYPE;
                }
                else if (id.StartsWith(Total.PrefixGuestbookId))
                {
                    pageType = PageType.GUESTBOOK_TYPE;
                }
                else if (id.StartsWith(Total.PrefixLinkId))
                {
                    pageType = PageType.LINK_ALL_TYPE;
                }
                else
                    return "";


            }

            StringBuilder aspxUrlSb = new StringBuilder();

            switch (pageType)
            {
                case PageType.INDEX_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlIndex);
                    break;
                case PageType.INTRO_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlIntro);
                    if (!string.IsNullOrEmpty(id))
                    {
                        aspxUrlSb.Append("?");
                        aspxUrlSb.Append(Total.QueryStringIntroId);
                        aspxUrlSb.Append("=");
                        aspxUrlSb.Append(id);
                    }
                    break;
                case PageType.FEE_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlFee);
                    if (!string.IsNullOrEmpty(id))
                    {
                        aspxUrlSb.Append("?");
                        aspxUrlSb.Append(Total.QueryStringFeeId);
                        aspxUrlSb.Append("=");
                        aspxUrlSb.Append(id);
                    }
                    break;
                case PageType.MODULE_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlModule);
                    aspxUrlSb.Append("?");
                    aspxUrlSb.Append(Total.QueryStringModuleClassId);
                    aspxUrlSb.Append("=");
                    aspxUrlSb.Append(id);
                    break;
                case PageType.BIG_CLASS_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlBigClass);
                    aspxUrlSb.Append("?");
                    aspxUrlSb.Append(Total.QueryStringBigClassId);
                    aspxUrlSb.Append("=");
                    aspxUrlSb.Append(id);
                    break;
                case PageType.SMALL_CLASS_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlSmallClass);
                    aspxUrlSb.Append("?");
                    aspxUrlSb.Append(Total.QueryStringSmallClassId);
                    aspxUrlSb.Append("=");
                    aspxUrlSb.Append(id);
                    break;
                case PageType.ARTICLE_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlShowdetail);
                    aspxUrlSb.Append("?");
                    aspxUrlSb.Append(Total.QueryStringArticleId);
                    aspxUrlSb.Append("=");
                    aspxUrlSb.Append(id);
                    break;
                case PageType.DELETED_ARTICLE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlShowdetail);
                    aspxUrlSb.Append("?");
                    aspxUrlSb.Append(Total.QueryStringDeletedArticleId);
                    aspxUrlSb.Append("=");
                    aspxUrlSb.Append(id);
                    break;
                case PageType.BUILDING_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlBuilding);
                    break;
                case PageType.GUESTASK_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlGuestAsk);
                    break;
                case PageType.GUESTBOOK_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlGuestbook);
                    if (!string.IsNullOrEmpty(id))
                    {
                        aspxUrlSb.Append("?");
                        aspxUrlSb.Append(Total.QueryStringGuestbookId);
                        aspxUrlSb.Append("=");
                        aspxUrlSb.Append(id);
                    }
                    break;
                case PageType.WEBSERVICE_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlWebService);
                    aspxUrlSb.Append("?");
                    aspxUrlSb.Append(Total.QueryStringAction);
                    aspxUrlSb.Append("=");
                    aspxUrlSb.Append(id);
                    break;
                case PageType.LINK_ALL_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlLinkAll);
                    break;
                case PageType.LINK_APP_TYPE:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlLinkApp);
                    break;
                case PageType.FRAME_FLZX:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlFrameFlzx);
                    break;
                case PageType.FRAME_TODAYTIPS:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlFrameTodaytips);
                    break;
                case PageType.FRAME_HEADER:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlFrameHeader);
                    break;
                case PageType.FRAME_FOOTER:
                    aspxUrlSb.Append(com.hujun64.Total.AspxUrlFrameFooter);
                    break;

                default:
                    break;
            }

            return aspxUrlSb.ToString();

        }
      
        public static string BuildHrefArticle(ArticleBase article, bool newWindow)
        {
            return BuildHrefArticle(article, newWindow, false);
        }
        public static string BuildHrefArticle(ArticleBase article, bool newWindow, bool isFullUrl)
        {
            if (article == null)
                return "";
            StringBuilder urlSb = new StringBuilder();


            if (isFullUrl)
            {
                urlSb.AppendFormat("{0}/", Total.SiteUrl);
            }
            urlSb.Append(GetAspxUrl(article.id, PageType.DYNAMIC_TYPE));
            urlSb.Replace("//", "/");
            return BuildHref(urlSb.ToString(), RemoveHtmlTag(article.title), RemoveHtmlTag(article.title), newWindow);
        }
        public static string BuildHref(string url, string hrefName, string title)
        {

            return BuildHref(url, hrefName, title, false);
        }
        public static string BuildHref(string url, string hrefName, string title, bool newWindow, string cssClass)
        {
            StringBuilder href = new StringBuilder("<a ");
            if (!string.IsNullOrEmpty(cssClass))
            {
                href.Append(" class=\"");
                href.Append(cssClass);
                href.Append("\"");
            }
            href.Append(" href=\"");
            href.Append(url);
            href.Append("\" title=\"");
            href.Append(RemoveHtmlTag(title));
            href.Append("\"");
            if (newWindow)
            {
                href.Append(" target=\"_blank\"");
            }
            href.Append(">");
            href.Append(hrefName);
            href.Append("</a>");
            return href.ToString();
        }
        public static string BuildHref(string url, string hrefName, string title, bool newWindow)
        {

            return BuildHref(url, hrefName, title, newWindow, null);
        }
        public static string BuildHref(string url)
        {
            StringBuilder href = new StringBuilder("<a href=\"");
            href.Append(url);
            href.Append("\">");
            href.Append(url);
            href.Append("</a>");
            return href.ToString();
        }
        public static string BuildFontzoomHref(string zoomId, FontSizeType fontSize)
        {
            StringBuilder href = new StringBuilder("<a href=\"javascript:fontZoom('");
            href.Append(zoomId);
            href.Append("',");
            href.Append((int)fontSize.FontSize);
            href.Append(")\">");
            href.Append(fontSize.ToString());
            href.Append("</a>");
            return href.ToString();
        }
        public static string BuildFontzoomHref(string[] zoomIdArray, FontSizeType fontSize)
        {
            StringBuilder href = new StringBuilder("<a href=\"javascript:fontZoomArray(new Array(");
            int i = 0;
            foreach (string zoomId in zoomIdArray)
            {
                if (i > 0)
                {
                    href.Append(",");
                }

                href.Append("'");
                href.Append(zoomId);
                href.Append("'");
                i++;
            }
            href.Append("),");
            href.Append((int)fontSize.FontSize);
            href.Append(")\">");
            href.Append(fontSize.ToString());
            href.Append("</a>");
            return href.ToString();
        }
        public static string BuildingLocationHrefModule(string bigClassId, string smallClassId, string moduleClassId)
        {
            StringBuilder locationUrl = new StringBuilder("<a href=\"");
            locationUrl.Append(com.hujun64.Total.AspxUrlModule);
            locationUrl.Append("?");
            locationUrl.Append(Total.QueryStringModuleClassId);
            locationUrl.Append("=");
            locationUrl.Append(moduleClassId);


            IMainClassService mainClassService = ServiceFactory.GetMainClassService();
            string moduleClassName = mainClassService.GetClassById(moduleClassId).class_name;
            if (string.IsNullOrEmpty(bigClassId) && !string.IsNullOrEmpty(smallClassId))
            {
                bigClassId = mainClassService.GetBigClassBySmall(smallClassId).id;
            }

            if (!string.IsNullOrEmpty(bigClassId))
            {
                locationUrl.Append("&");
                locationUrl.Append(Total.QueryStringBigClassId);
                locationUrl.Append("=");
                locationUrl.Append(bigClassId);
            }
            if (!string.IsNullOrEmpty(smallClassId))
            {
                locationUrl.Append("&");
                locationUrl.Append(Total.QueryStringSmallClassId);
                locationUrl.Append("=");
                locationUrl.Append(smallClassId);
            }
            locationUrl.Append("\" title=\"");
            locationUrl.Append(moduleClassName);
            locationUrl.Append("\">");
            locationUrl.Append(moduleClassName);
            locationUrl.Append("</a>");
            return locationUrl.ToString();
        }
        public static string BuildingLocationHref(string className, string classId, PageType pageType)
        {
            StringBuilder locationUrl = new StringBuilder("<a href=\"");
            switch (pageType)
            {
                case PageType.INDEX_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlIndex);
                    break;
                case PageType.BIG_CLASS_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlBigClass);
                    locationUrl.Append("?");
                    locationUrl.Append(Total.QueryStringBigClassId);
                    locationUrl.Append("=");

                    locationUrl.Append(classId);
                    break;

                case PageType.SMALL_CLASS_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlSmallClass);
                    locationUrl.Append("?");
                    locationUrl.Append(Total.QueryStringSmallClassId);
                    locationUrl.Append("=");
                    locationUrl.Append(classId);
                    break;
                case PageType.MODULE_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlModule);
                    locationUrl.Append("?");
                    locationUrl.Append(Total.QueryStringModuleClassId);
                    locationUrl.Append("=");
                    locationUrl.Append(classId);
                    break;
                case PageType.GUESTBOOK_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlGuestbook);
                    break;
                case PageType.INTRO_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlIntro);
                    break;
                case PageType.LINK_ALL_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlLinkAll);
                    break;
                case PageType.LINK_APP_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlLinkApp);
                    break;
                case PageType.FEE_TYPE:
                    locationUrl.Append(com.hujun64.Total.AspxUrlFee);
                    break;

                default:
                    return "";

            }
            locationUrl.Append("\" title=\"");
            locationUrl.Append(className);
            //locationUrl.Append("\" class=\"classLevel\">");
            locationUrl.Append("\">");
            locationUrl.Append(className);
            locationUrl.Append("</a>");
            return locationUrl.ToString();
        }
        public static PageInfo GetPageInfo(string className, PageType pageType)
        {
            PageInfo pageInfo = new PageInfo();

            IMainClassService mainClassService = ServiceFactory.GetMainClassService();

            string classId = mainClassService.GetClassByName(className, Total.SiteId).id;

            StringBuilder myLocation = new StringBuilder(BuildingLocationHref("首页", "", PageType.INDEX_TYPE));

            myLocation.Append(Total.SplitMyLocation);
            myLocation.Append(BuildingLocationHref(className, classId, pageType));
            pageInfo.locationHref = myLocation.ToString();

            if (pageType == PageType.SMALL_CLASS_TYPE)
            {

                string bigClassName = mainClassService.GetBigClassBySmall(classId).class_name;
                pageInfo.title = bigClassName + Total.SplitTitle + className;
            }
            else
            {
                pageInfo.title = className;
            }
            return pageInfo;
        }
        public static PageInfo GetPageInfo(string bigClassId, string smallClassId, string moduleClassId, PageType pageType)
        {
            PageInfo pageInfo = GetPageInfoBigSmallClass(bigClassId, smallClassId);


            IMainClassService mainClassService = ServiceFactory.GetMainClassService();
            string moduleClassName = mainClassService.GetClassById(moduleClassId).class_name;

            StringBuilder myLocation = new StringBuilder(pageInfo.locationHref);
            myLocation.Append(Total.SplitMyLocation);
            myLocation.Append(BuildingLocationHref(moduleClassName, moduleClassId, pageType));

            pageInfo.locationHref = myLocation.ToString();

            if (moduleClassName.Length > 0)
            {
                if (pageInfo.title.Length > 0)
                {
                    pageInfo.title += Total.SplitTitle;
                }
                pageInfo.title += moduleClassName;
            }

            return pageInfo;
        }
        public static PageInfo GetPageInfoBigSmallClass(string bigClassId, string smallClassId)
        {

            IMainClassService mainClassService = ServiceFactory.GetMainClassService();
            StringBuilder myLocation = new StringBuilder(BuildingLocationHref("首页", "", PageType.INDEX_TYPE));


            string smallClassName = mainClassService.GetClassById(smallClassId).class_name;
            string bigClassName = mainClassService.GetClassById(bigClassId).class_name;


            if (!string.IsNullOrEmpty(bigClassName))
            {
                myLocation.Append(Total.SplitMyLocation);
                myLocation.Append(BuildingLocationHref(bigClassName, bigClassId, PageType.BIG_CLASS_TYPE));
            }
            else
            {
                bigClassName = mainClassService.GetBigClassBySmall(smallClassId).class_name;
                if (!string.IsNullOrEmpty(bigClassName))
                {
                    myLocation.Append(Total.SplitMyLocation);
                    myLocation.Append(BuildingLocationHref(bigClassName, bigClassId, PageType.BIG_CLASS_TYPE));
                }
            }
            if (!string.IsNullOrEmpty(smallClassName))
            {
                myLocation.Append(Total.SplitMyLocation);
                myLocation.Append(BuildingLocationHref(smallClassName, smallClassId, PageType.SMALL_CLASS_TYPE));
            }

            StringBuilder titleSb = new StringBuilder();
            if (!string.IsNullOrEmpty(bigClassName))
            {
                titleSb.Append(bigClassName);
            }

            if (!string.IsNullOrEmpty(smallClassName))
            {
                if (titleSb.Length > 0)
                {
                    titleSb.Append(Total.SplitTitle);
                }
                titleSb.Append(smallClassName);
            }
            PageInfo pageInfo = new PageInfo();
            pageInfo.locationHref = myLocation.ToString();
            pageInfo.title = titleSb.ToString();

            return pageInfo;

        }
        public static PageInfo GetPageInfo(Article article)
        {
            return GetPageInfo(article.big_class_id, article.class_id, article.module_class_id);
        }
        public static PageInfo GetPageInfo(string bigClassId, string smallClassId, string moduleClassId)
        {

            PageInfo pageInfo = GetPageInfoBigSmallClass(bigClassId, smallClassId);


            IMainClassService mainClassService = ServiceFactory.GetMainClassService();
            string moduleClassName = mainClassService.GetClassById(moduleClassId).class_name;

            StringBuilder myLocation = new StringBuilder(pageInfo.locationHref);
            if (!string.IsNullOrEmpty(moduleClassName))
            {
                myLocation.Append(Total.SplitMyLocation);
                myLocation.Append(BuildingLocationHrefModule(bigClassId, smallClassId, moduleClassId));
            }

            pageInfo.locationHref = myLocation.ToString();

            if (!string.IsNullOrEmpty(moduleClassName))
            {
                if (pageInfo.title.Length > 0)
                {
                    pageInfo.title += Total.SplitTitle;
                }
                pageInfo.title += moduleClassName;
            }

            return pageInfo;


        }
        public static string GetFullImageUrl(string imgUrl)
        {
            return GetFullImageUrl(imgUrl, null, null, null);

        }
        public static string GetFullImageUrl(string imgUrl, string altString)
        {
            return GetFullImageUrl(imgUrl, altString, null, null);

        }
        public static string GetFullImageUrl(string imgUrl, string altString, string width, string height)
        {
            if (string.IsNullOrEmpty(imgUrl))
                return "";
            else
            {
                StringBuilder imgSb = new StringBuilder();
                imgSb.Append("<img src=\"");
                imgSb.Append(imgUrl);
                imgSb.Append("\" ");
                if (!string.IsNullOrEmpty(altString))
                {
                    imgSb.Append(" alt=\"");
                    imgSb.Append(altString);
                    imgSb.Append("\" ");
                }
                imgSb.Append(" class=\"imgNoborder\"");
                if (!string.IsNullOrEmpty(width))
                {
                    imgSb.Append(" width=\"");
                    imgSb.Append(width);
                    imgSb.Append("\"");
                }
                if (!string.IsNullOrEmpty(height))
                {
                    imgSb.Append(" height=\"");
                    imgSb.Append(height);
                    imgSb.Append("\"");
                }
                imgSb.Append("/>");
                return imgSb.ToString();
            }


        }
        public static string GetImageOfNewArticle(string id, DateTime articleDateTime)
        {
            string newImageUrl = UtilHtml.GetFullImageUrl(Total.ImgNewUrl);
            TimeSpan timespan = DateTime.Now - articleDateTime;


            if (id.StartsWith(Total.PrefixArticleId) && timespan.Days <= Total.ExpiresNewArticle)
            {
                return newImageUrl;
            }
            else if (id.StartsWith(Total.PrefixGuestbookId) && timespan.Days <= Total.ExpiresNewGuestbook)
            {
                return newImageUrl;
            }
            else
            {
                return "";
            }
        }
        public static string GetImageOfNewGuestbook(DateTime guestbookDateTime)
        {
            string newImageUrl = UtilHtml.GetFullImageUrl(Total.ImgNewUrl);
            TimeSpan timespan = DateTime.Now - guestbookDateTime;
            if (timespan.Days <= Total.ExpiresNewGuestbook)
            {
                return newImageUrl;
            }
            else
            {
                return "";
            }
        }
        public static string GetMyStatUrl(string id, PageType pageType)
        {

            if (id == null)
            {
                return Total.AspxUrlStat;
            }
            else
            {
                switch (pageType)
                {
                    case PageType.ARTICLE_TYPE:
                    case PageType.INTRO_TYPE:
                        return Total.AspxUrlStatClick + "?" + Total.QueryStringArticleId + "=" + id;

                    case PageType.GUESTBOOK_TYPE:
                        return Total.AspxUrlStatClick + "?" + Total.QueryStringGuestbookId + "=" + id;
                    default:
                        return Total.AspxUrlStat;
                }
            }
        }
        public static string TitleSubstring(string title, int maxLength)
        {
            if (title == null)
                return "";

            title = RemoveEnterBreak(RemoveHtmlTag(title));
            if (title.Trim().Length >= maxLength)
            {
                return title.Trim().Substring(0, maxLength - 3) + TitlePostfix;
            }
            else
            {
                return title.Trim();
            }
        }
        public static string TitleString(string id, string title,  DateTime articleDateTime)
        {
            return TitleSubstring(id,title,Int16.MaxValue,articleDateTime);
        }
        public static string TitleSubstring(string id, string title, int maxLength, DateTime articleDateTime)
        {
            if (title == null)
                return "";

            title = RemoveHtmlTag(title);
            string imgNewUrl = GetImageOfNewArticle(id, articleDateTime);
            if (string.IsNullOrEmpty(imgNewUrl))
            {
                if (title.Trim().Length > maxLength)
                {
                    return title.Trim().Substring(0, maxLength - 3) + TitlePostfix;
                }
                else
                {
                    return title;
                }
            }
            else
            {
                if (title.Trim().Length >= (maxLength - NEW_IMG_LEN))
                {

                    return title.Trim().Substring(0, maxLength - NEW_IMG_LEN - 3) + TitlePostfix + imgNewUrl;
                }
                else
                {

                    return title.Trim() + imgNewUrl;
                }

            }

        }
        public static string ImgTitleSubstring(string title)
        {

            return TitleSubstring(title, MAX_IMG_TITLE_LEN);
        }
        public static string NoImgTitleSubstring(string title)
        {

            return TitleSubstring(title, MAX_NO_IMG_TITLE_LEN);
        }
        public static string LeftTitleSubstring(string title)
        {

            return TitleSubstring(title, MAX_LEFT_TITLE_LEN);
        }

        public static string ImgTitleSubstring(string id, string title, DateTime articleDateTime)
        {
            string imgNewUrl = GetImageOfNewArticle(id, articleDateTime);
            if (string.IsNullOrEmpty(imgNewUrl))
            {
                return TitleSubstring(title, MAX_IMG_TITLE_LEN);
            }
            else
            {
                return TitleSubstring(title, MAX_IMG_TITLE_LEN - NEW_IMG_LEN) + imgNewUrl;
            }
        }
        public static string NoImgTitleSubstring(string id, string title, DateTime articleDateTime)
        {
            string imgNewUrl = GetImageOfNewArticle(id, articleDateTime);
            if (string.IsNullOrEmpty(imgNewUrl))
            {
                return TitleSubstring(title, MAX_NO_IMG_TITLE_LEN);
            }
            else
            {
                return TitleSubstring(title, MAX_NO_IMG_TITLE_LEN - NEW_IMG_LEN) + imgNewUrl;
            }

        }
        public static string LeftTitleSubstring(string id, string title, DateTime articleDateTime)
        {
            string imgNewUrl = GetImageOfNewArticle(id, articleDateTime);
            if (string.IsNullOrEmpty(imgNewUrl))
            {
                return TitleSubstring(title, MAX_LEFT_TITLE_LEN);
            }
            else
            {
                return TitleSubstring(title, MAX_LEFT_TITLE_LEN - NEW_IMG_LEN) + imgNewUrl;
            }

        }
        public static string LinknameSubstring(string title, int maxLength)
        {
            if (title == null)
                return "";

            title = RemoveHtmlTag(title);
            if (title.Trim().Length >= maxLength)
            {
                return title.Trim().Substring(0, maxLength - 1) + ".";
            }
            else
            {
                return title.Trim();
            }
        }
        //public static string GetNewIdUrl(string oldId, PageType pageType)
        //{

        //    if (!string.IsNullOrEmpty(oldId) && oldId.Trim().Length <= Total.IdFormatString.Length)
        //    {

        //        string newId = oldId.Trim();

        //        Regex objRegExp = new Regex(@"^\d+$");

        //        if (objRegExp.IsMatch(oldId.Trim()))
        //        {
        //            switch (pageType)
        //            {
        //                case PageType.ARTICLE_TYPE:
        //                case PageType.INTRO_TYPE:
        //                    newId = Total.PrefixArticleId + Convert.ToInt32(oldId).ToString(Total.IdFormatString);
        //                    break;
        //                case PageType.GUESTBOOK_TYPE:
        //                    newId = Total.PrefixGuestbookId + Convert.ToInt32(oldId).ToString(Total.IdFormatString);
        //                    break;
        //                default:
        //                    return Total.SiteUrl;
        //            }
        //            return ConvertAspxWithHtml(GetAspxUrl(newId, pageType));
        //        }
        //        else
        //        {
        //            return Total.SiteUrl;
        //        }

        //    }
        //    else
        //    {
        //        return Total.SiteUrl;
        //    }
        //}
        public static string BuildIncludeUrl(string url)
        {
            return "<!--#include file=\"" + url + "\" -->";
        }
        public static string GetFrameHtmlUrl(PageType pageType, string bigClassId, string smallClassId)
        {

            StringBuilder aspxUrlSb = new StringBuilder(GetAspxUrl(pageType));


            if (!string.IsNullOrEmpty(bigClassId))
            {
                aspxUrlSb.Append("?");
                aspxUrlSb.Append(Total.QueryStringBigClassId);
                aspxUrlSb.Append("=");
                aspxUrlSb.Append(bigClassId);
            }

            if (!string.IsNullOrEmpty(smallClassId))
            {
                aspxUrlSb.Append("&");
                aspxUrlSb.Append(Total.QueryStringSmallClassId);
                aspxUrlSb.Append("=");
                aspxUrlSb.Append(smallClassId);
            }

            StringBuilder retUrlSb = new StringBuilder();
            retUrlSb.Append(Total.HtmlPath);
            retUrlSb.Append("/");
            retUrlSb.Append(ReplaceAspxWithHtml(aspxUrlSb.ToString()));

            while (retUrlSb.ToString().Contains("//"))
                retUrlSb.Replace("//", "/");

            if (retUrlSb.ToString().StartsWith("/"))
                retUrlSb.Remove(0, 1);

            if (Total.IsFullSiteurl)
                retUrlSb.Insert(0, Total.CurrentSiteRootUrl);
            else
                retUrlSb.Insert(0, "/");

            return retUrlSb.ToString();

        }
    }
}
