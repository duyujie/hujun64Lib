using System;

namespace com.hujun64.admin
{
    /// <summary>
    /// AdminPageBase 的摘要说明。
    /// </summary>
    public class AdminPageBase : System.Web.UI.Page
    {
        
        protected void PageBase_Error(object sender, System.EventArgs e)
        {
            string errMsg;
            //得到系统上一个异常
            Exception currentError = Server.GetLastError();

            log4net.ILog log = log4net.LogManager.GetLogger(this.ToString());
            log.Error("页面异常：", currentError);


            errMsg = "<h1>页面错误</h1><hr/>此页面发现一个意外错误，对此我们非常抱歉。" +
                "此错误消息已发送给系统管理员，请及时联系我们，我们会及时解决此问题！ <br/>" +
                "错误发生位置： " + Request.Url.ToString() + "<br/>" +
                "错误消息： <font class=\"ErrorMessage\">" + currentError.Message.ToString() + "</font><hr/>" +
                "<b>Stack Trace:</b><br/>" +
                currentError.ToString();

            Session["err"] = errMsg;

            //在页面中显示错误
            Response.Write(errMsg);


            //清除异常
            Server.ClearError();
        }


        private void PageBase_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {

                if (Request.Cookies["uname"] != null && Request.Cookies["upass"] != null)//如果存在 cookies
                {

                    string username = Request.Cookies["uname"].Value, userpass = Request.Cookies["upass"].Value;
                    logic.IAdminService adminService = logic.ServiceFactory.GetAdminService();
                    if (!adminService.Islogin(username, userpass))
                    {
                        Response.Cookies["uname"].Value = null;                        

                        Response.Write("<script>javascript:alert('登录超时！！！');window.parent.location.href=('admin.aspx');</script>");
                    }
                }
                else
                {
                    Response.Write("<script>javascript:alert('登录超时！！！');window.parent.location.href=('admin.aspx');</script>");
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
