using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using sdkSilverlightXNACS;
using sdkSilverlightXNACS.Models;
using sdkSilverlightXNACS.Storage;

namespace sdkPhotosCS
{
    public partial class AddMember : PhoneApplicationPage
    {

        ApplicationBarIconButton btnAccept;

        //Variables for the crop feature
        Point p1, p2;
        bool cropping = false;


        public AddMember()
        {
            InitializeComponent();

            textStatus.Text = "Select face of you hero with your finger.";

            //Sets the source to the Image control on the crop page to the WriteableBitmap object created previously.
            DisplayedImageElement.Source = App.CapturedImage;


            //Create event handlers for cropping selection on the picture.
            DisplayedImageElement.MouseLeftButtonDown += CropImage_MouseLeftButtonDown;
            DisplayedImageElement.MouseLeftButtonUp += CropImage_MouseLeftButtonUp;
            DisplayedImageElement.MouseMove += CropImage_MouseMove;


            //Used for rendering the cropping rectangle on the image.
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            //Creating an application bar and then setting visibility and menu properties.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            //This code creates the application bar icon buttons.
            btnAccept = new ApplicationBarIconButton(new Uri("/Icons/appbar.check.rest.png", UriKind.Relative));

            //Labels for the application bar buttons.
            btnAccept.Text = "Accept";

            //This code adds buttons to application bar.

            ApplicationBar.Buttons.Add(btnAccept);

            btnAccept.Click += btnAccept_Click;
           
            btnAccept.IsEnabled = false;

            //Begin storyboard for rectangle color effect.
            Rectangle.Begin();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (cropping)
            {

                rect.SetValue(Canvas.LeftProperty, (p1.X < p2.X) ? p1.X : p2.X);
                rect.SetValue(Canvas.TopProperty, (p1.Y < p2.Y) ? p1.Y : p2.Y);
                rect.Width = (int)Math.Abs(p2.X - p1.X);
                rect.Height = (int)Math.Abs(p2.Y - p1.Y);
            }
        }


        /// <summary>
        /// Click event handler for mouse move.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CropImage_MouseMove(object sender, MouseEventArgs e)
        {
            p2 = e.GetPosition(DisplayedImageElement);
        }

        /// <summary>
        /// Click event handler for mouse left button up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CropImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            p2 = e.GetPosition(DisplayedImageElement);
            cropping = false;
        }

        /// <summary>
        /// Click event handler for mouse left button down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void CropImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnAccept.IsEnabled = true;
            p1 = e.GetPosition(DisplayedImageElement);
            p2 = p1;
            rect.Visibility = Visibility.Visible;
            cropping = true;
        }


        /// <summary>
        /// Click event handler for the accept button on the application bar.
        /// Saves cropped image to isolated storage, then into
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void btnAccept_Click(object sender, EventArgs e)
        {
            CropImage();
        }


        void CropImage()
        {
            // Get the size of the source image captured by the camera
            double originalImageWidth = App.CapturedImage.PixelWidth;
            double originalImageHeight = App.CapturedImage.PixelHeight;


            // Get the size of the image when it is displayed on the phone
            double displayedWidth = DisplayedImageElement.ActualWidth;
            double displayedHeight = DisplayedImageElement.ActualHeight;

            // Calculate the ratio of the original image to the displayed image
            double widthRatio = originalImageWidth / displayedWidth;
            double heightRatio = originalImageHeight / displayedHeight;

            // Create a new WriteableBitmap. The size of the bitmap is the size of the cropping rectangle
            // drawn by the user, multiplied by the image size ratio.
            App.CroppedImage = new WriteableBitmap((int)(widthRatio * Math.Abs(p2.X - p1.X)), (int)(heightRatio * Math.Abs(p2.Y - p1.Y)));


            // Calculate the offset of the cropped image. This is the distance, in pixels, to the top left corner
            // of the cropping rectangle, multiplied by the image size ratio.
            int xoffset = (int)(((p1.X < p2.X) ? p1.X : p2.X) * widthRatio);
            int yoffset = (int)(((p1.Y < p2.Y) ? p1.Y : p2.X) * heightRatio);

            // Copy the pixels from the targeted region of the source image into the target image, 
            // using the calculated offset
            for (int i = 0; i < App.CroppedImage.Pixels.Length; i++)
            {
                int x = (int)((i % App.CroppedImage.PixelWidth) + xoffset);
                int y = (int)((i / App.CroppedImage.PixelWidth) + yoffset);
                App.CroppedImage.Pixels[i] = App.CapturedImage.Pixels[y * App.CapturedImage.PixelWidth + x];
            }

            // Set the source of the image control to the new cropped bitmap
            DisplayedImageElement.Source = App.CroppedImage;
            rect.Visibility = Visibility.Collapsed;
          
            var userEnteredName = NameTextBox.Text;

            var game = GameState.GetInstance();
            game.FriendsTeam.Add(new Hero() { IsInYourTeam = true, MemberPhoto = App.CroppedImage, Name = userEnteredName });

            NavigationService.Navigate(new Uri("/Team.xaml", UriKind.Relative));
        }



    }
}