using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WCEmergency.ViewModel;

namespace WCEmergency.View
{
    public partial class ToiletView : PhoneApplicationPage
    {
        public ToiletView()
        {
            InitializeComponent();

            var viewModel = LayoutRoot.DataContext as ToiletViewModel;
            if (viewModel != null)
            {
                viewModel.ToiletChoosen += ViewModelToiletChoosen;
            }
        }

        private void ViewModelToiletChoosen(object sender, NavigationEventArgs e)
        {
            NavigationService.Navigate(new Uri(Constants.MapView, UriKind.Relative)); 
        }
    }
}