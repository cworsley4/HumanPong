using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;


namespace MyKinectGame
{
    class Item
    {
        Vector2 location;
        Texture2D texture;
        Rectangle boundingBox;

        public Item(Vector2 local, Texture2D tex, SpriteBatch spriteBach, GameTime gameTme)
        {
            location.X = (local.X + (tex.Width / 2));
            location.Y = (local.Y + (tex.Width / 2));
            texture = tex;
            boundingBox = new Rectangle((int)local.X, (int)local.Y, tex.Width, tex.Height);
            //boundingBox.Width = tex.Width;
            //boundingBox.Height = tex.Height;
            //boundingBox.X = (int)location.X - (boundingBox.Width / 2);
            //boundingBox.Y = (int)location.Y - (boundingBox.Height / 2);
        }

        //Getter for the Paddle rectangles
        public Rectangle getRectangle()
        {
            return boundingBox;
        }

        public void Update(float x, float y)
        {
            location = new Vector2(x, y);
            boundingBox.X = (int)location.X;
            boundingBox.Y = (int)location.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, location, Color.White);
        }
    }
}
