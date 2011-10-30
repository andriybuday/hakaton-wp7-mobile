using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;
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

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.Content is ObservableCollection<Toilet>)
            {
                (LayoutRoot.DataContext as MapViewModel).Toilets = (ObservableCollection<Toilet>) e.Content;
            }
        }
    }
}