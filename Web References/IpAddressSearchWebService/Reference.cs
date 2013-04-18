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

namespace SharedWebClassLibrary.IpAddressSearchWebService {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="IpAddressSearchWebServiceSoap", Namespace="http://WebXml.com.cn/")]
    public partial class IpAddressSearchWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getCountryCityByIpOperationCompleted;
        
        private System.Threading.SendOrPostCallback getGeoIPContextOperationCompleted;
        
        private System.Threading.SendOrPostCallback getVersionTimeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public IpAddressSearchWebService() {
            this.Url = global::SharedWebClassLibrary.Properties.Settings.Default.SharedWebClassLibrary_IpAddressSearchWebService_IpAddressSearchWebService;
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
        public event getCountryCityByIpCompletedEventHandler getCountryCityByIpCompleted;
        
        /// <remarks/>
        public event getGeoIPContextCompletedEventHandler getGeoIPContextCompleted;
        
        /// <remarks/>
        public event getVersionTimeCompletedEventHandler getVersionTimeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://WebXml.com.cn/getCountryCityByIp", RequestNamespace="http://WebXml.com.cn/", ResponseNamespace="http://WebXml.com.cn/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] getCountryCityByIp(string theIpAddress) {
            object[] results = this.Invoke("getCountryCityByIp", new object[] {
                        theIpAddress});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getCountryCityByIpAsync(string theIpAddress) {
            this.getCountryCityByIpAsync(theIpAddress, null);
        }
        
        /// <remarks/>
        public void getCountryCityByIpAsync(string theIpAddress, object userState) {
            if ((this.getCountryCityByIpOperationCompleted == null)) {
                this.getCountryCityByIpOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCountryCityByIpOperationCompleted);
            }
            this.InvokeAsync("getCountryCityByIp", new object[] {
                        theIpAddress}, this.getCountryCityByIpOperationCompleted, userState);
        }
        
        private void OngetCountryCityByIpOperationCompleted(object arg) {
            if ((this.getCountryCityByIpCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCountryCityByIpCompleted(this, new getCountryCityByIpCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://WebXml.com.cn/getGeoIPContext", RequestNamespace="http://WebXml.com.cn/", ResponseNamespace="http://WebXml.com.cn/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] getGeoIPContext() {
            object[] results = this.Invoke("getGeoIPContext", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getGeoIPContextAsync() {
            this.getGeoIPContextAsync(null);
        }
        
        /// <remarks/>
        public void getGeoIPContextAsync(object userState) {
            if ((this.getGeoIPContextOperationCompleted == null)) {
                this.getGeoIPContextOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetGeoIPContextOperationCompleted);
            }
            this.InvokeAsync("getGeoIPContext", new object[0], this.getGeoIPContextOperationCompleted, userState);
        }
        
        private void OngetGeoIPContextOperationCompleted(object arg) {
            if ((this.getGeoIPContextCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getGeoIPContextCompleted(this, new getGeoIPContextCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://WebXml.com.cn/getVersionTime", RequestNamespace="http://WebXml.com.cn/", ResponseNamespace="http://WebXml.com.cn/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string getVersionTime() {
            object[] results = this.Invoke("getVersionTime", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getVersionTimeAsync() {
            this.getVersionTimeAsync(null);
        }
        
        /// <remarks/>
        public void getVersionTimeAsync(object userState) {
            if ((this.getVersionTimeOperationCompleted == null)) {
                this.getVersionTimeOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetVersionTimeOperationCompleted);
            }
            this.InvokeAsync("getVersionTime", new object[0], this.getVersionTimeOperationCompleted, userState);
        }
        
        private void OngetVersionTimeOperationCompleted(object arg) {
            if ((this.getVersionTimeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getVersionTimeCompleted(this, new getVersionTimeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void getCountryCityByIpCompletedEventHandler(object sender, getCountryCityByIpCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCountryCityByIpCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getCountryCityByIpCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void getGeoIPContextCompletedEventHandler(object sender, getGeoIPContextCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getGeoIPContextCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getGeoIPContextCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void getVersionTimeCompletedEventHandler(object sender, getVersionTimeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getVersionTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getVersionTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591