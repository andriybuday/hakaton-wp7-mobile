using System.Diagnostics;
using System.Windows;

namespace GpsEmulator.DialogWindows
{
    /// <summary>
    /// Interaction logic for BingApiKeyDialog.xaml
    /// </summary>
    public partial class BingApiKeyDialog : Window
    {
        public bool cancelled = true;
        public bool Cancelled
        {
            get { return cancelled; }
        }

        public BingApiKeyDialog()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            cancelled = false;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
