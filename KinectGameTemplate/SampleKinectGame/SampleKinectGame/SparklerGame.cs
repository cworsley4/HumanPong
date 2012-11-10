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
using System.Diagnostics;

using KinectExplorer;

namespace SampleKinectGame
{
    public class SparklerGame : KinectGame
    {
        //Particle Effects. Not part of the Kinect, so it won't be documented here. Just trust the they work.
        private ParticleFX sparkler, explosion;
        //Boolean indicating whether the second player is in the middle of a clap
        private bool clapped = false;
        
        public SparklerGame(GameParameters gParams)
            : base(gParams) { } //Leave this blank!

        public override GameConfig GetConfig()
        {
            //Return the GameConfig
            return new GameConfig("Sparkler", "Thomas", "Use you finger or a 'wand' to throw sparks.");
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentLoader content)
        {
            //Load some particle effects
            sparkler = new ParticleFX(graphics, "wand");
            sparkler.LoadContent(content);
            sparkler.Visible = false;

            explosion = new ParticleFX(graphics, "BasicExplosion");
            explosion.LoadContent(content);
        }

        // Looks for the closest point to the kinect (least depth) in a small area around the startPoint
        private Point findShallowestPoint(Point startPoint, int startDepth, out int depthOut)
        {
            //max difference in depth allowed (so we don't count things too far in the foreground)
            int variance = 500;
            //how far away to look
            int range = 120;

            int min = startDepth;
            Point minPoint = startPoint;

            for (int i = -range; i < range; i++)
            {
                for (int j = -range; j < range; j++)
                {
                    int depth = KinectManager.GetDepthAtPixel(startPoint.X + i, startPoint.Y + j, Resolution);
                    int dif = Math.Abs(startDepth - depth);

                    //if the depth is shallower but not past the variance
                    if (dif < variance && depth < min)
                    {
                        //then set a new min
                        min = depth;
                        minPoint = new Point(startPoint.X + i, startPoint.Y + j);
                    }
                }
            }

            depthOut = min;
            return minPoint;
        }

        public override void Update(GameTime gameTime)
        {
            //SkeletonA does the sparkle wand
            if (SkeletonA != null) 
            {
                //Get the hand's position
                Joint rightHand = SkeletonA.Joints[JointType.HandRight];
                Vector2 handPos = GetJointPosOnScreen(rightHand);

                //Set that as the start position and get the depth there for the start depth
                Point startPos = new Point((int)handPos.X, (int)handPos.Y);
                int startDepth = KinectManager.GetDepthAtPoint(rightHand.Position);

                //If the depth is valid (not -1)
                if (startDepth > -1)
                {
                    //Get the closest point to the Kinect in a small area around it (looking for a wand tip)
                    int depthOut;
                    Point mostDepth = findShallowestPoint(startPos, startDepth, out depthOut);
                    sparkler.Position = new Vector2(mostDepth.X, mostDepth.Y);
                    //We only want to show it if the hand is tracked
                    sparkler.Visible = rightHand.TrackingState == JointTrackingState.Tracked;
                }
                else
                {
                    //If invalid, make the sparkler invisible
                    sparkler.Visible = false;
                }
            }

            //Skeleton B does the clap explosion
            if (SkeletonB != null)
            {
                //Get the hand positions
                SkeletonPoint rightHandPos = SkeletonB.Joints[JointType.HandRight].Position;
                SkeletonPoint leftHandPos = SkeletonB.Joints[JointType.HandLeft].Position;

                //Find the distance in every direction
                float dx = rightHandPos.X - leftHandPos.X;
                float dy = rightHandPos.Y - leftHandPos.Y;
                float dz = rightHandPos.Z - leftHandPos.Z;

                //"claps" a 0.1m, resets at 0.35m for another clap
                float triggerDis = 0.1f, untriggerDis = 0.35f;

                float dis = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
                if (dis < triggerDis)
                {
                    //if we're not already in the middle of a clap
                    if (!clapped)
                    {
                        //get the middle of the two hands and trigger the explosion
                        Vector2 rightHandOnScreen = KinectManager.GetSkeletonPointPosOnScreen(rightHandPos, Resolution);
                        Vector2 leftHandOnScreen = KinectManager.GetSkeletonPointPosOnScreen(leftHandPos, Resolution);

                        explosion.Position = (rightHandOnScreen + leftHandOnScreen) / 2;
                        explosion.Trigger();
                        clapped = true;
                    }
                }
                else if (dis > untriggerDis)
                {
                    //Or if we've come out of a clap, set it to false
                    clapped = false;
                }

            }

            //always sparkle
            sparkler.Trigger();

            //update the FX
            sparkler.Update(gameTime);
            explosion.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Do draw logic here

            //Convenience methods
            DrawCamera();
            DebugSkeletons();

            //Draw the FX
            sparkler.Draw();
            explosion.Draw();
        }
    }
}

