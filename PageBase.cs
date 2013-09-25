using System;

namespace com.hujun64
{
    /// <summary>
    /// PageBase 的摘要说明。
    /// </summary>
    public class PageBase : System.Web.UI.Page
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
           
            
            Response.Redirect(com.hujun64.Total.ErrorPage);

            //清除异常
            Server.ClearError();
        }


        private void PageBase_Load(object sender, System.EventArgs e)
        {

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Error += new System.EventHandler(this.PageBase_Error);
            base.Load += new EventHandler(this.PageBase_Load);
        }

    }
}
