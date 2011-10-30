using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BouncingBalls
{
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
        public bool IsOutsideOfBoard { get; set; }
        Vector2 velocity;

        public Vector2 CenterPosition
        {
            get
            {
                return new Vector2(TopLeftPosition.X + radius, TopLeftPosition.Y + radius);
            }
        }
        
        float radius;
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

                velocity = releaseDifference*0.05f;

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


                if(top < 0)
                {
                    IsOutsideOfBoard = true;
                }

                if (bottom > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height)
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
