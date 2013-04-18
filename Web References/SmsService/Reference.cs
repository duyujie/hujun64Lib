﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4927
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 2.0.50727.4927 版自动生成。
// 
#pragma warning disable 1591

namespace SharedWebClassLibrary.SmsService {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SmsServiceSoap", Namespace="http://www.m1860.com/service/")]
    public partial class SmsService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback queryOperationCompleted;
        
        private System.Threading.SendOrPostCallback submitmsgOperationCompleted;
        
        private System.Threading.SendOrPostCallback changepwOperationCompleted;
        
        private System.Threading.SendOrPostCallback qrymsgcountOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SmsService() {
            this.Url = global::SharedWebClassLibrary.Properties.Settings.Default.SharedWebClassLibrary_SmsService_SmsService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event queryCompletedEventHandler queryCompleted;
        
        /// <remarks/>
        public event submitmsgCompletedEventHandler submitmsgCompleted;
        
        /// <remarks/>
        public event changepwCompletedEventHandler changepwCompleted;
        
        /// <remarks/>
        public event qrymsgcountCompletedEventHandler qrymsgcountCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.m1860.com/service/query", RequestNamespace="http://www.m1860.com/service/", ResponseNamespace="http://www.m1860.com/service/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int query(string name, string hashedpass, string timestamp) {
            object[] results = this.Invoke("query", new object[] {
                        name,
                        hashedpass,
                        timestamp});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void queryAsync(string name, string hashedpass, string timestamp) {
            this.queryAsync(name, hashedpass, timestamp, null);
        }
        
        /// <remarks/>
        public void queryAsync(string name, string hashedpass, string timestamp, object userState) {
            if ((this.queryOperationCompleted == null)) {
                this.queryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnqueryOperationCompleted);
            }
            this.InvokeAsync("query", new object[] {
                        name,
                        hashedpass,
                        timestamp}, this.queryOperationCompleted, userState);
        }
        
        private void OnqueryOperationCompleted(object arg) {
            if ((this.queryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.queryCompleted(this, new queryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.m1860.com/service/submitmsg", RequestNamespace="http://www.m1860.com/service/", ResponseNamespace="http://www.m1860.com/service/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int submitmsg(string name, string hashedpass, string timestamp, string subphone, string msgsub) {
            object[] results = this.Invoke("submitmsg", new object[] {
                        name,
                        hashedpass,
                        timestamp,
                        subphone,
                        msgsub});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void submitmsgAsync(string name, string hashedpass, string timestamp, string subphone, string msgsub) {
            this.submitmsgAsync(name, hashedpass, timestamp, subphone, msgsub, null);
        }
        
        /// <remarks/>
        public void submitmsgAsync(string name, string hashedpass, string timestamp, string subphone, string msgsub, object userState) {
            if ((this.submitmsgOperationCompleted == null)) {
                this.submitmsgOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsubmitmsgOperationCompleted);
            }
            this.InvokeAsync("submitmsg", new object[] {
                        name,
                        hashedpass,
                        timestamp,
                        subphone,
                        msgsub}, this.submitmsgOperationCompleted, userState);
        }
        
        private void OnsubmitmsgOperationCompleted(object arg) {
            if ((this.submitmsgCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.submitmsgCompleted(this, new submitmsgCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.m1860.com/service/changepw", RequestNamespace="http://www.m1860.com/service/", ResponseNamespace="http://www.m1860.com/service/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int changepw(string name, string hashedoldpass, string timestamp, string newpass) {
            object[] results = this.Invoke("changepw", new object[] {
                        name,
                        hashedoldpass,
                        timestamp,
                        newpass});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void changepwAsync(string name, string hashedoldpass, string timestamp, string newpass) {
            this.changepwAsync(name, hashedoldpass, timestamp, newpass, null);
        }
        
        /// <remarks/>
        public void changepwAsync(string name, string hashedoldpass, string timestamp, string newpass, object userState) {
            if ((this.changepwOperationCompleted == null)) {
                this.changepwOperationCompleted = new System.Threading.SendOrPostCallback(this.OnchangepwOperationCompleted);
            }
            this.InvokeAsync("changepw", new object[] {
                        name,
                        hashedoldpass,
                        timestamp,
                        newpass}, this.changepwOperationCompleted, userState);
        }
        
        private void OnchangepwOperationCompleted(object arg) {
            if ((this.changepwCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.changepwCompleted(this, new changepwCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.m1860.com/service/qrymsgcount", RequestNamespace="http://www.m1860.com/service/", ResponseNamespace="http://www.m1860.com/service/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int qrymsgcount(string name) {
            object[] results = this.Invoke("qrymsgcount", new object[] {
                        name});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void qrymsgcountAsync(string name) {
            this.qrymsgcountAsync(name, null);
        }
        
        /// <remarks/>
        public void qrymsgcountAsync(string name, object userState) {
            if ((this.qrymsgcountOperationCompleted == null)) {
                this.qrymsgcountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnqrymsgcountOperationCompleted);
            }
            this.InvokeAsync("qrymsgcount", new object[] {
                        name}, this.qrymsgcountOperationCompleted, userState);
        }
        
        private void OnqrymsgcountOperationCompleted(object arg) {
            if ((this.qrymsgcountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.qrymsgcountCompleted(this, new qrymsgcountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void queryCompletedEventHandler(object sender, queryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class queryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal queryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void submitmsgCompletedEventHandler(object sender, submitmsgCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class submitmsgCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal submitmsgCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void changepwCompletedEventHandler(object sender, changepwCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class changepwCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal changepwCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void qrymsgcountCompletedEventHandler(object sender, qrymsgcountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class qrymsgcountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal qrymsgcountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591