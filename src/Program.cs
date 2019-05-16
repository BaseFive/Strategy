using System;

namespace Strategy
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Strategy())
                game.Run();
        }
    }
#endif
}
