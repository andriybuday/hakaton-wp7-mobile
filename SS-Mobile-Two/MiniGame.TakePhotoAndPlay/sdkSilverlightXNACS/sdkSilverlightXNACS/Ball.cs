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

        Color color;
        IList<Ball> balls = new List<Ball>();
        Texture2D texture;
        Vector2 topLeft;
        Vector2 velocity;
        
        float radius;
        float scale;

        public Ball(Color color, Texture2D texture, Vector2 center, Vector2 velocity, float radius)
        {
            this.color = color;
            this.texture = texture;
            this.topLeft = new Vector2(center.X - radius, center.Y - radius);
            this.velocity = velocity;
            this.radius = radius;
            CalculateScale();
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, topLeft, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Update()
        {
            BounceBall();

            topLeft += velocity;
        }

        private void BounceBall()
        {
            Vector2 newTopLeft = topLeft + velocity;
            float left, right, top, bottom;
            left = newTopLeft.X;
            right = newTopLeft.X + (radius * 2);
            top = newTopLeft.Y;
            bottom = newTopLeft.Y + (radius * 2);

            if (top < 0 || bottom > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height)
            {
                velocity.Y *= -1;
            }

            if (left < 0 || right > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width)
            {
                velocity.X *= -1;
            }
        }

        private void CalculateScale()
        {
            float width = (float)texture.Bounds.Width;
            this.scale = (this.radius * 2) / width;
        }
    }
}
