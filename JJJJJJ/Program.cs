using System;

namespace JJJJJJ
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Sandbox game = new Sandbox())
                game.Run();
        }
    }
#endif
}
