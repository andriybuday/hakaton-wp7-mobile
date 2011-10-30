/* 
    Copyright (c) 2011 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604 
  
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BouncingBalls;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Spritehand.FarseerHelper;
using sdkSilverlightXNACS.Storage;
using Color = Microsoft.Xna.Framework.Color;
using Point = System.Windows.Point;

namespace sdkSilverlightXNACS
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        #region balls stuff
        IList<Ball> balls = new List<Ball>();
        SpriteBatch spriteBatchBall;
        Texture2D ballTexture;
        bool touching = false;
        #endregion balls stuff

        List<Texture2D> faces = new List<Texture2D>();
        List<Vector2> facePositions = new List<Vector2>();
        List<Vector2> faceSpeeds = new List<Vector2>();

        Texture2D texture;
        Texture2D textureFace2;
        Vector2 spritePosition;
        Vector2 spriteSpeed = new Vector2(200.0f, 200.0f);

        // A variety of rectangle colors
        Texture2D redTexture;
        Texture2D greenTexture;
        Texture2D blueTexture;

        // For rendering the XAML onto a texture
        UIElementRenderer elementRenderer;
        private SoundMain _soundPuckHit;
        private Ball _catchedOne;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            //timer.UpdateInterval = TimeSpan.FromTicks(333333);

            // Using TimeSpan.Zero causes the update to happen 
            // at the actual framerate of the device. This makes 
            // animation much smoother. However, this will cause 
            // the speed of the app to vary from device to device 
            // where a fixed UpdateInterval will not.
            timer.UpdateInterval = TimeSpan.Zero;
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            // Use the LayoutUpdate event to know when the page layout 
            // has completed so we can create the UIElementRenderer
            LayoutUpdated += new EventHandler(GamePage_LayoutUpdated);
        }


        void GamePage_LayoutUpdated(object sender, EventArgs e)
        {
            // Create the UIElementRenderer to draw the XAML page to a texture.

            // Check for 0 because when we navigate away the LayoutUpdate event
            // is raised but ActualWidth and ActualHeight will be 0 in that case.
            if (ActualWidth > 0 && ActualHeight > 0 && elementRenderer == null)
            {
                elementRenderer = new UIElementRenderer(this, (int)ActualWidth, (int)ActualHeight);
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
            _soundPuckHit = new SoundMain(new Canvas(), "sounds/hitPuck.wav", 2, 0);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);


            ballTexture = contentManager.Load<Texture2D>("Ball");
            RandomlyGenerateBall();
            RandomlyGenerateBall();
            RandomlyGenerateBall();

            // If texture is null, we've never loaded our content.
            if (null == texture)
            {

                //redTexture = contentManager.Load<Texture2D>("redRect");
                greenTexture = contentManager.Load<Texture2D>("greenRect");
                blueTexture = contentManager.Load<Texture2D>("blueRect");
                // default, will be overriden
                texture = greenTexture;

                SetFaceTexture(Faces.Face1);
            }

            spritePosition.X = 0;
            spritePosition.Y = 0;

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        private void SetFaceTexture(Faces faces)
        {
            var teamMembers = GameState.GetInstance().FriendsTeam;

            foreach (var teamMember in teamMembers)
            {
                var newBallTexture = FromBitmapToTexture2D(teamMember.MemberPhoto);

                Random random = new Random(DateTime.Now.Millisecond);
                //Color ballColor = new Color(random.Next(255), random.Next(255), random.Next(255));
                Color ballColor = Color.White;
                Vector2 velocity = new Vector2((random.NextDouble() > .5 ? -1 : 1) * random.Next(3), (random.NextDouble() > .5 ? -1 : 1) * random.Next(3)) + Vector2.UnitX + Vector2.UnitY;
                Vector2 center = new Vector2((float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2, (float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 2);
                float radius = 5f * (float)random.NextDouble() + 65f;

                balls.Add(new Ball(ballColor, newBallTexture, center, velocity, radius));
            }
        }

        private Texture2D FromBitmapToTexture2D(WriteableBitmap faceToUse)
        {
            using (var face1FileStream = new MemoryStream())
            {
                Extensions.SaveJpeg(faceToUse, face1FileStream, faceToUse.PixelWidth, faceToUse.PixelHeight, 0, 85);
                var result = Texture2D.FromStream(SharedGraphicsDeviceManager.Current.GraphicsDevice, face1FileStream);
                face1FileStream.Close();
                return result;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }


        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // Move the sprite around.
            UpdateSprite(e);
            UpdateBalls();
            //HandleTouches();
            HandleTouches_HoldAndKickBall();
            //HandleForRemove();
        }

        private void UpdateBalls()
        {
            Ball toRemoveBall = null;
            foreach (Ball ball in balls)
            {
                if (ball.IsOutsideOfBoard)
                {

                    toRemoveBall = ball;
                }

                ball.Update();
            }
            if (toRemoveBall != null)
            {
                // TODO: add call to the server side...
                balls.Remove(toRemoveBall);
                _soundPuckHit.Play();
            }
        }

        private void HandleTouches_GenerateNewBalls()
        {
            TouchCollection touches = TouchPanel.GetState();
            if (!touching && touches.Count > 0)
            {
                touching = true;

                Random random = new Random(DateTime.Now.Millisecond);
                Color ballColor = new Color(random.Next(255), random.Next(255), random.Next(255));
                Vector2 velocity = new Vector2((random.NextDouble() > .5 ? -1 : 1) * random.Next(9), (random.NextDouble() > .5 ? -1 : 1) * random.Next(9)) + Vector2.UnitX + Vector2.UnitY;
                Vector2 center = new Vector2((float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2, (float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 2);
                float radius = 5f * (float)random.NextDouble() + 65f;
                balls.Add(new Ball(ballColor, ballTexture, center, velocity, radius));
            }
            else if (touches.Count == 0)
            {
                touching = false;
            }
        }

        private void HandleTouches_HoldAndKickBall()
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State == TouchLocationState.Pressed)
                {
                    _catchedOne = GetCatchedBall(tl.Position);
                    if (_catchedOne != null)
                    {
                        _catchedOne.IsOnHold = true;
                        _catchedOne.PressedTopLeftPosition = _catchedOne.TopLeftPosition;
                        _catchedOne.PressedPosition = tl.Position;
                        _catchedOne.HoldingPosition = tl.Position;
                    }
                }
                else if (tl.State == TouchLocationState.Moved)
                {
                    if (_catchedOne != null)
                    {
                        _catchedOne.HoldingPosition = tl.Position;
                    }
                }
                else if (tl.State == TouchLocationState.Released)
                {
                    if (_catchedOne != null)
                    {

                        _catchedOne.IsOnHold = false;
                    }
                }
            }
        }

        private void RandomlyGenerateBall()
        {
            // nothing was catched, let's generate void...
            Random random = new Random(DateTime.Now.Millisecond);
            Color ballColor = new Color(random.Next(255), random.Next(255), random.Next(255));
            Vector2 velocity =
                new Vector2((random.NextDouble() > .5 ? -1 : 1) * random.Next(3),
                            (random.NextDouble() > .5 ? -1 : 1) * random.Next(3)) + Vector2.UnitX + Vector2.UnitY;
            Vector2 center = new Vector2((float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2,
                                         (float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 2);
            float radius = 5f * (float)random.NextDouble() + 65f;
            balls.Add(new Ball(ballColor, ballTexture, center, velocity, radius));
        }

        private Ball GetCatchedBall(Vector2 position)
        {
            foreach (var ball in balls)
            {
                var xDiff = Math.Abs(ball.CenterPosition.X - position.X);
                var yDiff = Math.Abs(ball.CenterPosition.Y - position.Y);

                if (xDiff < 65 && yDiff < 65)
                {
                    return ball;
                }
            }
            return null;
        }


        public void DrawLine()
        {
            if(_catchedOne != null && _catchedOne.IsOnHold)
            {
                var r = _catchedOne.radius;

                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.White);
                line.StrokeThickness = 5;

                Point point1 = new Point();
                //var prevCenter = _catchedOne.GetCenterLocation(_catchedOne.PressedTopLeftPosition);
                var prevCenter = _catchedOne.PressedTopLeftPosition;
                //var center = _catchedOne.GetCenterLocation(_catchedOne.TopLeftPosition);
                var center = _catchedOne.TopLeftPosition;

                point1.X = prevCenter.X + r;
                point1.Y = prevCenter.Y;

                Point point2 = new Point();
                point2.X = center.X + r;
                point2.Y = center.Y;

                line.X1 = point1.X;
                line.Y1 = point1.Y;
                line.X2 = point2.X;
                line.Y2 = point2.Y;

                if(ContentPanelCanvas.Children.Count > 3)
                {
                    this.ContentPanelCanvas.Children.RemoveAt(ContentPanelCanvas.Children.Count - 1);
                }
                this.ContentPanelCanvas.Children.Add(line);
            }
            else
            {
                if (ContentPanelCanvas.Children.Count > 3)
                {
                    this.ContentPanelCanvas.Children.RemoveAt(ContentPanelCanvas.Children.Count - 1);
                }
            }
        }

        /// <summary>
        /// Moves the rectangle around the screen.
        /// </summary>
        /// <param name="e"></param>
        void UpdateSprite(GameTimerEventArgs e)
        {
            // Move the sprite by speed, scaled by elapsed time.
            spritePosition += spriteSpeed * (float)e.ElapsedTime.TotalSeconds;

            int MinX = 0;
            int MinY = 0;
            int MaxX = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - texture.Width;
            int MaxY = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - texture.Height;

            // Check for bounce.
            if (spritePosition.X > MaxX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }

            else if (spritePosition.X < MinX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }

            if (spritePosition.Y > MaxY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MaxY;
            }
            else if (spritePosition.Y < MinY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MinY;
            }

        }


        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);

            // Render the Silverlight controls using the UIElementRenderer
            elementRenderer.Render();

            // Draw the sprite
            spriteBatch.Begin();

            // Draw the rectangle in its new position
            spriteBatch.Draw(texture, spritePosition, Color.White);

            foreach (Ball ball in balls)
            {
                ball.Draw(spriteBatch);
            }

            //base.Draw(gameTime);

            // Using the texture from the UIElementRenderer, 
            // draw the Silverlight controls to the screen
            spriteBatch.Draw(elementRenderer.Texture, Vector2.Zero, Color.White);

            spriteBatch.End();

            // Drawing Silverlight

            DrawLine();
        }


        /// <summary>
        /// Toggle the visibility of the StackPanel named "ColorPanel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorPanelToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Visibility.Visible == ColorPanel.Visibility)
            {
                ColorPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                ColorPanel.Visibility = System.Windows.Visibility.Visible;
            }
        }


        /// <summary>
        /// Switches to the red rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redButton_Click(object sender, RoutedEventArgs e)
        {
            //texture = redTexture;

            SetFaceTexture(Faces.Face2);
        }

        public enum Faces
        {
            Face1,
            Face2
        }

        /// <summary>
        /// Switches to the green rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void greenButton_Click(object sender, RoutedEventArgs e)
        {
            //texture = greenTexture;
            SetFaceTexture(Faces.Face1);
        }


        /// <summary>
        /// Switches to the blue rectangle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blueButton_Click(object sender, RoutedEventArgs e)
        {
            texture = blueTexture;
        }
    }
}
