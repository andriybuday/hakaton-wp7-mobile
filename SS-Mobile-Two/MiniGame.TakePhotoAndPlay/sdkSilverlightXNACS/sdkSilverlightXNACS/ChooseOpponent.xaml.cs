using System.Windows;
using Microsoft.Phone.Controls;

namespace sdkSilverlightXNACS
{
    public partial class ChooseOpponent : PhoneApplicationPage
    {
       
        // Constructor
        public ChooseOpponent()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;         
        }

        private void Search_Phones_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Find phones
        }
    }
}
