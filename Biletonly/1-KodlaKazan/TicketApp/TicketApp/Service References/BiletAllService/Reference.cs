﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace TicketApp.BiletAllService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BiletAllService.ServiceSoap")]
    public interface ServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/XmlIslet", ReplyAction="*")]
        System.IAsyncResult BeginXmlIslet(TicketApp.BiletAllService.XmlIsletRequest request, System.AsyncCallback callback, object asyncState);
        
        TicketApp.BiletAllService.XmlIsletResponse EndXmlIslet(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/StrIslet", ReplyAction="*")]
        System.IAsyncResult BeginStrIslet(TicketApp.BiletAllService.StrIsletRequest request, System.AsyncCallback callback, object asyncState);
        
        TicketApp.BiletAllService.StrIsletResponse EndStrIslet(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class XmlIsletRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="XmlIslet", Namespace="http://tempuri.org/", Order=0)]
        public TicketApp.BiletAllService.XmlIsletRequestBody Body;
        
        public XmlIsletRequest() {
        }
        
        public XmlIsletRequest(TicketApp.BiletAllService.XmlIsletRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class XmlIsletRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement xmlIslem;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public System.Xml.Linq.XElement xmlYetki;
        
        public XmlIsletRequestBody() {
        }
        
        public XmlIsletRequestBody(System.Xml.Linq.XElement xmlIslem, System.Xml.Linq.XElement xmlYetki) {
            this.xmlIslem = xmlIslem;
            this.xmlYetki = xmlYetki;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class XmlIsletResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="XmlIsletResponse", Namespace="http://tempuri.org/", Order=0)]
        public TicketApp.BiletAllService.XmlIsletResponseBody Body;
        
        public XmlIsletResponse() {
        }
        
        public XmlIsletResponse(TicketApp.BiletAllService.XmlIsletResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class XmlIsletResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement XmlIsletResult;
        
        public XmlIsletResponseBody() {
        }
        
        public XmlIsletResponseBody(System.Xml.Linq.XElement XmlIsletResult) {
            this.XmlIsletResult = XmlIsletResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class StrIsletRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="StrIslet", Namespace="http://tempuri.org/", Order=0)]
        public TicketApp.BiletAllService.StrIsletRequestBody Body;
        
        public StrIsletRequest() {
        }
        
        public StrIsletRequest(TicketApp.BiletAllService.StrIsletRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class StrIsletRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strislem;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string stryetki;
        
        public StrIsletRequestBody() {
        }
        
        public StrIsletRequestBody(string strislem, string stryetki) {
            this.strislem = strislem;
            this.stryetki = stryetki;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class StrIsletResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="StrIsletResponse", Namespace="http://tempuri.org/", Order=0)]
        public TicketApp.BiletAllService.StrIsletResponseBody Body;
        
        public StrIsletResponse() {
        }
        
        public StrIsletResponse(TicketApp.BiletAllService.StrIsletResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class StrIsletResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string StrIsletResult;
        
        public StrIsletResponseBody() {
        }
        
        public StrIsletResponseBody(string StrIsletResult) {
            this.StrIsletResult = StrIsletResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceSoapChannel : TicketApp.BiletAllService.ServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class XmlIsletCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public XmlIsletCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Xml.Linq.XElement Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Xml.Linq.XElement)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StrIsletCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public StrIsletCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceSoapClient : System.ServiceModel.ClientBase<TicketApp.BiletAllService.ServiceSoap>, TicketApp.BiletAllService.ServiceSoap {
        
        private BeginOperationDelegate onBeginXmlIsletDelegate;
        
        private EndOperationDelegate onEndXmlIsletDelegate;
        
        private System.Threading.SendOrPostCallback onXmlIsletCompletedDelegate;
        
        private BeginOperationDelegate onBeginStrIsletDelegate;
        
        private EndOperationDelegate onEndStrIsletDelegate;
        
        private System.Threading.SendOrPostCallback onStrIsletCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ServiceSoapClient() {
        }
        
        public ServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<XmlIsletCompletedEventArgs> XmlIsletCompleted;
        
        public event System.EventHandler<StrIsletCompletedEventArgs> StrIsletCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult TicketApp.BiletAllService.ServiceSoap.BeginXmlIslet(TicketApp.BiletAllService.XmlIsletRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginXmlIslet(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginXmlIslet(System.Xml.Linq.XElement xmlIslem, System.Xml.Linq.XElement xmlYetki, System.AsyncCallback callback, object asyncState) {
            TicketApp.BiletAllService.XmlIsletRequest inValue = new TicketApp.BiletAllService.XmlIsletRequest();
            inValue.Body = new TicketApp.BiletAllService.XmlIsletRequestBody();
            inValue.Body.xmlIslem = xmlIslem;
            inValue.Body.xmlYetki = xmlYetki;
            return ((TicketApp.BiletAllService.ServiceSoap)(this)).BeginXmlIslet(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TicketApp.BiletAllService.XmlIsletResponse TicketApp.BiletAllService.ServiceSoap.EndXmlIslet(System.IAsyncResult result) {
            return base.Channel.EndXmlIslet(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.Xml.Linq.XElement EndXmlIslet(System.IAsyncResult result) {
            TicketApp.BiletAllService.XmlIsletResponse retVal = ((TicketApp.BiletAllService.ServiceSoap)(this)).EndXmlIslet(result);
            return retVal.Body.XmlIsletResult;
        }
        
        private System.IAsyncResult OnBeginXmlIslet(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.Xml.Linq.XElement xmlIslem = ((System.Xml.Linq.XElement)(inValues[0]));
            System.Xml.Linq.XElement xmlYetki = ((System.Xml.Linq.XElement)(inValues[1]));
            return this.BeginXmlIslet(xmlIslem, xmlYetki, callback, asyncState);
        }
        
        private object[] OnEndXmlIslet(System.IAsyncResult result) {
            System.Xml.Linq.XElement retVal = this.EndXmlIslet(result);
            return new object[] {
                    retVal};
        }
        
        private void OnXmlIsletCompleted(object state) {
            if ((this.XmlIsletCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.XmlIsletCompleted(this, new XmlIsletCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void XmlIsletAsync(System.Xml.Linq.XElement xmlIslem, System.Xml.Linq.XElement xmlYetki) {
            this.XmlIsletAsync(xmlIslem, xmlYetki, null);
        }
        
        public void XmlIsletAsync(System.Xml.Linq.XElement xmlIslem, System.Xml.Linq.XElement xmlYetki, object userState) {
            if ((this.onBeginXmlIsletDelegate == null)) {
                this.onBeginXmlIsletDelegate = new BeginOperationDelegate(this.OnBeginXmlIslet);
            }
            if ((this.onEndXmlIsletDelegate == null)) {
                this.onEndXmlIsletDelegate = new EndOperationDelegate(this.OnEndXmlIslet);
            }
            if ((this.onXmlIsletCompletedDelegate == null)) {
                this.onXmlIsletCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnXmlIsletCompleted);
            }
            base.InvokeAsync(this.onBeginXmlIsletDelegate, new object[] {
                        xmlIslem,
                        xmlYetki}, this.onEndXmlIsletDelegate, this.onXmlIsletCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult TicketApp.BiletAllService.ServiceSoap.BeginStrIslet(TicketApp.BiletAllService.StrIsletRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginStrIslet(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginStrIslet(string strislem, string stryetki, System.AsyncCallback callback, object asyncState) {
            TicketApp.BiletAllService.StrIsletRequest inValue = new TicketApp.BiletAllService.StrIsletRequest();
            inValue.Body = new TicketApp.BiletAllService.StrIsletRequestBody();
            inValue.Body.strislem = strislem;
            inValue.Body.stryetki = stryetki;
            return ((TicketApp.BiletAllService.ServiceSoap)(this)).BeginStrIslet(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TicketApp.BiletAllService.StrIsletResponse TicketApp.BiletAllService.ServiceSoap.EndStrIslet(System.IAsyncResult result) {
            return base.Channel.EndStrIslet(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private string EndStrIslet(System.IAsyncResult result) {
            TicketApp.BiletAllService.StrIsletResponse retVal = ((TicketApp.BiletAllService.ServiceSoap)(this)).EndStrIslet(result);
            return retVal.Body.StrIsletResult;
        }
        
        private System.IAsyncResult OnBeginStrIslet(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string strislem = ((string)(inValues[0]));
            string stryetki = ((string)(inValues[1]));
            return this.BeginStrIslet(strislem, stryetki, callback, asyncState);
        }
        
        private object[] OnEndStrIslet(System.IAsyncResult result) {
            string retVal = this.EndStrIslet(result);
            return new object[] {
                    retVal};
        }
        
        private void OnStrIsletCompleted(object state) {
            if ((this.StrIsletCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.StrIsletCompleted(this, new StrIsletCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void StrIsletAsync(string strislem, string stryetki) {
            this.StrIsletAsync(strislem, stryetki, null);
        }
        
        public void StrIsletAsync(string strislem, string stryetki, object userState) {
            if ((this.onBeginStrIsletDelegate == null)) {
                this.onBeginStrIsletDelegate = new BeginOperationDelegate(this.OnBeginStrIslet);
            }
            if ((this.onEndStrIsletDelegate == null)) {
                this.onEndStrIsletDelegate = new EndOperationDelegate(this.OnEndStrIslet);
            }
            if ((this.onStrIsletCompletedDelegate == null)) {
                this.onStrIsletCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnStrIsletCompleted);
            }
            base.InvokeAsync(this.onBeginStrIsletDelegate, new object[] {
                        strislem,
                        stryetki}, this.onEndStrIsletDelegate, this.onStrIsletCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override TicketApp.BiletAllService.ServiceSoap CreateChannel() {
            return new ServiceSoapClientChannel(this);
        }
        
        private class ServiceSoapClientChannel : ChannelBase<TicketApp.BiletAllService.ServiceSoap>, TicketApp.BiletAllService.ServiceSoap {
            
            public ServiceSoapClientChannel(System.ServiceModel.ClientBase<TicketApp.BiletAllService.ServiceSoap> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginXmlIslet(TicketApp.BiletAllService.XmlIsletRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("XmlIslet", _args, callback, asyncState);
                return _result;
            }
            
            public TicketApp.BiletAllService.XmlIsletResponse EndXmlIslet(System.IAsyncResult result) {
                object[] _args = new object[0];
                TicketApp.BiletAllService.XmlIsletResponse _result = ((TicketApp.BiletAllService.XmlIsletResponse)(base.EndInvoke("XmlIslet", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginStrIslet(TicketApp.BiletAllService.StrIsletRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("StrIslet", _args, callback, asyncState);
                return _result;
            }
            
            public TicketApp.BiletAllService.StrIsletResponse EndStrIslet(System.IAsyncResult result) {
                object[] _args = new object[0];
                TicketApp.BiletAllService.StrIsletResponse _result = ((TicketApp.BiletAllService.StrIsletResponse)(base.EndInvoke("StrIslet", _args, result)));
                return _result;
            }
        }
    }
}
