using System;
using System.Windows;

namespace GpsEmulator.DialogWindows
{
    /// <summary>
    /// Interaction logic for OptionsDialog.xaml
    /// </summary>
    public partial class OptionsDialog : Window
    {
        bool cancelled = false;

        public bool Cancelled
        {
            get { return cancelled; }
        }

        public OptionsDialog()
        {
            InitializeComponent();
            cmbPathColor.ItemsSource = new string[] { "Red", "Blue", "Green" };
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
