using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using KinectExplorer;

namespace MyKinectGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SampleGameHost : KinectGameHost
    {
        protected override Size GetResolution()
        {
            //The resolution at which to run the game
            return new Size(800, 600);
        }

        protected override Type GetKinnectGame()
        {
            //Return the type of your KinectGame here
            return typeof(SampleGame);
        }
    }
}
