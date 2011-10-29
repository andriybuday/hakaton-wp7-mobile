using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
/*using Allscripts.Homecare.Mobile.CommonUI.EventArgs;
using Allscripts.Homecare.Mobile.Device.CommonUI.EventArguments;
using Allscripts.Homecare.Mobile.Device.CommonUI.Extensions;
using Allscripts.Homecare.Mobile.Device.CommonUI.ViewModels;
using Allscripts.Homecare.Mobile.Device.Domain.LocalStorage;
using Allscripts.Homecare.Mobile.Device.Domain.LocalStorage.SystemCache;
using Allscripts.Homecare.Mobile.Device.WinPhone7UI.Controls;
using Allscripts.Homecare.Mobile.Device.WinPhone7UI.Navigation;
using Allscripts.Homecare.Mobile.Device.WinPhone7UI.Views.Authentication;
using Allscripts.Homecare.Mobile.Device.WinPhone7UI.Views.Diagnostics;
using Allscripts.Homecare.Mobile.Domain.Common;
using Allscripts.Homecare.Mobile.Domain.Impl;
using Allscripts.Homecare.Mobile.Domain.Impl.Facades;
using Allscripts.Homecare.Mobile.Domain.Impl.Location;
using Allscripts.Homecare.Mobile.Domain.Impl.Providers;
using Allscripts.Homecare.Mobile.Domain.LocalStorage;
using Allscripts.Homecare.Mobile.Domain.LocalStorage.DataManagement;
using Allscripts.Homecare.Mobile.Domain.Models.System;
using Allscripts.Homecare.Mobile.Domain.ServiceLocation;
using Allscripts.Homecare.MobileServices.Endpoint.Shared.Services.Registration.Results;*/
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WCEmergency.EventArgs;
using WCEmergency.Extentions;
using WCEmergency.Navigation;

namespace WCEmergency
{
    public class PhoneApplicationPageBase : PhoneApplicationPage, IDisposable
    {
        private Uri _nextUri;
        private static readonly Uri ExternalUri = new Uri(@"app://external/");
        private static readonly IList<string> _pageHistory = new List<string>();

        private Popup _progressIndicatorPopup;
        //private IAuthenticationProvider _authenticationProvider;
        private readonly ApplicationBarBuilder _applicationBarBuilder = new ApplicationBarBuilder();
        private Popup _popup;
        private DispatcherTimer _popupTimer;

        public bool AsyncActionInProgress { get; private set; }
        protected bool IsLoginOpen { get; set; }

        private bool _appbarHidden;
        private bool _appbarVisible;

        public const int ScreenWidth = 480;
        public const int ScreenHeight = 800;


        public PhoneApplicationPageBase()
        {
            // we know that if the constructor is hit is is because it was created anew and not pulled from the back stack
            ViewsLifeStage = Enums.ViewLifeStage.Constructed;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            DataContext = CreateViewModel();

            RegisterViewModel(ModelView);
        }

        #region Page Life Cycle

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // We set it t this in order to know that we have left the page and it has been put onto the backstack
            ViewsLifeStage = Enums.ViewLifeStage.Rehydrated;

            ModelView.OnUnloaded();

           // UnWirePivotPanalSelectionChanged();
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual)) { return; }

            if (LayoutContext != null)
            {
                LayoutContext.Opacity = 1;
            }

           /* WirePivotPanalSelectionChanged();
            DetermineScreenToPopulate();
            DetermineActivePivot();*/
        }

      //  
       /* private void OnDetermineAutorization(object sender, RequestFinishedEventArgs<object> args)
        {
            //No need to finish async process if local data not found
            if (args.RequestResult != DataRequestResult.LocalDataNotFound)
            {
                AsyncProcessFinished(null, null);
            }

            if (args.RequestResult == DataRequestResult.RemoteDataFound)
            {
                var authorizationStatus = (AuthorizationStatus)args.FoundValues;
                switch (authorizationStatus)
                {
                    case AuthorizationStatus.UnRegistered:
                        AuthenticationProvider.UnauthorizedDeviceCleaned();

                        // navigate to the not authorized dialog    
                        NavigateToPage(this, new NavigateToEventArgs(NonAuthorizedPagePath));
                        break;

                    case AuthorizationStatus.NeedsRegistration:
                        Dispatcher.BeginInvoke( () =>
                                                    {
                                                        var deviceRegistration = new DeviceRegistration( this, null );
                                                        OpenPopupDialog( this, new PopupDialogEventArgs( deviceRegistration, ( s, e ) => DetermineScreenToPopulate(), DialogService.PopupDialogTypes.Popup ) );
                                                    } );                        
                        break;

                    //case AuthorizationStatus.Unknown:
                    case AuthorizationStatus.Registered:
                        Dispatcher.BeginInvoke( OpenSafeLogin );
                        
                        break;
                }
            }
            //offline mode, do nothing
            else if (args.RequestResult == DataRequestResult.UnexpectedFailure || args.RequestResult == DataRequestResult.OfflineMode || args.RequestResult == DataRequestResult.NoConnectionAvailable)
            {
                OpenSafeLogin();
            }
        }

        private void OpenSafeLogin()
        {
            Dispatcher.BeginInvoke(() =>
                                       {

                                           var login = new ApplicationLogin(this, null);
                                           var popupArgs = new PopupDialogEventArgs(login,
                                                                                    (s, e) =>
                                                                                    DetermineScreenToPopulate(),
                                                                                    DialogService.PopupDialogTypes.
                                                                                        Frame);
                                           OpenPopupDialog(this, popupArgs);
                                           IsLoginOpen = true;
                                       });
        }


        private void DetermineScreenToPopulate()
        {
            if (ModelView != null)
            {
                IsLoginOpen = false;
                // need to check here is we need to prompt for user login/registration
                if (AuthenticationProvider.IsDeviceRegistrationRequired())
                {
                    // need to popup the registration dialog    
                    var registration = new DeviceRegistration(this, null);

                    OpenPopupDialog(this,
                        new PopupDialogEventArgs(registration, (s, e) => DetermineScreenToPopulate()));
                }
                else if (AuthenticationProvider.IsSpecialLogin())
                {
                    // For special user we don't want to populate data for any page, since it requires correct OperatorId and ResourcesIds...
                }
                else if (AuthenticationProvider.IsLoginRequired())
                {
                    AsyncProcessStarted(null, new AsyncProcessingEventArgs("Please wait"));
                    AuthenticationProvider.IsDeviceAuthorizedForUsage(OnDetermineAutorization);
                }
                else
                {
                    InitializeUserSettings();
                    ModelView.PopulateData();
                }
            }
        }

        private void InitializeUserSettings()
        {
            var sessionManager = ServiceLocator.Get<ISessionManager>();
            if (!sessionManager.Contains<UserSettings>())
            {
                var userCacheManager = ServiceLocator.Get<UserSettingsCacheManager>();
                int currentUserId = AuthenticationProvider.FetchLastLoggedInUser().OperatorId;
                var cachedUserSettings = userCacheManager.Fetch(currentUserId);

                if (cachedUserSettings == null)
                {
                    cachedUserSettings = new UserSettings
                                             {
                                                 UseLocationServices = true,
                                                 UserId = currentUserId
                                             };
                    sessionManager.SetValue<UserSettings>(cachedUserSettings);
                    userCacheManager.Cache(cachedUserSettings);
                }
                else
                {
                    // need to push the cached in db value into memory...
                    sessionManager.SetValue<UserSettings>(cachedUserSettings);
                }

                // This allows the app to remain running while the lock screen is enabled
                if (cachedUserSettings.AllowApplicationToRunWhileLocked)
                {
                    PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
                }

                var coordinateLocator = ServiceLocator.Get<ICoordinateLocator>();
                if (cachedUserSettings.UseLocationServices)
                {
                    coordinateLocator.Start();
                }
                else
                {
                    coordinateLocator.Stop();
                }
            }
        }*/

        #endregion

        #region Page Navigation

        public virtual void ExitApplication()
        {

            BackchainingManager.Quit();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (AsyncActionInProgress || IsLoginOpen)
            {
                e.Cancel = true;
                return;
            }
            base.OnBackKeyPress(e);
            RemoveLastItemFromPageHistory();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_pageHistory.Count == 0 ||
                (_pageHistory[_pageHistory.Count - 1] != e.Uri.ToString()))
            {
                ModelView.OnNavigateTo(NavigationContext.QueryString, e.Uri);
            }

            _pageHistory.Add(e.Uri.ToString());


            // for backstack navigation
            if (_nextUri != ExternalUri)
            {
                // Loaded += OnLoaded;

                if (LayoutContext != null)
                {
                    LayoutContext.Opacity = 0;
                }

            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ModelView.OnNavigateFrom(e);

            base.OnNavigatedFrom(e);           

            _nextUri = e.Uri;
        }

        public virtual void NavigateToPage(object sender, NavigateToEventArgs e)
        {
            try
            {
                if (NavigationService.CurrentSource.ToString() != e.RouteName)
                {
                    NavigationService.Navigate(new Uri(e.RouteName, UriKind.Relative));
                }


                //NaviagationController.NaviagteTo<v, vm >();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public virtual void NavigateBack(object sender)
        {
            BackchainingManager.GoBack();
            RemoveLastItemFromPageHistory();
        }

        public void NavigateBackToPage(object sender, string pageName)
        {
            BackchainingManager.GoBack(pageName);
        }

        #endregion

        #region Popup Dialog Services

        #endregion

        #region ViewModel Services

        protected virtual ViewModelBase ModelView
        {
            get
            {
                return DataContext as ViewModelBase;
            }
        }

        public virtual ViewModelBase CreateViewModel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual))
                return null;
            throw new Exception("This property has to be overriden");
        }

        protected virtual void RegisterViewModel<TModel>(TModel model) where TModel : ViewModelBase
        {
            // needed to resolve design time null reference failures
            if (model != null)
            {
               // model.AsyncProcessStarted += AsyncProcessStarted;
               // model.AsyncProcessFinished += AsyncProcessFinished;
               // model.AlertMessage += AlertMessage;
                model.NavigateToPage += NavigateToPage;
                model.NavigateBack += NavigateBack;
                model.NavigateBackToPage += NavigateBackToPage;
                model.ExitApplication += ExitApplication;
               // model.OpenPopupDialog += OpenPopupDialog;

                model.SetupCommands();

                // not sure i like this, but this works for now
                // may want/need to do this at the view model level
                RegisterApplicationBar();
            }
        }

        #endregion

        #region Pivot/Panel Selection Services

        /// <summary>
        /// When the page is loaded it could be loaded from tombstone.  If this is the case we want to ensure that the 
        /// correct panel/pivot is set to the default one.
        /// </summary>
      /*  protected virtual void DetermineActivePivot()
        {
            var activePageContext = ViewModel<ViewModelBase>().ActivePageContext;

            if (activePageContext.PageName == GetType().Name && PivotPointer != null)
            {
                if (PivotPointer is Panorama)
                {
                    ((Panorama)PivotPointer).DefaultItem = ((Panorama)PivotPointer).Items[activePageContext.PivotIndex];
                }
                else if (PivotPointer is Pivot)
                {
                    // There is logic that possible can remove pivot items, we don't want to failure if we go again to same pivot
                    // in another context
                    if (activePageContext.PivotIndex < ((Pivot)PivotPointer).Items.Count)
                    {
                        ((Pivot) PivotPointer).SelectedItem = ((Pivot) PivotPointer).Items[activePageContext.PivotIndex];
                    }
                }
            }
        }*/

        /// <summary>
        /// This is the generic pointer to either the pivot or panarama control on the form.
        /// This is used to allow us to determine what pivot/panel should be shown when the form loads from tombstone.
        /// </summary>
        protected virtual object PivotPointer { get; private set; }

     /*   private void PivotPanelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string panelName = string.Empty;
            Int32 panelIndex = -1;

            if (PivotPointer != null)
            {
                if (PivotPointer is Panorama)
                {
                    var addedPanel = (PanoramaItem)e.AddedItems[0];
                    panelName = addedPanel.Tag != null ? addedPanel.Tag.ToString() : string.Empty;
                    panelIndex = ((Panorama)PivotPointer).SelectedIndex;
                }
                else if (PivotPointer is Pivot)
                {
                    var addedPanel = (PivotItem)e.AddedItems[0];
                    panelName = addedPanel.Tag != null ? addedPanel.Tag.ToString() : string.Empty;
                    panelIndex = ((Pivot)PivotPointer).SelectedIndex;
                }
            }

            ViewModel<ViewModelBase>().SetActivePageContext(GetType().Name, panelName, panelIndex);
        }

        private void UnWirePivotPanalSelectionChanged()
        {
            if (PivotPointer != null)
            {
                if (PivotPointer is Panorama)
                {
                    ((Panorama)PivotPointer).SelectionChanged -= PivotPanelSelectionChanged;
                }
                else if (PivotPointer is Pivot)
                {
                    ((Pivot)PivotPointer).SelectionChanged -= PivotPanelSelectionChanged;
                }
            }
        }

        private void WirePivotPanalSelectionChanged()
        {
            if (PivotPointer != null)
            {
                if (PivotPointer is Panorama)
                {
                    ((Panorama)PivotPointer).SelectionChanged += PivotPanelSelectionChanged;
                }
                else if (PivotPointer is Pivot)
                {
                    ((Pivot)PivotPointer).SelectionChanged += PivotPanelSelectionChanged;
                }
            }
        }*/

        #endregion

      /*  protected void OpenPopupDialog(object sender, PopupDialogEventArgs e)
        {
            var service = new DialogService(this);
            service.PopupDialogType = e.Type;
            service.Closed += (s, arg) => e.CloseAction.Invoke(s, arg);
            service.AnimationType = DialogService.AnimationTypes.Swivel;
            service.PopupDialogType = e.Type;
            e.PopupDialog.CloseAction = () => { service.Hide(); };
            service.Child = (FrameworkElement)e.PopupDialog;
            service.Opened += (s, arg) => OnPopupDialogOpened(s, e);
            service.Show();
        }

        protected virtual void OnPopupDialogOpened(object sender, PopupDialogEventArgs e)
        {
            var popup = e.PopupDialog as PhoneApplicationPopupBase;
            if (popup != null)
                popup.CreateAppBar();
        }*/

        protected virtual void RegisterApplicationBar()
        {

        }

        public static Transform GetOrientatedPopupTransform(PhoneApplicationPageBase pageBase)
        {
            TransformGroup tgroup = null;
            if (pageBase != null)
            {
                var orientation = pageBase.Orientation;
                if (orientation == PageOrientation.LandscapeLeft)
                {
                    tgroup = new TransformGroup();
                    tgroup.Children.Add(new RotateTransform() { Angle = 90 });
                    tgroup.Children.Add(new TranslateTransform() { X = ScreenWidth });
                }
                else if (orientation == PageOrientation.LandscapeRight)
                {
                    tgroup = new TransformGroup();
                    tgroup.Children.Add(new RotateTransform() { Angle = -90 });
                    tgroup.Children.Add(new TranslateTransform() { Y = ScreenHeight });
                }
            }
            return tgroup;
        }

        public bool IsInLandscapeMode()
        {
            bool landscape = false;

            if ((Orientation == PageOrientation.Landscape) ||
                (Orientation == PageOrientation.LandscapeLeft) ||
                (Orientation == PageOrientation.LandscapeRight))
            {
                landscape = true;
            }

            return landscape;
        }

       /* public virtual void AlertMessage(object sender, AlertMessageEventArgs e)
        {
            CloseAlertMessage();
            var popupMessageControl = new PopupMessageControl(this, e.Message)
                                          {
                                              Width = ActualWidth
                                          };

            _popup = new Popup
                         {
                             Child = popupMessageControl,
                             Height = 100,
                             Width = ActualWidth,
                             IsOpen = true
                         };


            _popup.RenderTransform = GetOrientatedPopupTransform(this);


            _popupTimer = new DispatcherTimer();
            _popupTimer.Interval = TimeSpan.FromSeconds(5);
            _popupTimer.Tick += (s, arg) =>
                                    {
                                        _popupTimer.Stop();
                                        CloseAlertMessage();

                                    };
            _popupTimer.Start();
        }*/

        protected virtual void EnableForProcessing() { }

        protected virtual void DisableForProcessing() { }

        public T ViewModel<T>() where T : ViewModelBase
        {
            return ModelView as T;
        }

        public ApplicationBarBuilder AppBarBuilder { get { return _applicationBarBuilder; } }

        protected Enums.ViewLifeStage ViewsLifeStage { get; set; }

        public string GetQueryStringValue(string key)
        {
            if (NavigationContext.QueryString != null)
            {
                return NavigationContext.QueryString[key];
            }

            throw new KeyNotFoundException(string.Format("The key provided {0} was not found in the query string", key));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PhoneApplicationPageBase()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ModelView != null)
                {
                   // ModelView.AsyncProcessStarted -= AsyncProcessStarted;
                   // ModelView.AsyncProcessFinished -= AsyncProcessFinished;
                   // ModelView.AlertMessage -= AlertMessage;
                    ModelView.NavigateToPage -= NavigateToPage;
                    ModelView.NavigateBack -= NavigateBack;
                }
            }
        }

        private void RemoveLastItemFromPageHistory()
        {
            var currentSource = NavigationService.CurrentSource.ToString();

            if (_pageHistory.Count > 0 && _pageHistory[_pageHistory.Count - 1] == currentSource)
            {
                _pageHistory.RemoveAt(_pageHistory.Count - 1);
            }
        }

      /*  private void CloseAlertMessage()
        {
            if (_popup == null) { return; }

            if (_popup.Child is PopupMessageControl)
            {
                _popup.IsOpen = false;
                _popup.Visibility = Visibility.Collapsed;
                _popup = null;
            }
        }*/

     /*   public virtual void AsyncProcessStarted(object sender, AsyncProcessingEventArgs e)
        {
            AsyncActionInProgress = true;
            Dispatcher.BeginInvoke(() =>
                                        {
                                            if (!_appbarHidden && ApplicationBar != null)
                                            {
                                                _appbarHidden = true; //used to insure in case the app bar was initialized during the async action
                                                _appbarVisible = ApplicationBar.IsVisible;
                                                ApplicationBar.IsVisible = false;
                                            }

                                            // if this is already up once no need to do it again
                                            if (_progressIndicatorPopup != null) { return; }

                                            var isLandscape = IsInLandscapeMode();
                                            var progressControl = new ProgressControl()
                                                                          {
                                                                              Width = isLandscape ? ScreenHeight : ScreenWidth,
                                                                              Height = isLandscape ? ScreenWidth : ScreenHeight
                                                                          };

                                            progressControl.Start(e.Message);
                                            progressControl.RenderTransform = GetOrientatedPopupTransform(this);

                                            _progressIndicatorPopup = new Popup
                                                         {
                                                             Child = progressControl,
                                                             Width = isLandscape ? ActualHeight : ActualWidth,
                                                             Height = isLandscape ? ActualWidth : ActualHeight,
                                                             VerticalAlignment = VerticalAlignment.Stretch,
                                                             HorizontalAlignment = HorizontalAlignment.Stretch,
                                                             IsOpen = true
                                                         };
                                        });
        }

        public virtual void AsyncProcessFinished(object sender, EventArgs e)
        {
            AsyncActionInProgress = false;
            Dispatcher.BeginInvoke(() =>
                                        {
                                            if (_appbarHidden && ApplicationBar != null)
                                            {
                                                ApplicationBar.IsVisible = _appbarVisible;
                                            }
                                            _appbarHidden = false;

                                            if (_progressIndicatorPopup == null) { return; }

                                            if (_progressIndicatorPopup.Child is ProgressControl)
                                            {
                                                _progressIndicatorPopup.IsOpen = false;
                                                _progressIndicatorPopup.Visibility = Visibility.Collapsed;
                                                _progressIndicatorPopup = null;
                                            }
                                        });

        }*/

        public static readonly DependencyProperty AnimationContextProperty = DependencyProperty.Register("LayoutContext", typeof(FrameworkElement), typeof(PhoneApplicationPageBase), new PropertyMetadata(null));
        public FrameworkElement LayoutContext
        {
            get
            {
                return (FrameworkElement)GetValue(AnimationContextProperty);
            }
            set
            {
                SetValue(AnimationContextProperty, value);
            }
        }
        /*
        public IAuthenticationProvider AuthenticationProvider
        {
            get
            {
                if (_authenticationProvider == null)
                {
                    if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual))
                    {
                        _authenticationProvider = ServiceLocator.Get<IAuthenticationProvider>();
                    }
                }

                return _authenticationProvider;
            }
        }*/

        public new IApplicationBar ApplicationBar
        {
            get { return base.ApplicationBar; }
            set { base.ApplicationBar = value; }
        }

        #region Shake validation

        protected void PlayShakeAnimation(UIElement target)
        {
            var storyboard = Application.Current.Resources["ShakeStoryboard"] as Storyboard;
            if (storyboard != null)
            {
                storyboard.SkipToFill(); // finish the previous run if it's active
                storyboard.Stop();
                foreach (var child in storyboard.Children)
                {
                    Storyboard.SetTarget(child, target);
                }
                Dispatcher.BeginInvoke(storyboard.Begin);
            }
        }

        protected virtual UIElement FindEmptyUIElement()
        {
            UIElement element = null;

           /* element = VisualTreeExtensions.GetVisualDescendants(this).OfType<Homecare.Mobile.Device.CommonUI.UserControls.ListPicker>().Where(
                p => p.SelectedItem.ToString() == String.Empty).FirstOrDefault();
            if (element == null)
                element = VisualTreeExtensions.GetVisualDescendants(this).OfType<TextBox>().Where(p => (string.IsNullOrEmpty(p.Text) && p.Tag != null && p.Tag.ToString() == Constants.RequiredConst)).FirstOrDefault();*/
            return element;
        }

        protected void ValidationFailed(object sender, System.EventArgs e)
        {
            var element = FindEmptyUIElement();
            if (element != null)
            {
                PlayShakeAnimation(element);
            }
        }

        #endregion

    }
}

