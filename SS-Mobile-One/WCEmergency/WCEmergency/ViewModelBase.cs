using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Info;
using Microsoft.Phone.UserData;
using WCEmergency.Common;
using WCEmergency.EventArgs;
using WCEmergency.Extentions;

namespace WCEmergency
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
       //private SystemModuleContext _systemModuleContext = SystemModuleContext.Unknown;
        private readonly IThreadDispatcher _threadDispatcher;
        private const string PleaseWaitMessage = "Please wait...";
        public const string UnexpectedFailureMessage = "Cannot retrieve data";
        private const string OfflineModeMessage = "Offline Mode";

        public ViewModelBase(IThreadDispatcher threadDispatcher, string pageTitle, string userProgressKey = "")

        {
            _threadDispatcher = threadDispatcher;
             UserProgressKey = userProgressKey;

            RunSetup();
            # if DEBUG
            //uncomment this to see diagnostics info (you also should set session manager value for that
            //DetermineIfDiagnosticsInfoIsShown();
            #endif
            PageTitle = pageTitle;

            ApplicationTitle = "WC emergency";
        }

       // public delegate void AlertMessageEventHandler(object sender, AlertMessageEventArgs e);
        public delegate void NavigateToPageEventHandler(object sender, NavigateToEventArgs e);
        public delegate void NavigateBackEventHandler(object sender);
        public delegate void NavigateBackToPageEventHandler( object sender, string pageName );
        public delegate void ExitApplicationEventHandler();
      //  public delegate void AsyncProcessSatartedEventHandler(object sender, AsyncProcessingEventArgs e);
      //  public delegate void OpenPopupDialogEventHandler( object sender, PopupDialogEventArgs e );

        public string ActivePanelPivotKey { get; private set; }

       // public event AsyncProcessSatartedEventHandler AsyncProcessStarted;
        public event EventHandler AsyncProcessFinished;
      //  public event AlertMessageEventHandler AlertMessage;
        public event NavigateToPageEventHandler NavigateToPage;
        public event NavigateBackEventHandler NavigateBack;
        public event NavigateBackToPageEventHandler NavigateBackToPage;
        public event ExitApplicationEventHandler ExitApplication;
      //  public event OpenPopupDialogEventHandler OpenPopupDialog;
        public event EventHandler DataPopulated;
        

        #region Diagnostics Helpers

       /* private void DetermineIfDiagnosticsInfoIsShown()
        {
            var sessionManager = new SessionManager(); //ServiceLocator.Get<ISessionManager>();
            if (sessionManager.Contains(Constants.DisplayMemoryUsage))
            {
                ShouldShowDiagnosticsInformation = (bool)sessionManager.GetValue(Constants.DisplayMemoryUsage);
                return;
            }
                ShouldShowDiagnosticsInformation = false;
            }*/

        public virtual double DeviceTotalMemory
        {
            get
            {
                if ( !ShouldShowDiagnosticsInformation ) { return 0; }
                var rawMemory = (long)DeviceExtendedProperties.GetValue( "DeviceTotalMemory" );
                var calculatedInMb = (double)rawMemory / 1048576;

                return Math.Round( calculatedInMb, 2 );
            }
        }

        public virtual double DevicePeakMemory
        {
            get
            {
                if ( !ShouldShowDiagnosticsInformation ) { return 0; }
                var rawMemory = (long)DeviceExtendedProperties.GetValue( "ApplicationPeakMemoryUsage" );
                var calculatedInMb = (double)rawMemory / 1048576;

                return Math.Round( calculatedInMb, 2 );
            }
        }

        public virtual double DeviceCurrentMemory
        {
            get
            {
                if ( !ShouldShowDiagnosticsInformation ) { return 0; }
                var rawMemory = (long)DeviceExtendedProperties.GetValue( "ApplicationCurrentMemoryUsage" );
                var calculatedInMb = (double)rawMemory / 1048576;

                return Math.Round( calculatedInMb, 2 );
            }
        }

        public bool ShouldShowDiagnosticsInformation { get; set; }

        #endregion

        #region View Model Initialization

        private void RunSetup()
        {
            InitializeViewModel();
           // ConfigureProgressMonitor();
        }
        
       /* protected virtual void ConfigureProgressMonitor()
        {
            // if there was no key provided then skip this
            if ( !UserProgressKey.Equals( string.Empty ))
            {
                ProgressMonitor.Monitor( UserProgressKey );    
            }
            
            ProgressMonitor.ProgressUpdated += OnProgressMonitorUpdated;
        }

        protected virtual void OnProgressMonitorUpdated( object s, ProgressMonitoryStatusChangedEventArgs e )
        {
            switch ( e.NewStatus )
            {
                case ProgressStatus.Completed:
                case ProgressStatus.CompletedWithFailure:
                    RaiseAsyncProcessFinished();
                    break;

                case ProgressStatus.InProgress:
                    RaiseAsyncProcessStarted();
                    break;
            }
        }*/
        
        protected virtual void InitializeViewModel() { }

       public virtual void PopulateData()
        {
           /* if ( !UserProgressKey.Equals( string.Empty ) )
            {
                ProgressMonitor.Pulse( UserProgressKey, ProgressStatus.InProgress );
            }*/

            RaiseDataPopulated();
        }

        public virtual void SetupCommands() { }

        #endregion

        #region Async Helpers

        protected void BeginDispatcherInvoke( Action action )
        {
            _threadDispatcher.RunOnUiThread(action);
        }


      /*  protected void RunAsyncAction( Action action )
        {
            _threadDispatcher.RunOnBackgroundThread(action, OnExceptionAction);
        }*/

       /* private void OnExceptionAction(Exception exception)
        {
            //we do not interested in showing any message to user
            //BeginDispatcherInvoke(() => RaiseAlertMessageToPage(exception.Message));

            var errorProvider = ServiceLocator.Get<IExceptionHandlingProvider>();
            errorProvider.LogException(exception.Message, exception);
        }*/

        #endregion

        public virtual void OnNavigateTo(IDictionary<string, string> parameters, Uri uri)
        {
            //SetupCommands();
        }

        public virtual void OnNavigateFrom(NavigationEventArgs e)
        {
            //Do nothing here
        }

        public virtual void OnUnloaded() { }

        #region PopulateData base logic

        /// <summary>
        /// base property for having the status
        /// </summary>
        private string _inlineStatus;
        public string InlineStatus
        {
            get { return _inlineStatus; }
            set
            {
                _inlineStatus = value;
                RaisePropertyChanged("InlineStatus");
            }
        }

       /* protected virtual void LocalRequestStarted(object sender, LocalRequestStartedEventArgs args)
        {
            RaiseAsyncProcessStarted();
        }

        protected virtual void RemoteRequestStarted(object sender, RemoteRequestStartedEventArgs args)
        {
            RaiseAsyncProcessStarted();
        }

        public virtual void DataRequestFinished( object sender, RequestFinishedEventArgs<object> args )
        {
            BeginDispatcherInvoke(() =>
                                        {
            switch ( args.RequestResult )
            {
                                                case DataRequestResult.DoesntNeedRefresh:
                                                    DoesntNeedRefresh(args);
                                                    RemoteResultFound(args);
                                                    break;
                case DataRequestResult.UnexpectedFailure:
                                                    UnexpectedFailure(args);
                                                    RemoteResultFound(args);
                    break;
                case DataRequestResult.LocalDataNotFound:
                                                    LocalDataNotFound(args);
                                                    LocalResultFound(args);
                    break;
                case DataRequestResult.RemoteDataNotFound:
                                                    RemoteDataNotFound(args);
                                                    RemoteResultFound(args);
                    break;
                case DataRequestResult.OfflineMode:
                                                    OfflineMode(args);
                                                    RemoteResultFound(args);
                                                    break;
                                                case DataRequestResult.NoConnectionAvailable:
                                                    NoNetworkConnectivity(args);
                                                    RemoteResultFound(args);
                                                    break;
                                                case DataRequestResult.LocalDataFound:
                                                    LocalDataFound(args);
                                                    LocalResultFound(args);
                                                    DataFound(args);
                                                    break;
                case DataRequestResult.RemoteDataFound:
                                                    RemoteDataFound(args);
                                                    RemoteResultFound(args);
                                                    DataFound(args);
                    break;
                                            }
                                            DataFinished(args);
                                        });
        }*/

        /// <summary>
        /// Override this to handle all remote results
        /// </summary>
       /* protected virtual void RemoteResultFound(RequestFinishedEventArgs<object> args)
        {    
            }

        /// <summary>
        /// Override this to handle all local results
        /// </summary>
        protected virtual void LocalResultFound(RequestFinishedEventArgs<object> args)
        {
        }

        /// <summary>
        /// Override this to handle all FOUND results
        /// </summary>
        protected virtual void DataFound(RequestFinishedEventArgs<object> args)
        {            
        }

        /// <summary>
        /// Override this to handle all results
        /// </summary>
        protected virtual void DataFinished(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            
        }

        /// <summary>
        /// Override this to handle situation if refresh is not needed
        /// </summary>
        protected virtual void DoesntNeedRefresh(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            RaiseAsyncProcessFinished();
        }

        /// <summary>
        /// Override this to handle situation if unexpected failure occured
        /// </summary>
        protected virtual void UnexpectedFailure(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            ProgressMonitor.Pulse(UserProgressKey, ProgressStatus.CompletedWithFailure);
            RaiseAlertMessageToPage(UnexpectedFailureMessage);
            RaiseAsyncProcessFinished();
        }

        /// <summary>
        /// Override this to handle situation if no data in cache found
        /// </summary>
        protected virtual void LocalDataNotFound(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
        }

        /// <summary>
        /// Override this to handle situation if data on server is not found
        /// </summary>
        protected virtual void RemoteDataNotFound(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            ProgressMonitor.Pulse(UserProgressKey, ProgressStatus.Completed);
            RaiseAsyncProcessFinished();
        }

        /// <summary>
        /// Override this to handle situation if offline mode
        /// </summary>
        protected virtual void OfflineMode(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            ProgressMonitor.Pulse(UserProgressKey, ProgressStatus.Completed);
            InformAboutOfflineMode();
            RaiseAsyncProcessFinished();
        }

        /// <summary>
        /// Override this to handle situation if offline mode
        /// </summary>
        protected virtual void NoNetworkConnectivity(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            ProgressMonitor.Pulse(UserProgressKey, ProgressStatus.Completed);
            InformAboutNoNetworkConnection();
            RaiseAsyncProcessFinished();
        }

        /// <summary>
        /// Override this to handle situation if data is found on server
        /// </summary>
        protected virtual void RemoteDataFound(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            ProgressMonitor.Pulse(UserProgressKey, ProgressStatus.Completed);
            RaiseAsyncProcessFinished();
        }

        /// <summary>
        /// Override this to handle situation if data is found in cache
        /// </summary>
        protected virtual void LocalDataFound(RequestFinishedEventArgs<object> requestFinishedEventArgs)
        {
            ProgressMonitor.Pulse(UserProgressKey, ProgressStatus.Completed);
            RaiseAsyncProcessFinished();
        }

        protected void InformAboutOfflineMode()
        {
            RaiseAlertMessageToPage(OfflineModeMessage);
        }

        protected void InformAboutNoNetworkConnection()
        {
            RaiseAlertMessageToPage("No network connection");
        }
        */
        #endregion

        protected void RaiseDataPopulated()
        {
            if (DataPopulated != null)
            {
                DataPopulated(this, System.EventArgs.Empty);
            }
        }
        /*
        protected void RaiseAsyncProcessStarted(string message = "")
        {
            if (AsyncProcessStarted != null)
            {
                AsyncProcessStarted(this, new AsyncProcessingEventArgs(message));
            }
        }

        protected void RaiseAsyncProcessStarted()
        {
            RaiseAsyncProcessStarted(PleaseWaitMessage);
        }

        protected void RaiseAsyncProcessFinished()
        {
            if (AsyncProcessFinished != null)
            {
                AsyncProcessFinished(this, new EventArgs());
            }
        }*/

        protected void RaiseAlertMessageToPage(string message, params string[] args)
        {
            RaiseAlertMessageToPage(string.Format(message, args));
        }
        /*
        protected void RaiseAlertMessageToPage(string message)
        {
            BeginDispatcherInvoke( () =>
                                       {
                                           if ( AlertMessage != null )
                                           {
                                               AlertMessage( this, new AlertMessageEventArgs( message ) );
                                               
                                           }
                                       } );

        }*/

        protected void RaiseNavigateToPage(string routeName)
        {
            if (NavigateToPage != null)
            {
                NavigateToPage(this, new NavigateToEventArgs(routeName));
            }
            else
            {
                Debug.WriteLine( "There was a call to NavigateToPage which was not registered at {0} on thread {1}", DateTime.Now, Thread.CurrentThread.ManagedThreadId );
            }
        }
        /*
        protected void RaiseOpenPopupDialog( IPopupViewBase popupDialog, Action<object, EventArgs> closeAction )
        {
            if ( OpenPopupDialog != null )
            {
                OpenPopupDialog( this, new PopupDialogEventArgs( popupDialog, closeAction ) );
            }
        }*/

        public void RaiseNavigateBack()
        {
            if (NavigateBack != null)
            {
                NavigateBack( this );
            }
        }

        public void RaiseNavigateBackToPage( string pageName )
        {
            if ( NavigateBackToPage != null )
            {
                NavigateBackToPage( this, pageName );
            }
        }

        public void RaiseNavigateToPage( string routeName, params string[] args )
        {
            RaiseNavigateToPage(string.Format(routeName, args));
        }

        public void RaiseExitApplication()
        {
            if (ExitApplication != null)
            {
                BeginDispatcherInvoke(() => ExitApplication());
            }
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            BeginDispatcherInvoke(() => base.RaisePropertyChanged(propertyName));
        }

        protected virtual void RaisePropertyChanged(string propertyName, bool dirtyPage)
        {
            PageDirty = dirtyPage;

            RaisePropertyChanged(propertyName);
        }

        protected virtual void RaisePropertyChanged(Expression<Func<object>> property)
        {
            var propertyChangedName = "".GetPropertyName(property);
            BeginDispatcherInvoke(() => RaisePropertyChanged(propertyChangedName));
        }
/*
        public void SetAppointmentContext( Appointment appointment )
        {
            ContextManager.SetContext( new ActiveAppointmentContext{Appointment = appointment} );
        }

        public void SetPatientContext( Patient patient )
        {
            ContextManager.SetContext( new ActivePatientContext { Patient = patient } );
        }
        
        public void SetActivePageContext( string pageName, string pivotName = "", Int32 pivotIndex = 0 )
        {
            ContextManager.SetContext( new ActivePageContext ( pageName, pivotName, pivotIndex ) );
        } */
        
        public string ApplicationTitle { get; protected set; }

        /// <summary>
        /// The pointer to the user progress monitor which is used to allow the VM to know
        ///     the state of the data being loaded at any point in time
        /// </summary>
   //     protected UserProgressMonitor ProgressMonitor { get; set; }

        /// <summary>
        /// This is the default progress key which will be used each time the VM is loaded
        /// This should be overwritten in cases where there are multiple view models being used on 
        ///     a given form.  If this is not overwritten exceptions will be thrown        
        /// </summary>
        public string UserProgressKey { get; set; }

        private string _pageTitle;
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            protected set
            {
                _pageTitle = value;
                RaisePropertyChanged("PageTitle");
            }
        }

        private string _pageTitle2Level;
        public string PageTitle2Level
        {
            get
            {
                return _pageTitle2Level;
            }
            protected set
            {
                _pageTitle2Level = value;
                RaisePropertyChanged("PageTitle2Level");
            }
        }

        public bool PageDirty { get; protected set; }
     /*   protected IConfigurationReader ConfigurationReader { get; set; }
        protected IRouteCreator RouteCreator { get; set; }
        protected IContextManager ContextManager { get; set; }
        protected IFeatureAvailabilityManager FeatureAvailability { get; set; }
        
        public ActiveAppointmentContext AppointmentContext { get { return ContextManager.AppointmentContext; } }
        public ActivePatientContext PatientContext { get { return ContextManager.PatientContext; } }
        public ActivePageContext ActivePageContext { get { return ContextManager.ActivePageContext; } }

        public SystemModuleContext SystemModuleContext
        {
            get { return _systemModuleContext; }
            set
            {
                _systemModuleContext = value;
                RaisePropertyChanged("SystemModuleContext");
                RaisePropertyChanged("PageHeaderStyle");
            }
        }

        public Style PageHeaderStyle
        {
            get { return ResourceLocator.LoadFor(SystemModuleContext, StyleNames.PageHeader); }
        }

        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );

            if ( ProgressMonitor != null )
            {
                ProgressMonitor.ProgressUpdated -= OnProgressMonitorUpdated;
            }
        }

        protected void SubmitTextData()
        {
            var focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is TextBox)
            {
                (focusObj as TextBox).ForceBinding();
            }
        }
        */
        /// <summary>
        /// Occurs when IsValidInformation returns false
        /// </summary>
        public event EventHandler ValidationFailed;

        public void OnValidationFailed(System.EventArgs e)
        {
            EventHandler handler = ValidationFailed;
            if (handler != null) handler(this, e);
        }

        protected virtual bool IsValidInformation()
        {
            throw new NotImplementedException();
        }

        private EventHandler _sessionBeforeSavedHandler;


   /*     protected EventHandler SessionBeforeSavedHandler
        {
            get
            {
                return _sessionBeforeSavedHandler ??
                       (_sessionBeforeSavedHandler = new EventHandler(OnBeforeSessionSaving));
    }
}*/

    /*    protected virtual void OnBeforeSessionSaving(object sender, EventArgs e)
        {
            SubmitTextData();
        }*/
    }
}
