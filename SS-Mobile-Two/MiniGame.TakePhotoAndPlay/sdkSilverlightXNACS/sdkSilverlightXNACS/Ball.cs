using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BouncingBalls
{
    enum BallIs
    {
        Friend,
        Enemy,
        Bomb,
        FriendIsOutsideOfBoard,
        EnemyIsOutsideOfBoard
    }

    class Ball
    {
        public static int ballCounter = 0;

        int Id;
        Color color;
        IList<Ball> balls = new List<Ball>();
        Texture2D texture;
        public Vector2 TopLeftPosition;
        public Vector2 PressedTopLeftPosition;
        public Vector2 PressedPosition;
        public Vector2 HoldingPosition;
        public bool IsOnHold { get; set; }


        public BallIs BallIs { get; set; }


        Vector2 velocity;

        public Vector2 CenterPosition
        {
            get
            {
                return GetCenterLocation(TopLeftPosition);
            }
        }

        public Vector2 GetCenterLocation(Vector2 topLeftPosition)
        {
            return new Vector2(topLeftPosition.X + radius, topLeftPosition.Y + radius);
        }

        public float radius;
        float scale;

        public Ball(Color color, Texture2D texture, Vector2 center, Vector2 velocity, float radius)
        {
            this.color = color;
            this.texture = texture;
            this.TopLeftPosition = new Vector2(center.X - radius, center.Y - radius);
            this.velocity = velocity;
            this.radius = radius;
            CalculateScale();

            Id = ballCounter;
            ballCounter = ++ballCounter;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, TopLeftPosition, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Update()
        {
            BounceBall();

            
        }

        private void BounceBall()
        {
            if(IsOnHold)
            {
                var releaseDifference = PressedPosition - HoldingPosition;


                if (BallIs == BallIs.Friend)
                {
                    velocity = - releaseDifference * 0.05f;    
                }
                else if (BallIs == BallIs.Enemy)
                {
                    velocity = releaseDifference * 0.05f;
                }
                else if (BallIs == BallIs.Bomb)
                {
                    velocity = releaseDifference * 0.5f;
                }

                TopLeftPosition = PressedTopLeftPosition - releaseDifference;
            }
            else
            {
                Vector2 newTopLeft = TopLeftPosition + velocity;
                float left, right, top, bottom;
                left = newTopLeft.X;
                right = newTopLeft.X + (radius * 2);
                top = newTopLeft.Y;
                bottom = newTopLeft.Y + (radius * 2);


                if(top < 50 && left > 100 && left < 200)
                {
                    // bombs are not leaving field of battle..
                    if (BallIs != BallIs.Bomb)
                    {
                        if(BallIs == BallIs.Enemy)
                        {
                            BallIs = BallIs.EnemyIsOutsideOfBoard;
                        }
                        else if(BallIs == BallIs.Friend)
                        {
                            BallIs = BallIs.FriendIsOutsideOfBoard;
                        }
                    }
                    
                }

                if (top < 0 || bottom > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height)
                {
                    velocity.Y *= -1;
                }

                if (left < 0 || right > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width)
                {
                    velocity.X *= -1;
                }

                TopLeftPosition += velocity;
            }
        }

        private void CalculateScale()
        {
            float width = (float)texture.Bounds.Width;
            this.scale = (this.radius * 2) / width;
        }

        public bool Equals(Ball other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Ball)) return false;
            return Equals((Ball) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
