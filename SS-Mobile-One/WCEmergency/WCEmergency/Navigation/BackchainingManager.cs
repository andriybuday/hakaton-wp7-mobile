using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace WCEmergency.Navigation
{
    public static class BackchainingManager
    {
        public enum GoBackMode
        {
            Default = 0,
            BackNumberOfPages,
            BackToPage,
            Dashboard,
            Quit
        }

        public static GoBackMode CurrentMode { get; set; }

        public static void DeterminePageToReturnTo(NavigationEventArgs e)
        {
            switch (CurrentMode)
            {
                case GoBackMode.BackNumberOfPages:
                    if (--GoBackPageCount > 0 && CanGoBack)
                    {
                        GoBack();
                    }
                    else
                    {
                        CurrentMode = GoBackMode.Default;
                    }

                    break;

                case GoBackMode.Dashboard:
                    if (CanGoBack)
                    {
                        GoBack();
                    }
                    else
                    {
                        CurrentMode = GoBackMode.Default;
                    }

                    break;

                case GoBackMode.BackToPage:
                    var pageName = e.Content.ToString();
                    var periodPosition = pageName.LastIndexOf(".");

                    if (periodPosition > -1) { pageName = pageName.Substring(periodPosition + 1); }

                    if (CanGoBack && pageName != BackPageStop)
                    {
                        GoBack();
                    }
                    else
                    {
                        CurrentMode = GoBackMode.Default;
                    }

                    break;

                case GoBackMode.Quit:
                    GoBack();
                    break;

            }
        }

        public static void GoBack()
        {
            if (CanGoBack)
            {
                CurrentFrame.GoBack();
            }
        }

        public static void GoBack(int numberOfPages)
        {
            CurrentMode = GoBackMode.BackNumberOfPages;
            GoBackPageCount = numberOfPages;
            GoBack();
        }

        public static void GoBack(string pageName)
        {
            CurrentMode = GoBackMode.BackToPage;
            BackPageStop = pageName;
            GoBack();
        }

        public static void GoToDashboard()
        {
            CurrentMode = GoBackMode.Dashboard;
            GoBack();
        }

        public static void Quit()
        {
            //CurrentMode = GoBackMode.Quit;
            //GoBack();
            throw new Exception();
        }

        private static bool CanGoBack
        {
            get
            {
                return CurrentFrame != null ? CurrentFrame.CanGoBack : false;
            }
        }

        private static string BackPageStop { get; set; }



        private static int GoBackPageCount { get; set; }

        private static PhoneApplicationFrame CurrentFrame
        {
            get { return Application.Current.RootVisual as PhoneApplicationFrame; }
        }
    }
}
