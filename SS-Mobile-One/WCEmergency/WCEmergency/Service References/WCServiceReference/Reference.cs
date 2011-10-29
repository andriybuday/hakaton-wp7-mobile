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
namespace WCEmergency.WCServiceReference {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Coordinate", Namespace="http://schemas.datacontract.org/2004/07/WCEmergency.Common")]
    public partial class Coordinate : object, System.ComponentModel.INotifyPropertyChanged {
        
        private double XField;
        
        private double YField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double X {
            get {
                return this.XField;
            }
            set {
                if ((this.XField.Equals(value) != true)) {
                    this.XField = value;
                    this.RaisePropertyChanged("X");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Y {
            get {
                return this.YField;
            }
            set {
                if ((this.YField.Equals(value) != true)) {
                    this.YField = value;
                    this.RaisePropertyChanged("Y");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Toilet", Namespace="http://schemas.datacontract.org/2004/07/WCEmergency.Common")]
    public partial class Toilet : object, System.ComponentModel.INotifyPropertyChanged {
        
        private WCEmergency.WCServiceReference.Coordinate CoordinateField;
        
        private string DescriptionField;
        
        private int IdField;
        
        private string NameField;
        
        private byte[] PictureField;
        
        private System.Nullable<int> RateField;
        
        private WCEmergency.WCServiceReference.Sex SexField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public WCEmergency.WCServiceReference.Coordinate Coordinate {
            get {
                return this.CoordinateField;
            }
            set {
                if ((object.ReferenceEquals(this.CoordinateField, value) != true)) {
                    this.CoordinateField = value;
                    this.RaisePropertyChanged("Coordinate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
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
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Picture {
            get {
                return this.PictureField;
            }
            set {
                if ((object.ReferenceEquals(this.PictureField, value) != true)) {
                    this.PictureField = value;
                    this.RaisePropertyChanged("Picture");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Rate {
            get {
                return this.RateField;
            }
            set {
                if ((this.RateField.Equals(value) != true)) {
                    this.RateField = value;
                    this.RaisePropertyChanged("Rate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public WCEmergency.WCServiceReference.Sex Sex {
            get {
                return this.SexField;
            }
            set {
                if ((this.SexField.Equals(value) != true)) {
                    this.SexField = value;
                    this.RaisePropertyChanged("Sex");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Sex", Namespace="http://schemas.datacontract.org/2004/07/WCEmergency.Common")]
    public enum Sex : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Male = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Female = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unisex = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WCServiceReference.IWCEmergencyService")]
    public interface IWCEmergencyService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IWCEmergencyService/GetNearestToiltes", ReplyAction="http://tempuri.org/IWCEmergencyService/GetNearestToiltesResponse")]
        System.IAsyncResult BeginGetNearestToiltes(WCEmergency.WCServiceReference.Coordinate currrentPosition, double distance, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet> EndGetNearestToiltes(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IWCEmergencyService/AddToilet", ReplyAction="http://tempuri.org/IWCEmergencyService/AddToiletResponse")]
        System.IAsyncResult BeginAddToilet(WCEmergency.WCServiceReference.Toilet newToilet, System.AsyncCallback callback, object asyncState);
        
        void EndAddToilet(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWCEmergencyServiceChannel : WCEmergency.WCServiceReference.IWCEmergencyService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetNearestToiltesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetNearestToiltesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WCEmergencyServiceClient : System.ServiceModel.ClientBase<WCEmergency.WCServiceReference.IWCEmergencyService>, WCEmergency.WCServiceReference.IWCEmergencyService {
        
        private BeginOperationDelegate onBeginGetNearestToiltesDelegate;
        
        private EndOperationDelegate onEndGetNearestToiltesDelegate;
        
        private System.Threading.SendOrPostCallback onGetNearestToiltesCompletedDelegate;
        
        private BeginOperationDelegate onBeginAddToiletDelegate;
        
        private EndOperationDelegate onEndAddToiletDelegate;
        
        private System.Threading.SendOrPostCallback onAddToiletCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public WCEmergencyServiceClient() {
        }
        
        public WCEmergencyServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WCEmergencyServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCEmergencyServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCEmergencyServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<GetNearestToiltesCompletedEventArgs> GetNearestToiltesCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> AddToiletCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult WCEmergency.WCServiceReference.IWCEmergencyService.BeginGetNearestToiltes(WCEmergency.WCServiceReference.Coordinate currrentPosition, double distance, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetNearestToiltes(currrentPosition, distance, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet> WCEmergency.WCServiceReference.IWCEmergencyService.EndGetNearestToiltes(System.IAsyncResult result) {
            return base.Channel.EndGetNearestToiltes(result);
        }
        
        private System.IAsyncResult OnBeginGetNearestToiltes(object[] inValues, System.AsyncCallback callback, object asyncState) {
            WCEmergency.WCServiceReference.Coordinate currrentPosition = ((WCEmergency.WCServiceReference.Coordinate)(inValues[0]));
            double distance = ((double)(inValues[1]));
            return ((WCEmergency.WCServiceReference.IWCEmergencyService)(this)).BeginGetNearestToiltes(currrentPosition, distance, callback, asyncState);
        }
        
        private object[] OnEndGetNearestToiltes(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet> retVal = ((WCEmergency.WCServiceReference.IWCEmergencyService)(this)).EndGetNearestToiltes(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetNearestToiltesCompleted(object state) {
            if ((this.GetNearestToiltesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetNearestToiltesCompleted(this, new GetNearestToiltesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetNearestToiltesAsync(WCEmergency.WCServiceReference.Coordinate currrentPosition, double distance) {
            this.GetNearestToiltesAsync(currrentPosition, distance, null);
        }
        
        public void GetNearestToiltesAsync(WCEmergency.WCServiceReference.Coordinate currrentPosition, double distance, object userState) {
            if ((this.onBeginGetNearestToiltesDelegate == null)) {
                this.onBeginGetNearestToiltesDelegate = new BeginOperationDelegate(this.OnBeginGetNearestToiltes);
            }
            if ((this.onEndGetNearestToiltesDelegate == null)) {
                this.onEndGetNearestToiltesDelegate = new EndOperationDelegate(this.OnEndGetNearestToiltes);
            }
            if ((this.onGetNearestToiltesCompletedDelegate == null)) {
                this.onGetNearestToiltesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetNearestToiltesCompleted);
            }
            base.InvokeAsync(this.onBeginGetNearestToiltesDelegate, new object[] {
                        currrentPosition,
                        distance}, this.onEndGetNearestToiltesDelegate, this.onGetNearestToiltesCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult WCEmergency.WCServiceReference.IWCEmergencyService.BeginAddToilet(WCEmergency.WCServiceReference.Toilet newToilet, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginAddToilet(newToilet, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void WCEmergency.WCServiceReference.IWCEmergencyService.EndAddToilet(System.IAsyncResult result) {
            base.Channel.EndAddToilet(result);
        }
        
        private System.IAsyncResult OnBeginAddToilet(object[] inValues, System.AsyncCallback callback, object asyncState) {
            WCEmergency.WCServiceReference.Toilet newToilet = ((WCEmergency.WCServiceReference.Toilet)(inValues[0]));
            return ((WCEmergency.WCServiceReference.IWCEmergencyService)(this)).BeginAddToilet(newToilet, callback, asyncState);
        }
        
        private object[] OnEndAddToilet(System.IAsyncResult result) {
            ((WCEmergency.WCServiceReference.IWCEmergencyService)(this)).EndAddToilet(result);
            return null;
        }
        
        private void OnAddToiletCompleted(object state) {
            if ((this.AddToiletCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.AddToiletCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void AddToiletAsync(WCEmergency.WCServiceReference.Toilet newToilet) {
            this.AddToiletAsync(newToilet, null);
        }
        
        public void AddToiletAsync(WCEmergency.WCServiceReference.Toilet newToilet, object userState) {
            if ((this.onBeginAddToiletDelegate == null)) {
                this.onBeginAddToiletDelegate = new BeginOperationDelegate(this.OnBeginAddToilet);
            }
            if ((this.onEndAddToiletDelegate == null)) {
                this.onEndAddToiletDelegate = new EndOperationDelegate(this.OnEndAddToilet);
            }
            if ((this.onAddToiletCompletedDelegate == null)) {
                this.onAddToiletCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnAddToiletCompleted);
            }
            base.InvokeAsync(this.onBeginAddToiletDelegate, new object[] {
                        newToilet}, this.onEndAddToiletDelegate, this.onAddToiletCompletedDelegate, userState);
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
        
        protected override WCEmergency.WCServiceReference.IWCEmergencyService CreateChannel() {
            return new WCEmergencyServiceClientChannel(this);
        }
        
        private class WCEmergencyServiceClientChannel : ChannelBase<WCEmergency.WCServiceReference.IWCEmergencyService>, WCEmergency.WCServiceReference.IWCEmergencyService {
            
            public WCEmergencyServiceClientChannel(System.ServiceModel.ClientBase<WCEmergency.WCServiceReference.IWCEmergencyService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetNearestToiltes(WCEmergency.WCServiceReference.Coordinate currrentPosition, double distance, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = currrentPosition;
                _args[1] = distance;
                System.IAsyncResult _result = base.BeginInvoke("GetNearestToiltes", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet> EndGetNearestToiltes(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet> _result = ((System.Collections.ObjectModel.ObservableCollection<WCEmergency.WCServiceReference.Toilet>)(base.EndInvoke("GetNearestToiltes", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginAddToilet(WCEmergency.WCServiceReference.Toilet newToilet, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = newToilet;
                System.IAsyncResult _result = base.BeginInvoke("AddToilet", _args, callback, asyncState);
                return _result;
            }
            
            public void EndAddToilet(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("AddToilet", _args, result);
            }
        }
    }
}