using System;
using GramEngine.Core;

namespace GramPong // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WindowSettings windowSettings = new WindowSettings()
            {
                NaiveCollision = true,
                WindowTitle = "Pong demo",
                Width = 600,
                Height = 400
            };
            Window window = new Window(new MainScene(), windowSettings);
            window.Run();

        }
    }
}