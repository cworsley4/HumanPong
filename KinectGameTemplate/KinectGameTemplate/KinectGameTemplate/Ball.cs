using System;
using System.Collections;
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
    class Ball
    {
        Vector2 location;
        Texture2D texture;
        Vector2 trajectory;
        Rectangle boundingBox;
        float velocity;
        SpriteFont font;
        SoundEffect wallHit;
        SoundEffect paddleHit;
        int lives;
        bool gameOver;
        GameTime gameTime;
        int span;

        //public Ball()
        //{
        //    location = new Vector2(0f, 0f);
        //    texture = null;
        //    trajectory = new Texture2D();
        //    boundingBox = null;


        //}

        public Ball(Vector2 local, Texture2D tex, SpriteBatch spriteBatch, SpriteFont font, ArrayList sound, GameTime myGameTime)
        {
            location = local;
            texture = tex;
            trajectory = new Vector2(10f, 1f);
            velocity = 5f;
            boundingBox = new Rectangle(960, 540, texture.Width, texture.Height);
            this.font = font;
            wallHit = (SoundEffect)sound[1];
            paddleHit = (SoundEffect)sound[0];
            lives = 3;
            gameTime = myGameTime;
            

        }

        public void Update(GameTime gametime, GraphicsDeviceManager gm, Item Paddle1, Item Paddle2)
        {
            //Place the bounding box at the same location as the ball sprite
            boundingBox.X = (int)location.X;
            boundingBox.Y = (int)location.Y;



            //hits floor
            if (location.Y + texture.Width >= gm.PreferredBackBufferHeight) 
            {
                trajectory.Y = -(velocity);
                velocity += .1f;
                wallHit.Play();
            }

            //hits ceiling
            if (location.Y <= 0)
            {
                trajectory.Y = velocity;
                velocity += .1f;
                wallHit.Play();
            }

            //hits right wall
            if ((location.X + texture.Width) >= gm.PreferredBackBufferWidth)
            {
                trajectory.X = -(velocity);
                velocity += .1f;
                if (lives > 0)
                {
                    lives--;
                }
                wallHit.Play();
            }
            //hits left wall
            if(location.X <= 0)
            {
                trajectory.X = (velocity);
                velocity += .1f;
                if (lives > 0)
                {
                    lives--;
                }
                wallHit.Play();
            }

            //Ask if the Rectangles of the ball and of paddle one have intersected.
            if(boundingBox.Intersects(Paddle2.getRectangle()))
            {
                trajectory.X = 5f;
                paddleHit.Play();
            }

            //Ask if the Rectangles of the ball and of paddle one have intersected.
            if (boundingBox.Intersects(Paddle1.getRectangle()))
            {
                trajectory.X = -5f;
                paddleHit.Play();
            }


            if (lives <= 0)
            {
                gameOver = true;
                lives = 0;
            }


            //Make the ball move.
            location += trajectory;


        }


        public void Respawn()
        {
            lives = 3;
            trajectory = new Vector2(10f, 10f);
            velocity = 5f;
            location = new Vector2(50f, 50f);
            gameOver = false;

        }


        public void GameOverDraw(SpriteBatch spriteBatch, TimeSpan gameSpan)
        {

            if (gameOver == true)
            {
                spriteBatch.DrawString(font, "GAME OVER!!!  You lasted " + span + " seconds!\nClap to play again!", new Vector2(200f, 200f), Color.Red);
            }
            else
            {
                span = gameSpan.Seconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Update the score of the players
            spriteBatch.DrawString(font, "" + lives + "", new Vector2(170f, 7f), Color.White, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, location, Color.White);

        }
    }
}
