using System;
using System.Collections.Generic;
using System.Threading;

namespace Doom3d
{
    public interface ICommand
    {
    }

    public class GameObject
    {
        private int x;
        private int y;
        private int width;
        private int height;
    }

    public class Program
    {
        private readonly IList<ICommand> _userCommands = new List<ICommand>();

        public static void Main()
        {
            var inputThread = new Thread(InputManagerLoop) { IsBackground = false };
            var mainLoopThread = new Thread(MainGameLoop) { IsBackground = false };
            inputThread.Start();
            mainLoopThread.Start();
        }

        public static void MainGameLoop()
        {
            while (true)
            {
                Render();
                Thread.Sleep(1000);
            }
        }

        private static void Render()
        {
        }

        public static void InputManagerLoop()
        {
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        break;

                    case ConsoleKey.RightArrow:
                        break;

                    case ConsoleKey.Spacebar:
                        break;

                    case ConsoleKey.Escape:
                        break;

                    default:
                        break;
                }
            }
        }
    }
}