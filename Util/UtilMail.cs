using System;
using System.Text;
using System.Net.Mail;
namespace com.hujun64.util
{
    /// <summary>
    ///MailSender 的摘要说明
    /// </summary>
    public class UtilMail
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("UtilMail");
        private UtilMail()
        { 
        }
        
        delegate void MyDelegate(string subject, string content, string receiverEmailAddress, string[] ccEmailArray);
       
        public static IAsyncResult SendMailAsync(string subject, string content, string receiverEmailAddress, string[] ccEmailArray)
        {
            log.Info("异步发送邮件" + receiverEmailAddress);
            try
            {

                MyDelegate myDelegate = new MyDelegate(SendMail);
                return myDelegate.BeginInvoke(subject, content, receiverEmailAddress,ccEmailArray, null, null);


            }
            catch (Exception ex)
            {
                log.Error("异步发送邮件出错(" + receiverEmailAddress+")", ex);
                throw ex;
            }

        }

        //签名
        private static string Sign()
        {
            StringBuilder signSb = new StringBuilder();

            signSb.AppendFormat("{0} {1}<br />", Total.Author, UtilHtml.BuildHref(Total.SiteUrl));
            signSb.AppendFormat("电话：{0}<br />", Total.Mobile.Replace(" ", ""));
            signSb.AppendFormat("传真：{0}<br />", Total.Fax.Replace(" ", ""));
            signSb.AppendFormat("律所地址：{0} ({1})<br />", Total.AddressWorkday,Total.NearbyWorkday);
           

            return signSb.ToString();

        }
        public static void SendMail(string subject, string content, string receiverEmailAddress, string[] ccEmailArray)
        {
            log.Info("发送邮件" + receiverEmailAddress);
            //发送方地址(如test@163.com)
            string from = Total.AdminMail;
            //接收方地址(如test@163.com)
            string to;
            if (string.IsNullOrEmpty(receiverEmailAddress))
                to = Total.AdminMail;
            else
                to = receiverEmailAddress;

            
            //内容        
            StringBuilder bodySb = new StringBuilder("<html><body><br />");
            bodySb.Append(content);


            
            bodySb.Append("<br /><br /><br />");
            //签名
            bodySb.Append(Sign());

            bodySb.Append("</body></html>");



            //SmtpClient是发送邮件的主体，这个构造函数是告知SmtpClient发送邮件时使用哪个SMTP服务器
            System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient(Total.SmtpServerHost, Total.SmtpServerPort);
            mailClient.EnableSsl = Total.SmtpServerEnableSSL;
            //构建一个认证实例
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(Total.AdminMailUser, Total.AdminMailPasswd);
            //将认证实例赋予mailClient
            mailClient.Credentials = nc;
            //千万不要再画蛇添足在“mailClient.Credentials = nc;”语句下再对mailclient.UseDefaultCredentials赋值了，不管是false还是true，都将导致程序运行出错


            try
            {
                using (MailMessage message = new MailMessage(from, to, subject, bodySb.ToString()))
                {
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.IsBodyHtml = true;

                    if (ccEmailArray != null && ccEmailArray.Length > 0)
                    {

                        foreach (string cc in ccEmailArray)
                        {
                            message.CC.Add(cc);
                        }
                    }

                    // 最终的发送方法
                    mailClient.Send(message);

                    log.Info("邮件发送"+to +"成功！");
                }
            }
            catch (Exception ex)
            {
                log.Error("邮件发送"+to +"失败！", ex);
            }
        }



       
    }
}