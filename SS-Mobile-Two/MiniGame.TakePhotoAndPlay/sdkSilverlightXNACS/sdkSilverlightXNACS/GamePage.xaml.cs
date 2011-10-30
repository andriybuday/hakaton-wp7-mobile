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
using System.Threading;
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

        Texture2D texture;

        // A variety of rectangle colors
        Texture2D bombTexture;
        private Timer _timer;

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
            bombTexture = contentManager.Load<Texture2D>("Bomb");

            LoadGameWorld();

            // Start the timer
            timer.Start();

            _timer = new Timer(PingServiceCallBack,null,0,2500);

            base.OnNavigatedTo(e);
        }

        private void PingServiceCallBack(object state)
        {
            //TODO: call service to get status...
            //call LoseGame or WinGame accordingly... 
        }

        private void AddBombTwoTheWorld()
        {
            RandomlyGenerateBall(BallIs.Bomb);
        }

        private void LoadGameWorld()
        {

            // remove all players
            balls.Clear();
            // no status about game here... yet
            ControlGameStatus.Visibility = Visibility.Collapsed;
            ImageFinishedWithBomb.Visibility = Visibility.Collapsed;
            AddBombTwoTheWorld();

            if (GameState.GetInstance().FriendsTeam.Count < 1 || GameState.GetInstance().EnemyTeam.Count < 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    RandomlyGenerateBall(BallIs.Friend);
                }
                for (int i = 0; i < 4; i++)
                {
                    RandomlyGenerateBall(BallIs.Enemy);
                }
            }
            else // there are players...
            {
                ReloadTeams();
            }

            if (GameState.GetInstance().IsMultiPlayerGame.HasValue && GameState.GetInstance().IsMultiPlayerGame.Value)
            {
                ButtonPlayAgain.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonPlayAgain.Visibility = Visibility.Visible;
            }
        }

        private void ReloadTeams()
        {
            // load friends
            var teamMembers = GameState.GetInstance().FriendsTeam;
            foreach (var teamMember in teamMembers)
            {
                var newBallTexture = FromBitmapToTexture2D(teamMember.MemberPhoto);
                Color ballColor = Color.White;
                Vector2 velocity = new Vector2((_random.NextDouble() > .5 ? -1 : 1) * _random.Next(3), (_random.NextDouble() > .5 ? -1 : 1) * _random.Next(3)) + Vector2.UnitX + Vector2.UnitY;
                Vector2 center = new Vector2((float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2, (float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 2);

                var ball = new Ball(ballColor, newBallTexture, center, velocity, 50f);
                ball.BallIs = BallIs.Friend;
                balls.Add(ball);
            }

            // load enemies
            teamMembers = GameState.GetInstance().EnemyTeam;
            foreach (var teamMember in teamMembers)
            {
                var newBallTexture = FromBitmapToTexture2D(teamMember.MemberPhoto);
                Color ballColor = Color.White;
                Vector2 velocity = new Vector2((_random.NextDouble() > .5 ? -1 : 1) * _random.Next(3), (_random.NextDouble() > .5 ? -1 : 1) * _random.Next(3)) + Vector2.UnitX + Vector2.UnitY;
                Vector2 center = new Vector2((float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - 100, (float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 100);

                var ball = new Ball(ballColor, newBallTexture, center, velocity, 50f);
                ball.BallIs = BallIs.Enemy;
                balls.Add(ball);
            }
        }

        private Texture2D FromBitmapToTexture2D(byte[] faceToUse)
        {
            using (var face1FileStream = new MemoryStream())
            {
                face1FileStream.Write(faceToUse, 0, faceToUse.Count());
                face1FileStream.Flush();
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
            UpdateBalls();
            //HandleTouches();
            HandleTouches_HoldAndKickBall();
            //HandleForRemove();
        }

        private void UpdateBalls()
        {
            int enemyCounter = 0;
            int friendsCounter = 0;

            Ball toRemoveBall = null;
            foreach (Ball ball in balls)
            {
                GameState.GetInstance().IsGameStarted = true;

                if (ball.BallIs == BallIs.IsOutsideOfBoard)
                {
                    toRemoveBall = ball;
                }
                else if (ball.BallIs == BallIs.Friend)
                {
                    friendsCounter++;
                }
                else if (ball.BallIs == BallIs.Enemy)
                {
                    enemyCounter++;
                }
                // UPDATE
                ball.Update();
            }
            // REMOVE
            if (toRemoveBall != null)
            {
                // TODO: add call to the server side...
                balls.Remove(toRemoveBall);
                _soundPuckHit.Play();
            }

            ucScoreBoard1.BlueScore = friendsCounter;
            ucScoreBoard1.RedScore = enemyCounter;

            if (GameState.GetInstance().IsGameStarted)
            {
                if (enemyCounter == 0 && friendsCounter > 0)
                {
                    WinGame();
                }
                else if (friendsCounter == 0 && enemyCounter > 0)
                {
                    LoseGame();
                }
            }

        }
        #region Finish game.. .
        private void WinGame()
        {
            // game just finished or not started at all...
            GameState.GetInstance().IsGameOver = true;
            GameState.GetInstance().IsGameStarted = false;

            ControlGameStatus.IsVictory = true;
            ControlGameStatus.Visibility = Visibility.Visible;
            balls.Clear();
        }

        private void LoseGame()
        {
            // game just finished or not started at all...
            GameState.GetInstance().IsGameOver = true;
            GameState.GetInstance().IsGameStarted = false;

            ControlGameStatus.IsVictory = false;
            ControlGameStatus.Visibility = Visibility.Visible;
            balls.Clear();
        }

        private void FinishGameWithBomb()
        {
            balls.Clear();
            GameState.GetInstance().IsGameOver = true;
            GameState.GetInstance().IsGameStarted = false;
            ImageFinishedWithBomb.Visibility = Visibility.Visible;
            ControlGameStatus.IsVictory = false;
            ControlGameStatus.Visibility = Visibility.Visible;
        }
        #endregion Finish game.. .

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
                        if(_catchedOne.BallIs == BallIs.Bomb)
                        {
                            FinishGameWithBomb();
                        }

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



        Random _random = new Random(DateTime.Now.Millisecond);
        private void RandomlyGenerateBall(BallIs ballIs)
        {
            // nothing was catched, let's generate void...

            Vector2 velocity =
                new Vector2((_random.NextDouble() > .5 ? -1 : 1) * _random.Next(5),
                            (_random.NextDouble() > .5 ? -1 : 1) * _random.Next(4)) + Vector2.UnitX + Vector2.UnitY;

            Vector2 center = new Vector2((float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - 100,
                                         (float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 100);
            float radius = 50f;


            Color ballColor = Color.Red;
            if (ballIs == BallIs.Friend)
            {
                ballColor = Color.Blue;
                center = new Vector2((float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2,
                                         (float)SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 2);
            }
            var texture = ballTexture;
            if (ballIs == BallIs.Bomb)
            {
                texture = bombTexture;
                ballColor = Color.White;
            }
            var ball = new Ball(ballColor, texture, center, velocity, radius);

            ball.BallIs = ballIs;
            balls.Add(ball);
        }

        private Ball GetCatchedBall(Vector2 position)
        {
            foreach (var ball in balls)
            {
                var xDiff = Math.Abs(ball.CenterPosition.X - position.X);
                var yDiff = Math.Abs(ball.CenterPosition.Y - position.Y);

                if (xDiff < 50 && yDiff < 50)
                {
                    return ball;
                }
            }
            return null;
        }


        public void DrawLine()
        {
            if (_catchedOne != null && _catchedOne.IsOnHold && _catchedOne.BallIs == BallIs.Enemy)
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

                if (ContentPanelCanvas.Children.Count > 4)
                {
                    this.ContentPanelCanvas.Children.RemoveAt(ContentPanelCanvas.Children.Count - 1);
                }
                this.ContentPanelCanvas.Children.Add(line);
            }
            else
            {
                if (ContentPanelCanvas.Children.Count > 4)
                {
                    this.ContentPanelCanvas.Children.RemoveAt(ContentPanelCanvas.Children.Count - 1);
                }
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

            // Draw all players..
            foreach (Ball ball in balls)
            {
                ball.Draw(spriteBatch);
            }

            // Using the texture from the UIElementRenderer, 
            // draw the Silverlight controls to the screen
            spriteBatch.Draw(elementRenderer.Texture, Vector2.Zero, Color.White);

            spriteBatch.End();

            // Drawing Silverlight

            DrawLine();
        }


        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            LoadGameWorld();
        }
    }
}
