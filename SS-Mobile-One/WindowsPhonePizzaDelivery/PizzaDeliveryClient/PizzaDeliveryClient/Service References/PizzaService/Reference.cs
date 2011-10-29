﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace PizzaDeliveryClient.PizzaService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PizzaService.IPizzaDelivery")]
    public interface IPizzaDelivery {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPizzaDelivery/Ping", ReplyAction="http://tempuri.org/IPizzaDelivery/PingResponse")]
        System.IAsyncResult BeginPing(PizzaDeliveryClient.PizzaService.PingRequest request, System.AsyncCallback callback, object asyncState);
        
        PizzaDeliveryClient.PizzaService.PingResponse EndPing(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Ping", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class PingRequest {
        
        public PingRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PingResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class PingResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string PingResult;
        
        public PingResponse() {
        }
        
        public PingResponse(string PingResult) {
            this.PingResult = PingResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPizzaDeliveryChannel : PizzaDeliveryClient.PizzaService.IPizzaDelivery, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PingCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public PingCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public PizzaDeliveryClient.PizzaService.PingResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((PizzaDeliveryClient.PizzaService.PingResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PizzaDeliveryClient : System.ServiceModel.ClientBase<PizzaDeliveryClient.PizzaService.IPizzaDelivery>, PizzaDeliveryClient.PizzaService.IPizzaDelivery {
        
        private BeginOperationDelegate onBeginPingDelegate;
        
        private EndOperationDelegate onEndPingDelegate;
        
        private System.Threading.SendOrPostCallback onPingCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public PizzaDeliveryClient() {
        }
        
        public PizzaDeliveryClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PizzaDeliveryClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PizzaDeliveryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PizzaDeliveryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<PingCompletedEventArgs> PingCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult PizzaDeliveryClient.PizzaService.IPizzaDelivery.BeginPing(PizzaDeliveryClient.PizzaService.PingRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginPing(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PizzaDeliveryClient.PizzaService.PingResponse PizzaDeliveryClient.PizzaService.IPizzaDelivery.EndPing(System.IAsyncResult result) {
            return base.Channel.EndPing(result);
        }
        
        private System.IAsyncResult OnBeginPing(object[] inValues, System.AsyncCallback callback, object asyncState) {
            PizzaDeliveryClient.PizzaService.PingRequest request = ((PizzaDeliveryClient.PizzaService.PingRequest)(inValues[0]));
            return ((PizzaDeliveryClient.PizzaService.IPizzaDelivery)(this)).BeginPing(request, callback, asyncState);
        }
        
        private object[] OnEndPing(System.IAsyncResult result) {
            PizzaDeliveryClient.PizzaService.PingResponse retVal = ((PizzaDeliveryClient.PizzaService.IPizzaDelivery)(this)).EndPing(result);
            return new object[] {
                    retVal};
        }
        
        private void OnPingCompleted(object state) {
            if ((this.PingCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.PingCompleted(this, new PingCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void PingAsync(PizzaDeliveryClient.PizzaService.PingRequest request) {
            this.PingAsync(request, null);
        }
        
        public void PingAsync(PizzaDeliveryClient.PizzaService.PingRequest request, object userState) {
            if ((this.onBeginPingDelegate == null)) {
                this.onBeginPingDelegate = new BeginOperationDelegate(this.OnBeginPing);
            }
            if ((this.onEndPingDelegate == null)) {
                this.onEndPingDelegate = new EndOperationDelegate(this.OnEndPing);
            }
            if ((this.onPingCompletedDelegate == null)) {
                this.onPingCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnPingCompleted);
            }
            base.InvokeAsync(this.onBeginPingDelegate, new object[] {
                        request}, this.onEndPingDelegate, this.onPingCompletedDelegate, userState);
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
        
        protected override PizzaDeliveryClient.PizzaService.IPizzaDelivery CreateChannel() {
            return new PizzaDeliveryClientChannel(this);
        }
        
        private class PizzaDeliveryClientChannel : ChannelBase<PizzaDeliveryClient.PizzaService.IPizzaDelivery>, PizzaDeliveryClient.PizzaService.IPizzaDelivery {
            
            public PizzaDeliveryClientChannel(System.ServiceModel.ClientBase<PizzaDeliveryClient.PizzaService.IPizzaDelivery> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginPing(PizzaDeliveryClient.PizzaService.PingRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("Ping", _args, callback, asyncState);
                return _result;
            }
            
            public PizzaDeliveryClient.PizzaService.PingResponse EndPing(System.IAsyncResult result) {
                object[] _args = new object[0];
                PizzaDeliveryClient.PizzaService.PingResponse _result = ((PizzaDeliveryClient.PizzaService.PingResponse)(base.EndInvoke("Ping", _args, result)));
                return _result;
            }
        }
    }
}
