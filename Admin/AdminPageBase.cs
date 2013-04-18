using System;

namespace com.hujun64.admin
{
    /// <summary>
    /// AdminPageBase ��ժҪ˵����
    /// </summary>
    public class AdminPageBase : System.Web.UI.Page
    {
        
        protected void PageBase_Error(object sender, System.EventArgs e)
        {
            string errMsg;
            //�õ�ϵͳ��һ���쳣
            Exception currentError = Server.GetLastError();

            log4net.ILog log = log4net.LogManager.GetLogger(this.ToString());
            log.Error("ҳ���쳣��", currentError);


            errMsg = "<h1>ҳ�����</h1><hr/>��ҳ�淢��һ��������󣬶Դ����Ƿǳ���Ǹ��" +
                "�˴�����Ϣ�ѷ��͸�ϵͳ����Ա���뼰ʱ��ϵ���ǣ����ǻἰʱ��������⣡ <br/>" +
                "������λ�ã� " + Request.Url.ToString() + "<br/>" +
                "������Ϣ�� <font class=\"ErrorMessage\">" + currentError.Message.ToString() + "</font><hr/>" +
                "<b>Stack Trace:</b><br/>" +
                currentError.ToString();

            Session["err"] = errMsg;

            //��ҳ������ʾ����
            Response.Write(errMsg);


            //����쳣
            Server.ClearError();
        }


        private void PageBase_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {

                if (Request.Cookies["uname"] != null && Request.Cookies["upass"] != null)//������� cookies
                {

                    string username = Request.Cookies["uname"].Value, userpass = Request.Cookies["upass"].Value;
                    logic.IAdminService adminService = logic.ServiceFactory.GetAdminService();
                    if (!adminService.Islogin(username, userpass))
                    {
                        Response.Cookies["uname"].Value = null;                        

                        Response.Write("<script>javascript:alert('��¼��ʱ������');window.parent.location.href=('admin.aspx');</script>");
                    }
                }
                else
                {
                    Response.Write("<script>javascript:alert('��¼��ʱ������');window.parent.location.href=('admin.aspx');</script>");
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Error += new System.EventHandler(this.PageBase_Error);
            base.Load += new EventHandler(this.PageBase_Load);


        }

    }
}
