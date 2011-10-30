using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using WCEmergency.View;

namespace WCEmergency
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Popup popup;
        private BackgroundWorker backroungWorker;
        // Constructor
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ShowPopup();
        }

        private void OnMapClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(Constants.MapView, UriKind.Relative));
        }

        #region Splash

        private void ShowPopup()
        {
            this.popup = new Popup();
            this.popup.Child = new PopupSplash();
            this.popup.IsOpen = true;
            StartLoadingData();
        }

        private void StartLoadingData()
        {
            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);
            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;

            }
            );
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do some data loading on a background
            // We'll just sleep for the demo
            Thread.Sleep(7000);
        }
        #endregion
    }
}