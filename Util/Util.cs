using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Reflection;
namespace com.hujun64.util
{
    /// <summary>
    ///Util 的摘要说明
    /// </summary>
    public class Util
    {
       
        
        private Util()
        {
        }
        
       
      
         /// <summary>
        /// 对象属性拷贝(全匹配拷贝)
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="des">目标对象</param>
        /// <returns>目标对象</returns>
        public static T PropertyCopy<K, T>(K src, ref T des)
        {
            Type souType = src.GetType();
            Type tarType = des.GetType();
            PropertyInfo[] pis = souType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (null != pis)
            {
                foreach (PropertyInfo pi in pis)
                {
                    string propertyName = pi.Name;
                    PropertyInfo pit = tarType.GetProperty(propertyName);
                    if (pit != null)
                    {
                        pit.SetValue(des, pi.GetValue(src, null), null);
                    }
                }
            }
            return (T)des;
        } 
        
        
        public static void ValidateCode(string validateNum, HttpResponse response)
        {
            int Gheight = (int)(validateNum.Length * 11.5);
            //gheight为图片宽度,根据字符长度自动更改图片宽度
            System.Drawing.Bitmap Img = new System.Drawing.Bitmap(Gheight, 20);
            Graphics g = Graphics.FromImage(Img);
            g.DrawString(validateNum, new System.Drawing.Font("Arial", 10), new System.Drawing.SolidBrush(Color.Red), 3, 3);
            //在矩形内绘制字串（字串，字体，画笔颜色，左上x.左上y） 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            response.ClearContent(); //需要输出图象信息 要修改HTTP头 
            response.ContentType = "image/Png";
            response.BinaryWrite(ms.ToArray());
            g.Dispose();
            Img.Dispose();
            response.End();
        }

        private static char[] constant =
		{
			'0','1','2','3','4','5','6','7','8','9'
            //,
			//'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
			//'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
			
		};
        public static string GenerateRandom(int length)
        {
            if (length <= 0)
                return null;

            int constant_len = constant.Length;
            StringBuilder newRandom = new StringBuilder(length);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(constant_len)]);
            }
            return newRandom.ToString();
        }

       
       
        
        public static TimeSpan DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
           
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan dateDiff = ts1.Subtract(ts2).Duration();
            //dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }     
        
    }


}
