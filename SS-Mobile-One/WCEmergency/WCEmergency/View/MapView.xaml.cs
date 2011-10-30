using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;
using WCEmergency.Common;
using WCEmergency.ViewModel;
using WCEmergency.WCServiceReference;

namespace WCEmergency.View
{
    public partial class MapView : PhoneApplicationPage
    {
        public MapView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var viewModel = LayoutRoot.DataContext as MapViewModel;
            if (viewModel != null)
            {
                viewModel.TargetToilet = ToiletContext.Instance.Item;
            }
        }


    }
}