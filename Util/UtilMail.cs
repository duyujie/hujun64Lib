using System;
using System.Text;
using System.Net.Mail;
namespace com.hujun64.util
{
    /// <summary>
    ///MailSender ��ժҪ˵��
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
            log.Info("�첽�����ʼ�" + receiverEmailAddress);
            try
            {

                MyDelegate myDelegate = new MyDelegate(SendMail);
                return myDelegate.BeginInvoke(subject, content, receiverEmailAddress,ccEmailArray, null, null);


            }
            catch (Exception ex)
            {
                log.Error("�첽�����ʼ�����(" + receiverEmailAddress+")", ex);
                throw ex;
            }

        }

        //ǩ��
        private static string Sign()
        {
            StringBuilder signSb = new StringBuilder();

            signSb.AppendFormat("{0} {1}<br />", Total.Author, UtilHtml.BuildHref(Total.SiteUrl));
            signSb.AppendFormat("�绰��{0}<br />", Total.Mobile.Replace(" ", ""));
            signSb.AppendFormat("���棺{0}<br />", Total.Fax.Replace(" ", ""));
            signSb.AppendFormat("������ַ��{0} ({1})<br />", Total.AddressWorkday,Total.NearbyWorkday);
           

            return signSb.ToString();

        }
        public static void SendMail(string subject, string content, string receiverEmailAddress, string[] ccEmailArray)
        {
            log.Info("�����ʼ�" + receiverEmailAddress);
            //���ͷ���ַ(��test@163.com)
            string from = Total.AdminMail;
            //���շ���ַ(��test@163.com)
            string to;
            if (string.IsNullOrEmpty(receiverEmailAddress))
                to = Total.AdminMail;
            else
                to = receiverEmailAddress;

            
            //����        
            StringBuilder bodySb = new StringBuilder("<html><body><br />");
            bodySb.Append(content);


            
            bodySb.Append("<br /><br /><br />");
            //ǩ��
            bodySb.Append(Sign());

            bodySb.Append("</body></html>");



            //SmtpClient�Ƿ����ʼ������壬������캯���Ǹ�֪SmtpClient�����ʼ�ʱʹ���ĸ�SMTP������
            System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient(Total.SmtpServerHost, Total.SmtpServerPort);
            mailClient.EnableSsl = Total.SmtpServerEnableSSL;
            //����һ����֤ʵ��
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(Total.AdminMailUser, Total.AdminMailPasswd);
            //����֤ʵ������mailClient
            mailClient.Credentials = nc;
            //ǧ��Ҫ�ٻ��������ڡ�mailClient.Credentials = nc;��������ٶ�mailclient.UseDefaultCredentials��ֵ�ˣ�������false����true���������³������г���


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

                    // ���յķ��ͷ���
                    mailClient.Send(message);

                    log.Info("�ʼ�����"+to +"�ɹ���");
                }
            }
            catch (Exception ex)
            {
                log.Error("�ʼ�����"+to +"ʧ�ܣ�", ex);
            }
        }



       
    }
}