using System;
using System.Text;
using System.Text.RegularExpressions;
namespace com.hujun64.po
{
    /// <summary>
    ///Banner 的摘要说明
    /// </summary>
    public class ImageUrl
    {
        private int width, height;
        public string imgUrl
        {
            get;
            set;
        }
      
        private void replaceWidth()
        {
            Regex urlRegExp = new Regex(@"width?=""(.*?)+?""", RegexOptions.IgnoreCase);
            if (urlRegExp.IsMatch(imgUrl))
            {
                string matchedUrl = urlRegExp.Match(imgUrl).Value;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"width=""{0}px""", width);

                imgUrl = imgUrl.Replace(matchedUrl, sb.ToString());
            }

        }
        private void replaceHeight()
        {
            Regex urlRegExp = new Regex(@"height?=""(.*?)+?""", RegexOptions.IgnoreCase);
            if (urlRegExp.IsMatch(imgUrl))
            {
                string matchedUrl = urlRegExp.Match(imgUrl).Value;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"height=""{0}px""", height);

                imgUrl = imgUrl.Replace(matchedUrl, sb.ToString());
            }

        }
        public ImageUrl(string url, int width, int height)
        {
            imgUrl = url;
            this.width = width;
            this.height = height;

            replaceWidth();
            replaceHeight();
        }
        public ImageUrl(string url)
        {
            imgUrl = url;
        }
    }
}
