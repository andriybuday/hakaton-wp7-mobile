﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace sdkSilverlightXNACS.MiniGameService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="HeroDataContact", Namespace="http://schemas.datacontract.org/2004/07/MiniGame.DataContractsShared")]
    public partial class HeroDataContact : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool IsInYourTeamField;
        
        private byte[] MemberPhotoField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsInYourTeam {
            get {
                return this.IsInYourTeamField;
            }
            set {
                if ((this.IsInYourTeamField.Equals(value) != true)) {
                    this.IsInYourTeamField = value;
                    this.RaisePropertyChanged("IsInYourTeam");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] MemberPhoto {
            get {
                return this.MemberPhotoField;
            }
            set {
                if ((object.ReferenceEquals(this.MemberPhotoField, value) != true)) {
                    this.MemberPhotoField = value;
                    this.RaisePropertyChanged("MemberPhoto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MiniGameService.IMiniGameService")]
    public interface IMiniGameService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IMiniGameService/RegisterMe", ReplyAction="http://tempuri.org/IMiniGameService/RegisterMeResponse")]
        System.IAsyncResult BeginRegisterMe(System.AsyncCallback callback, object asyncState);
        
        string EndRegisterMe(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IMiniGameService/SetTeam", ReplyAction="http://tempuri.org/IMiniGameService/SetTeamResponse")]
        System.IAsyncResult BeginSetTeam(string myName, System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> myHeros, System.AsyncCallback callback, object asyncState);
        
        bool EndSetTeam(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IMiniGameService/GetEnemyTeam", ReplyAction="http://tempuri.org/IMiniGameService/GetEnemyTeamResponse")]
        System.IAsyncResult BeginGetEnemyTeam(string myTeamName, System.AsyncCallback callback, object asyncState);
        
        System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> EndGetEnemyTeam(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMiniGameServiceChannel : sdkSilverlightXNACS.MiniGameService.IMiniGameService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegisterMeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public RegisterMeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public partial class SetTeamCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SetTeamCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetEnemyTeamCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetEnemyTeamCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MiniGameServiceClient : System.ServiceModel.ClientBase<sdkSilverlightXNACS.MiniGameService.IMiniGameService>, sdkSilverlightXNACS.MiniGameService.IMiniGameService {
        
        private BeginOperationDelegate onBeginRegisterMeDelegate;
        
        private EndOperationDelegate onEndRegisterMeDelegate;
        
        private System.Threading.SendOrPostCallback onRegisterMeCompletedDelegate;
        
        private BeginOperationDelegate onBeginSetTeamDelegate;
        
        private EndOperationDelegate onEndSetTeamDelegate;
        
        private System.Threading.SendOrPostCallback onSetTeamCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetEnemyTeamDelegate;
        
        private EndOperationDelegate onEndGetEnemyTeamDelegate;
        
        private System.Threading.SendOrPostCallback onGetEnemyTeamCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public MiniGameServiceClient() {
        }
        
        public MiniGameServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MiniGameServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MiniGameServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MiniGameServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<RegisterMeCompletedEventArgs> RegisterMeCompleted;
        
        public event System.EventHandler<SetTeamCompletedEventArgs> SetTeamCompleted;
        
        public event System.EventHandler<GetEnemyTeamCompletedEventArgs> GetEnemyTeamCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult sdkSilverlightXNACS.MiniGameService.IMiniGameService.BeginRegisterMe(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRegisterMe(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string sdkSilverlightXNACS.MiniGameService.IMiniGameService.EndRegisterMe(System.IAsyncResult result) {
            return base.Channel.EndRegisterMe(result);
        }
        
        private System.IAsyncResult OnBeginRegisterMe(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((sdkSilverlightXNACS.MiniGameService.IMiniGameService)(this)).BeginRegisterMe(callback, asyncState);
        }
        
        private object[] OnEndRegisterMe(System.IAsyncResult result) {
            string retVal = ((sdkSilverlightXNACS.MiniGameService.IMiniGameService)(this)).EndRegisterMe(result);
            return new object[] {
                    retVal};
        }
        
        private void OnRegisterMeCompleted(object state) {
            if ((this.RegisterMeCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RegisterMeCompleted(this, new RegisterMeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RegisterMeAsync() {
            this.RegisterMeAsync(null);
        }
        
        public void RegisterMeAsync(object userState) {
            if ((this.onBeginRegisterMeDelegate == null)) {
                this.onBeginRegisterMeDelegate = new BeginOperationDelegate(this.OnBeginRegisterMe);
            }
            if ((this.onEndRegisterMeDelegate == null)) {
                this.onEndRegisterMeDelegate = new EndOperationDelegate(this.OnEndRegisterMe);
            }
            if ((this.onRegisterMeCompletedDelegate == null)) {
                this.onRegisterMeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRegisterMeCompleted);
            }
            base.InvokeAsync(this.onBeginRegisterMeDelegate, null, this.onEndRegisterMeDelegate, this.onRegisterMeCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult sdkSilverlightXNACS.MiniGameService.IMiniGameService.BeginSetTeam(string myName, System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> myHeros, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSetTeam(myName, myHeros, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        bool sdkSilverlightXNACS.MiniGameService.IMiniGameService.EndSetTeam(System.IAsyncResult result) {
            return base.Channel.EndSetTeam(result);
        }
        
        private System.IAsyncResult OnBeginSetTeam(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string myName = ((string)(inValues[0]));
            System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> myHeros = ((System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact>)(inValues[1]));
            return ((sdkSilverlightXNACS.MiniGameService.IMiniGameService)(this)).BeginSetTeam(myName, myHeros, callback, asyncState);
        }
        
        private object[] OnEndSetTeam(System.IAsyncResult result) {
            bool retVal = ((sdkSilverlightXNACS.MiniGameService.IMiniGameService)(this)).EndSetTeam(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSetTeamCompleted(object state) {
            if ((this.SetTeamCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SetTeamCompleted(this, new SetTeamCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SetTeamAsync(string myName, System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> myHeros) {
            this.SetTeamAsync(myName, myHeros, null);
        }
        
        public void SetTeamAsync(string myName, System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> myHeros, object userState) {
            if ((this.onBeginSetTeamDelegate == null)) {
                this.onBeginSetTeamDelegate = new BeginOperationDelegate(this.OnBeginSetTeam);
            }
            if ((this.onEndSetTeamDelegate == null)) {
                this.onEndSetTeamDelegate = new EndOperationDelegate(this.OnEndSetTeam);
            }
            if ((this.onSetTeamCompletedDelegate == null)) {
                this.onSetTeamCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSetTeamCompleted);
            }
            base.InvokeAsync(this.onBeginSetTeamDelegate, new object[] {
                        myName,
                        myHeros}, this.onEndSetTeamDelegate, this.onSetTeamCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult sdkSilverlightXNACS.MiniGameService.IMiniGameService.BeginGetEnemyTeam(string myTeamName, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetEnemyTeam(myTeamName, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> sdkSilverlightXNACS.MiniGameService.IMiniGameService.EndGetEnemyTeam(System.IAsyncResult result) {
            return base.Channel.EndGetEnemyTeam(result);
        }
        
        private System.IAsyncResult OnBeginGetEnemyTeam(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string myTeamName = ((string)(inValues[0]));
            return ((sdkSilverlightXNACS.MiniGameService.IMiniGameService)(this)).BeginGetEnemyTeam(myTeamName, callback, asyncState);
        }
        
        private object[] OnEndGetEnemyTeam(System.IAsyncResult result) {
            System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> retVal = ((sdkSilverlightXNACS.MiniGameService.IMiniGameService)(this)).EndGetEnemyTeam(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetEnemyTeamCompleted(object state) {
            if ((this.GetEnemyTeamCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetEnemyTeamCompleted(this, new GetEnemyTeamCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetEnemyTeamAsync(string myTeamName) {
            this.GetEnemyTeamAsync(myTeamName, null);
        }
        
        public void GetEnemyTeamAsync(string myTeamName, object userState) {
            if ((this.onBeginGetEnemyTeamDelegate == null)) {
                this.onBeginGetEnemyTeamDelegate = new BeginOperationDelegate(this.OnBeginGetEnemyTeam);
            }
            if ((this.onEndGetEnemyTeamDelegate == null)) {
                this.onEndGetEnemyTeamDelegate = new EndOperationDelegate(this.OnEndGetEnemyTeam);
            }
            if ((this.onGetEnemyTeamCompletedDelegate == null)) {
                this.onGetEnemyTeamCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetEnemyTeamCompleted);
            }
            base.InvokeAsync(this.onBeginGetEnemyTeamDelegate, new object[] {
                        myTeamName}, this.onEndGetEnemyTeamDelegate, this.onGetEnemyTeamCompletedDelegate, userState);
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
        
        protected override sdkSilverlightXNACS.MiniGameService.IMiniGameService CreateChannel() {
            return new MiniGameServiceClientChannel(this);
        }
        
        private class MiniGameServiceClientChannel : ChannelBase<sdkSilverlightXNACS.MiniGameService.IMiniGameService>, sdkSilverlightXNACS.MiniGameService.IMiniGameService {
            
            public MiniGameServiceClientChannel(System.ServiceModel.ClientBase<sdkSilverlightXNACS.MiniGameService.IMiniGameService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginRegisterMe(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("RegisterMe", _args, callback, asyncState);
                return _result;
            }
            
            public string EndRegisterMe(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("RegisterMe", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginSetTeam(string myName, System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> myHeros, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = myName;
                _args[1] = myHeros;
                System.IAsyncResult _result = base.BeginInvoke("SetTeam", _args, callback, asyncState);
                return _result;
            }
            
            public bool EndSetTeam(System.IAsyncResult result) {
                object[] _args = new object[0];
                bool _result = ((bool)(base.EndInvoke("SetTeam", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetEnemyTeam(string myTeamName, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = myTeamName;
                System.IAsyncResult _result = base.BeginInvoke("GetEnemyTeam", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> EndGetEnemyTeam(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact> _result = ((System.Collections.Generic.List<sdkSilverlightXNACS.MiniGameService.HeroDataContact>)(base.EndInvoke("GetEnemyTeam", _args, result)));
                return _result;
            }
        }
    }
}
