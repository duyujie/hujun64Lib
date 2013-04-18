using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using O2S.Components.PDFRender4NET;

namespace com.hujun64.util
{
    /// <summary>
    ///Util 的摘要说明
    /// </summary>
    public class UtilFile
    {



        private UtilFile()
        {
        }

        //读二进制filename到byte[]
        public static byte[] ReadBinaryFile(string fileName)
        {

            FileStream pFileStream = null;

            byte[] pReadByte = new byte[0];

            try
            {

                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                BinaryReader r = new BinaryReader(pFileStream);


                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

                pReadByte = r.ReadBytes((int)r.BaseStream.Length);

                return pReadByte;

            }
            finally
            {

                if (pFileStream != null)
                    pFileStream.Close();

            }

        }
        //写二进制byte[]到fileName

        public static bool WriteBinaryFile(byte[] pReadByte, string fileName)
        {

            FileStream pFileStream = null;


            try
            {

                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);


            }
            finally
            {

                if (pFileStream != null)

                    pFileStream.Close();

            }

            return true;

        }

        //读文本filename到string
        public static string ReadTextFile(string fileName)
        {

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(fileName);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                return reader.ReadToEnd();
            }
            finally
            {

                if (reader != null)
                    reader.Close();

            }

        }

        public static string ReadTextFile(string fileName, Encoding encoding)
        {

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(fileName, encoding);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                return reader.ReadToEnd();
            }
            finally
            {

                if (reader != null)
                    reader.Close();

            }

        }

        //写string到fileName

        public static void WriteStringFile(string s, string fileName)
        {


            StreamWriter writer = null;

            try
            {

                writer = new StreamWriter(fileName);
                writer.Write(s);


            }
            finally
            {

                if (writer != null)

                    writer.Close();

            }


        }

        public static void WriteStringFile(string s, string fileName, bool append, Encoding encoding)
        {


            StreamWriter writer = null;

            try
            {

                writer = new StreamWriter(fileName, append, encoding);
                writer.Write(s);


            }
            finally
            {

                if (writer != null)

                    writer.Close();

            }


        }

        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }

        /// <summary>
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出路径</param>
        /// <param name="imageName">生成图片的名字</param>
        /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
        /// <param name="imageFormat">设置所需图片格式</param>
        /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
        public static IList<string> ConvertPDF2Image(string pdfInputPath, string imageOutputPath,
            string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition)
        {
            IList<string> imgFileList = new List<string>();
            PDFFile pdfFile = PDFFile.Open(pdfInputPath);

            if (!Directory.Exists(imageOutputPath))
            {
                Directory.CreateDirectory(imageOutputPath);
            }

            // validate pageNum
            if (startPageNum <= 0)
            {
                startPageNum = 1;
            }

            if (endPageNum > pdfFile.PageCount)
            {
                endPageNum = pdfFile.PageCount;
            }

            if (startPageNum > endPageNum)
            {
                int tempPageNum = startPageNum;
                startPageNum = endPageNum;
                endPageNum = startPageNum;
            }

            // start to convert each page
            for (int i = startPageNum; i <= endPageNum; i++)
            {
                Bitmap pageImage = pdfFile.GetPageImage(i - 1, 28 * (int)definition);
                string imgFilename=imageOutputPath + imageName +"-"+ i.ToString() + "." + imageFormat.ToString();
                pageImage.Save(imgFilename, imageFormat);
                imgFileList.Add(imgFilename);
                pageImage.Dispose();
            }

            pdfFile.Dispose();

            return imgFileList;
        }

        
    }


}
