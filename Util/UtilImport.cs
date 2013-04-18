using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using com.hujun64.po;
namespace com.hujun64.util
{
    /// <summary>
    ///CGW 的摘要说明
    /// </summary>
    public class UtilImport
    {
        private UtilImport()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static List<Client> importClient(string csvFilename){

            string allContentCsv = UtilFile.ReadTextFile(csvFilename, Total.EncodingDefault).Replace("\r", "");

            string[] lineContentArray = allContentCsv.Split('\n');

            Regex regExp = new Regex(@"^1[0-9]{10}$");
            List<Client> clientList=new List<Client>();
            foreach (string lineContent in lineContentArray)
            {
                string[] clientInfoArray = lineContent.Split(',');

                if (clientInfoArray.Length > 1 && regExp.IsMatch(clientInfoArray[1].Trim()))
                {
                    Client client = new Client(clientInfoArray[0].Trim(), clientInfoArray[1].Trim());
                    clientList.Add(client);
                }
            }
            return clientList;
        }
        public static List<string> importCGW(string txtFilename)
        {
            string allContentTxt = UtilFile.ReadTextFile(txtFilename, Total.EncodingDefault).Replace("\r", "");

            string[] lineContentArray = allContentTxt.Split('\n');
           // Regex regExp = new Regex(@"\w");

            List<string> wordsList = new List<string>(lineContentArray.Length);
            StringBuilder sb=new StringBuilder();
            foreach (string word in lineContentArray)
            {
                wordsList.Add(word);
            }
            return wordsList;

        }
    }
}