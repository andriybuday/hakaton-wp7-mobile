using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone;

namespace sdkSilverlightXNACS
{
    public partial class AddMemberPhoto : PhoneApplicationPage
    {
        CameraCaptureTask _ctask;
        private bool _photoIsCaptured;
        private string _queryString = string.Empty;

        public AddMemberPhoto()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
           
            _photoIsCaptured = false;            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Uri.OriginalString) && e.Uri.OriginalString.Contains("?"))
            {
                _queryString = e.Uri.OriginalString.Substring(e.Uri.OriginalString.IndexOf("?"));
            }

            if (_photoIsCaptured)
            {
                _photoIsCaptured = false;
                NavigationService.Navigate(new Uri(string.Format("/AddMember.xaml{0}", _queryString), UriKind.Relative));
                return;
            }
            if (_ctask == null)
            {
                _ctask = new CameraCaptureTask();
                _ctask.Completed += CtaskCompleted;
            }
            _ctask.Show();
        }

        
        void CtaskCompleted(object sender, PhotoResult e)
        {
            if (e.ChosenPhoto != null)
            {
                App.CapturedImage = PictureDecoder.DecodeJpeg(e.ChosenPhoto);
                progressBar1.Visibility = Visibility.Collapsed;
                MainImage.Source = App.CapturedImage;
                _photoIsCaptured = true;
            }
            else
            {
                NavigationService.GoBack();
            }
        }
    }
}
