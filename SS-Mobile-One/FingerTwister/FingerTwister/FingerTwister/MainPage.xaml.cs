using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace FingerTwister
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Popup popup;
        private BackgroundWorker backroungWorker;
        private Timer _playerTimer;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ShowPopup();
        }

        private bool alreadyStarted = false;

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void PhoneApplicationPage_Tap(object sender, GestureEventArgs e)
        {
            
        }

        private Color GetColor(int newRandomValue)
        {
            if (newRandomValue == 0)
                return Colors.Red;
            if (newRandomValue == 1)
                return Colors.Cyan;
            if (newRandomValue == 2)
                return Colors.Orange;
            if (newRandomValue == 3)
                return Colors.Green;

            throw new Exception("Not supported color");

        }


        private string GetText(int newRandomValue)
        {
            if (newRandomValue == 0)
                return "Thumb";
            if (newRandomValue == 1)
                return "Forefinger";
            if (newRandomValue == 2)
                return "Middle finger";
            if (newRandomValue == 3)
                return "Ring finger";
            if (newRandomValue == 4)
                return "Small finger";

            throw new Exception("Not supported finger");

        }

    

        private void UpdateImage()
        {
            Random Rnd = new Random();
            int RandImage = Rnd.Next(5) + 1;
            
            ImageSource imgsrc1 = new BitmapImage(new Uri(string.Format(@"Pictures/{0}.png", RandImage), UriKind.Relative));
            NextFinger.Source = imgsrc1;

            int m = Rnd.Next(4);
            SolidColorBrush myBrush = new SolidColorBrush(GetColor(m));
            NextColor.Background = myBrush;

            TextFinger.Text = GetText(m);
        }

        private void SafeCall(object obj)
        {
            Dispatcher.BeginInvoke((Action)UpdateImage, null);
        }

        

        private void Image_Hold(object sender, GestureEventArgs e)
        {
            
        }

        private void PhoneApplicationPage_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void PhoneApplicationPage_Hold(object sender, GestureEventArgs e)
        {
            if (alreadyStarted) return;
            alreadyStarted = true;
            _playerTimer = new Timer(SafeCall, null, 0, 10000);
        }

        private void PhoneApplicationPage_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        #region

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
            Thread.Sleep(5000);
        }

        #endregion
    }
}