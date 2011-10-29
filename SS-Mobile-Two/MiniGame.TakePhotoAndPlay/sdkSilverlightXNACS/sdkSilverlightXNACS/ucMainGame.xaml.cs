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
using System.Windows.Shapes;
using Spritehand.FarseerHelper;
using FarseerGames.FarseerPhysics.Mathematics;
using System.ComponentModel;

namespace ShuffleBoard
{
    public partial class ucMainGame : UserControl
    {
        PhysicsControllerMain _physicsController;
        List<PhysicsSprite> _pucks = new List<PhysicsSprite>();
        Point _ptStartShot;
        bool _draggingPuck;
        int _draggingStartTick;
        PhysicsSprite _puckDragging = null;

        int _currentPuckToShoot = 0;
        int _currentPuckShot = -1;
        int _gameOverScore = 15;

        SoundMain _soundPuckHit;


        public ucMainGame()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ucMainGame_Loaded);
            imgSplash.Visibility = Visibility.Visible;

            // ******************
            // ** METRICS
            // ******************
            Application.Current.Host.Settings.MaxFrameRate = 30;
            //Application.Current.Host.Settings.EnableCacheVisualization = true;
            //Application.Current.Host.Settings.EnableFrameRateCounter = true;
            //Application.Current.Host.Settings.EnableRedrawRegions = true;

        }


        void ucMainGame_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _physicsController = cnvTable.GetValue(PhysicsControllerMain.PhysicsControllerProperty) as PhysicsControllerMain;
            _physicsController.Initialized += new PhysicsControllerMain.InitializedHandler(_physicsController_Initialized);
            _physicsController.Collision += new PhysicsControllerMain.CollisionHandler(_physicsController_Collision);
            _physicsController.TimerLoop += new PhysicsControllerMain.TimerLoopHandler(_physicsController_TimerLoop);

#if WINDOWS_PHONE
            _soundPuckHit = new SoundMain(this.LayoutRoot, "sounds/hitPuck.wav", 2, 0);
#else
            _soundPuckHit = new SoundMain(this.LayoutRoot, "/sounds/hitPuck.wma", 2, 0);
#endif
        }

        void _physicsController_Initialized(object source)
        {
            _pucks.Add(_physicsController.PhysicsObjects["Puck"]);      // blue
            _pucks.Add(_physicsController.PhysicsObjects["Puck_3"]);    // red
            _pucks.Add(_physicsController.PhysicsObjects["Puck_1"]);    // blue
            _pucks.Add(_physicsController.PhysicsObjects["Puck_4"]);    // red
            _pucks.Add(_physicsController.PhysicsObjects["Puck_2"]);    // blue
            _pucks.Add(_physicsController.PhysicsObjects["Puck_5"]);    // red

            foreach (PhysicsSprite puck in _pucks)
            {
                puck.BodyObject.LinearDragCoefficient = 0.4F;
                puck.GeometryObject.RestitutionCoefficient = 1.5F;
                puck.BodyObject.Enabled = false;
                puck.Visibility = Visibility.Collapsed;

#if WINDOWS_PHONE
                puck.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(puck_ManipulationStarted);
                puck.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(puck_ManipulationCompleted);
#else
                puck.MouseLeftButtonDown += new MouseButtonEventHandler(puck_MouseLeftButtonDown);
                puck.MouseMove += new MouseEventHandler(puck_MouseMove);
                puck.MouseLeftButtonUp += new MouseButtonEventHandler(puck_MouseLeftButtonUp);
#endif
            }

#if WINDOWS_PHONE
            cnvTable.MouseMove += new MouseEventHandler(cnvTable_MouseMove);
#endif

            sbRotateTable.Begin();

            imgSplash.Visibility = Visibility.Collapsed;

            SetupThePuck();
        }

        void _physicsController_Collision(string sprite1, string sprite2)
        {
            // check for puck off sides
            if (sprite1.StartsWith("rectSensor") && sprite2.StartsWith("Puck"))
            {
                ucPuck puck = GetPuckControl(sprite2);

                _physicsController.PhysicsObjects[sprite2].BodyObject.Enabled = false;
                puck.sbLostPuck.Begin();
            }

            // check for puck to puck collsion
            if (sprite1.StartsWith("Puck") && sprite2.StartsWith("Puck"))
            {
                PhysicsSprite sprite = _physicsController.PhysicsObjects[sprite1];

                if (Math.Abs(sprite.BodyObject.LinearVelocity.X) > 10
                    || Math.Abs(sprite.BodyObject.LinearVelocity.Y) > 10)
                {
                    _soundPuckHit.Play();
                }
            }
        }

        void _physicsController_TimerLoop(object source)
        {
            // check to see if the current shot is completed
            if (_currentPuckShot >= 0)
            {
                if (_pucks[_currentPuckShot].BodyObject.Enabled == false ||
                    Math.Abs(_pucks[_currentPuckShot].BodyObject.LinearVelocity.X) < 3 && Math.Abs(_pucks[_currentPuckShot].BodyObject.LinearVelocity.Y) < 3)
                {
                    // did the shot clear the end zone?
                    if (!PointWithinBounds(new Point(_pucks[_currentPuckShot].BodyObject.Position.X, _pucks[_currentPuckShot].BodyObject.Position.Y)))
                    {
                        _currentPuckShot = -1;
                        _currentPuckToShoot++;
                        SetupThePuck();
                    }
                }
            }
        }

        void SetupThePuck()
        {
            // Is this the end of a round?
            if (_currentPuckToShoot > 5)
            {
                // tally the score
                var pucks = from p in _pucks
                            where p.BodyObject.Enabled == true
                            orderby p.BodyObject.Position.Y
                            select p;

                List<PhysicsSprite> puckSorted = pucks.ToList();

                if (puckSorted.Count > 0)
                {
                    ucPuck puckCtl = GetPuckControl(puckSorted[0].Name);

                    bool isBlueWinner = puckCtl.Name.StartsWith("blue");

                    int score = 0;
                    foreach (PhysicsSprite puck in puckSorted)
                    {
                        puckCtl = GetPuckControl(puck.Name);

                        if (isBlueWinner && puckCtl.Name.StartsWith("red"))
                            break;
                        if (!isBlueWinner && puckCtl.Name.StartsWith("blue"))
                            break;

                        score += GetPointsForPuck(puck);
                    }

                    if (isBlueWinner)
                        ucScoreBoard1.BlueScore += score;
                    else
                        ucScoreBoard1.RedScore += score;

                    if (ucScoreBoard1.BlueScore >= _gameOverScore || ucScoreBoard1.RedScore >= _gameOverScore)
                    {
                        if (ucScoreBoard1.BlueScore >= _gameOverScore)
                            MessageBox.Show("BLUE WINS!!!", "GAME OVER", MessageBoxButton.OK);
                        if (ucScoreBoard1.RedScore >= _gameOverScore)
                            MessageBox.Show("RED WINS!!!", "GAME OVER", MessageBoxButton.OK);

                        ucScoreBoard1.BlueScore = 0;
                        ucScoreBoard1.RedScore = 0;
                    }

                }

                // reset the pucks
                foreach (PhysicsSprite puck in _pucks)
                {
                    puck.BodyObject.ClearForce();
                    puck.BodyObject.ClearImpulse();
                    puck.BodyObject.ClearTorque();
                    puck.BodyObject.LinearVelocity = new Vector2(0, 0);
                    puck.BodyObject.Position = new Vector2(240, 700);

                    puck.BodyObject.Enabled = false;
                    puck.Visibility = Visibility.Collapsed;
                    ucPuck puckCtl = GetPuckControl(puck.Name);
                    puckCtl.sbLostPuck.Stop();
                }


                _currentPuckToShoot = 0;
            }

            _pucks[_currentPuckToShoot].Visibility = Visibility.Visible;
            _pucks[_currentPuckToShoot].BodyObject.Enabled = true;
            _pucks[_currentPuckToShoot].BodyObject.Position = new Vector2(240, 700);

            // is this a blue or a red puck?
            if (_currentPuckToShoot % 2 == 0)
                ucPlayerUp1.IsBlueTurn = true;
            else
                ucPlayerUp1.IsBlueTurn = false;

            ucPlayerUp1.sbFadeIn.Begin();
        }

        ucPuck GetPuckControl(string spriteName)
        {
            ucPuck puck;
            switch (spriteName)
            {
                case "Puck":
                    puck = PhysicsControllerMain.ParentCanvas.FindName("bluePuck1") as ucPuck;
                    break;
                case "Puck_1":
                    puck = PhysicsControllerMain.ParentCanvas.FindName("bluePuck2") as ucPuck;
                    break;
                case "Puck_2":
                    puck = PhysicsControllerMain.ParentCanvas.FindName("bluePuck3") as ucPuck;
                    break;
                case "Puck_3":
                    puck = PhysicsControllerMain.ParentCanvas.FindName("redPuck1") as ucPuck;
                    break;
                case "Puck_4":
                    puck = PhysicsControllerMain.ParentCanvas.FindName("redPuck2") as ucPuck;
                    break;
                default:
                    puck = PhysicsControllerMain.ParentCanvas.FindName("redPuck3") as ucPuck;
                    break;
            }

            return puck;
        }

        void puck_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // is this the current puck to shoot?
            if (sender != _pucks[_currentPuckToShoot])
                return;

            _ptStartShot = e.GetPosition(cnvTable);

            if (!PointWithinBounds(_ptStartShot))
                return;

            ucPlayerUp1.sbFadeOut.Begin();

            _draggingPuck = true;
            _draggingStartTick = System.Environment.TickCount;

        }

        void puck_MouseMove(object sender, MouseEventArgs e)
        {
            Point ptMouse = e.GetPosition(cnvTable);
            if (!PointWithinBounds(ptMouse))
                return;

            if (_draggingPuck)
            {
                PhysicsSprite puck = sender as PhysicsSprite;
                puck.BodyObject.Position = new Vector2((float)(ptMouse.X), (float)(ptMouse.Y));

                // we want to govern the start drag point in case the user goes back-and-forth with the mouse.
                if (ptMouse.Y > _ptStartShot.Y) _ptStartShot.Y = ptMouse.Y;
                if (System.Environment.TickCount - _draggingStartTick > 300)
                {
                    _ptStartShot.X = ptMouse.X;
                    _ptStartShot.Y = ptMouse.Y;
                    _draggingStartTick = System.Environment.TickCount;
                }
            }
        }

        void puck_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // is this the current puck to shoot?
            if (sender != _pucks[_currentPuckToShoot])
                return;

            PhysicsSprite puck = sender as PhysicsSprite;
            Point ptEndShot = e.GetPosition(cnvTable);
            _draggingPuck = false;

            if (!PointWithinBounds(_ptStartShot))
                return;

            double diffX = -(_ptStartShot.X - ptEndShot.X);
            double diffY = -(_ptStartShot.Y - ptEndShot.Y);

            if (Math.Abs(diffX) < 10 && Math.Abs(diffY) < 10)
                return;

            int scalePower = 20;
            Vector2 force = new Vector2((float)(diffX * scalePower), (float)(diffY * scalePower));

            puck.BodyObject.ApplyForce(force);
            _currentPuckShot = _currentPuckToShoot;

        }

#if WINDOWS_PHONE
        void puck_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            // is this the current puck to shoot?
            _puckDragging = _pucks[_currentPuckToShoot];
            if (sender != _puckDragging)
                return;

            _draggingPuck = true;

            _ptStartShot = e.ManipulationOrigin;

            _ptStartShot = new Point(_ptStartShot.X + _puckDragging.BodyObject.Position.X - _puckDragging.Width / 2, _ptStartShot.Y + _puckDragging.BodyObject.Position.Y - _puckDragging.Height / 2);

            // note that the Manipulation coordinates are relative to the PUCK, so we need to add in the offset position.
            if (!PointWithinBounds(_ptStartShot))
                return;

            _puckDragging.BodyObject.Position = new Vector2((float)(_ptStartShot.X), (float)(_ptStartShot.Y));

            ucPlayerUp1.sbFadeOut.Begin();
        }

        void cnvTable_MouseMove(object sender, MouseEventArgs e)
        {
            if (_puckDragging != null)
            {

                Point ptMouse = e.GetPosition(cnvTable);

                if (!PointWithinBounds(ptMouse))
                    return;

                _puckDragging.BodyObject.Position = new Vector2((float)(ptMouse.X), (float)(ptMouse.Y));

                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        void puck_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

            Point ptMouse = new Point(_ptStartShot.X + e.TotalManipulation.Translation.X, _ptStartShot.Y + e.TotalManipulation.Translation.Y);

            if (!PointWithinBounds(new Point(_puckDragging.BodyObject.Position.X, _puckDragging.BodyObject.Position.Y)))
                return;

            int scalePower = 4;
            Vector2 force = new Vector2(-(float)(e.FinalVelocities.LinearVelocity.X * scalePower), -(float)(e.FinalVelocities.LinearVelocity.Y * scalePower));

            _puckDragging.BodyObject.ApplyForce(force);
            _currentPuckShot = _currentPuckToShoot;

            _puckDragging = null;
        }
#endif

        bool PointWithinBounds(Point ptMouse)
        {
            double left = Convert.ToDouble(rectInBounds.GetValue(Canvas.LeftProperty));
            double top = Convert.ToDouble(rectInBounds.GetValue(Canvas.TopProperty));

            if ((ptMouse.X > left && ptMouse.X < left + rectInBounds.Width) &&
            (ptMouse.Y > top && ptMouse.Y < top + rectInBounds.Height))
                return true;
            else
                return false;
        }

        int GetPointsForPuck(PhysicsSprite puck)
        {
            int score = 0;
            Vector2 puckPos = puck.BodyObject.Position;

            double left = Convert.ToDouble(rectPoints3.GetValue(Canvas.LeftProperty));
            double top = Convert.ToDouble(rectPoints3.GetValue(Canvas.TopProperty));
            if ((puckPos.X > left && puckPos.X < left + rectPoints3.Width) && (puckPos.Y > top && puckPos.Y < top + rectPoints3.Height))
                score = 3;

            left = Convert.ToDouble(rectPoints2.GetValue(Canvas.LeftProperty));
            top = Convert.ToDouble(rectPoints2.GetValue(Canvas.TopProperty));
            if ((puckPos.X > left && puckPos.X < left + rectPoints2.Width) && (puckPos.Y > top && puckPos.Y < top + rectPoints2.Height))
                score = 2;

            left = Convert.ToDouble(rectPoints1.GetValue(Canvas.LeftProperty));
            top = Convert.ToDouble(rectPoints1.GetValue(Canvas.TopProperty));
            if ((puckPos.X > left && puckPos.X < left + rectPoints1.Width) && (puckPos.Y > top && puckPos.Y < top + rectPoints1.Height))
                score = 1;

            return score;
        }
    }
}
