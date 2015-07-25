using System;

namespace FreemooSDL
{
    static class Freemoo
    {

        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            using (FreemooGame game = new FreemooGame())
            {
                game.Run();
            }
            DateTime stop = DateTime.Now;
            TimeSpan t = stop.Subtract(start);
            //Console.WriteLine("t = " + t.TotalMilliseconds.ToString());
            //Console.ReadKey();
        }
    }
}
