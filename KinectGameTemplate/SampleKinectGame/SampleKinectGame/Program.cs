using System;

namespace SampleKinectGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SparklerGameHost game = new SparklerGameHost())
            //using (MercuryTest.Game1 game = new MercuryTest.Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

