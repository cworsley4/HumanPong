using System;

namespace MyKinectGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //This should be your GameHost
            using (SampleGameHost game = new SampleGameHost())
            {
                game.Run();
            }
        }
    }
#endif
}

