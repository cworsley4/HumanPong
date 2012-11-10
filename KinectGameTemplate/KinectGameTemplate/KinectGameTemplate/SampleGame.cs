using System;
using System.Collections;
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

using KinectExplorer;

namespace MyKinectGame
{
    /*
     * You may want to take a Look at the methods in KinectManager and KinectGame
     * to access some helper methods.
     */
    public class SampleGame : KinectGame
    {
        Vector2 p1LhandPos;
        Texture2D paddletex;
        Texture2D paddletex2;
        Texture2D balltex;
        Texture2D background;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Item paddle1;
        Item paddle2;
        Ball ball;
        SoundEffect hitsound;
        SoundEffect wallsound;
        ArrayList sounds = new ArrayList();
        GameTime gametime;
        TimeSpan totalGameTime;

        //Most Kinect functionality is handled through
        //superclass methods, but if you need to access
        //the Kinect, do so with KinectManager.Kinect
        KinectSensor kinect = KinectManager.Kinect;

        public SampleGame(GameParameters gParams)
            : base(gParams) { } //Leave this blank!

        public override GameConfig GetConfig()
        {
            return new GameConfig("Human Pong", "Cecil Worsley and Christopher Bosack", "This is the Kinect adaptation of the classic Pong by Atari.");
        }

        public override void Initialize()
        {
            gametime = new GameTime();
            //Add sounds to ArrayList
            sounds.Add(hitsound);
            sounds.Add(wallsound);
            //Create ball
            ball = new Ball(new Vector2(graphics.PreferredBackBufferWidth/2, 0f), balltex, spriteBatch, font, sounds, gametime);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();
            graphics.ToggleFullScreen();
        }

        public override void LoadContent(ContentLoader content)
        {
            //Load content here
            background = content.Load<Texture2D>("PongBoard");
            paddletex = content.Load<Texture2D>("paddle");
            paddletex2 = content.Load<Texture2D>("paddle");
            balltex = content.Load<Texture2D>("ball");
            font = content.Load<SpriteFont>("basic");
            hitsound = content.Load<SoundEffect>("hitblip");
            wallsound = content.Load<SoundEffect>("wallblip");


        }

        public override void UnloadContent()
        {
            //Unload resources here
        }

        public override void Update(GameTime gameTime)
        {



            totalGameTime = totalGameTime + gameTime.ElapsedGameTime;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
            
            //Do update logic here
            //Send right hand position of Player1 to the Item update method
            if (SkeletonA != null)
            {
                Vector2 tempLeftHandPos = new Vector2();
                Vector2 tempRightHandPos = new Vector2();
                tempLeftHandPos = GetJointPosOnScreen(SkeletonA.Joints[JointType.HandLeft]);
                tempRightHandPos = GetJointPosOnScreen(SkeletonA.Joints[JointType.HandRight]);
                if ((tempLeftHandPos.X+15) >= (tempRightHandPos.X))
                {
                    ball.Respawn();
                }

                p1LhandPos = GetJointPosOnScreen(SkeletonA.Joints[JointType.HandLeft]);
                paddle1.Update(p1LhandPos.X, p1LhandPos.Y);
            }

            //Human paddle
            paddle2 = new Item(new Vector2(0f, p1LhandPos.Y * 2), paddletex2, spriteBatch, gameTime);

            //Opposite Paddle
            paddle1 = new Item(new Vector2((graphics.PreferredBackBufferWidth - 75), (graphics.PreferredBackBufferHeight - p1LhandPos.Y * 2)), paddletex, spriteBatch, gameTime);
            
            //Send positions of the paddles to the ball object
            ball.Update(gameTime, graphics, paddle1, paddle2);
        }

        public override void Draw(GameTime gameTime)
        {
            //Do draw logic here
            spriteBatch.Begin();

            DrawCamera(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            
            //DebugSkeletons();
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), null, new Color(255, 255, 255, 127), 0f, new Vector2(0, 0), SpriteEffects.None, 1);
            ball.GameOverDraw(spriteBatch, totalGameTime);
            paddle1.Draw(spriteBatch);
            paddle2.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
