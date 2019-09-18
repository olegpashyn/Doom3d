using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Doom3d
{
    public class Constants
    {
        public const int LoopWaitingTimeMs = 250;
    }

    public interface IRenderable
    {
        int Width { get; }
        int Height { get; }

        void Render();
    }

    public interface ICommand
    {
    }

    public class ResetCommand : ICommand { }

    public class EscapeCommand : ICommand { }

    public interface IShipCommand : ICommand { }

    public class MoveLeft : IShipCommand { }

    public class MoveRight : IShipCommand { }

    public class Shoot : IShipCommand { }

    public abstract class GameObject
    {
        protected GameObject(int x, int y, IRenderable renderable)
        {
            X = x;
            Y = y;
            Renderable = renderable;
        }

        public int X { get; protected set; }
        public int Y { get; protected set; }

        public IRenderable Renderable { get; }
    }

    public class PlayerShipUi : IRenderable
    {
        public int Width { get; } = 3;

        public int Height { get; } = 2;

        public void Render()
        {
            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                    Console.Write("X");
        }
    }

    public class PlayerShip : GameObject
    {
        public PlayerShip(int initialX, int initialY) : base(initialX, initialY, new PlayerShipUi())
        {
        }

        public void Execute(IShipCommand command)
        {
            if (command is MoveLeft)
            {
                X = X - 1;
            }
            else if (command is MoveRight)
            {
                X = X + 1;
            }
        }
    }

    public class InvaderUi1 : IRenderable
    {
        public int Width => 1;

        public int Height => 1;

        public void Render() => Console.Write("O");
    }

    public class Invader : GameObject
    {
        public Invader(int initialX, int initialY, IRenderable renderable) : base(initialX, initialY, renderable)
        {
        }

        public void Execute()
        {
        }
    }

    public class Program
    {
        private static readonly ConcurrentQueue<ICommand> _userCommands = new ConcurrentQueue<ICommand>();
        private static IList<Invader> _levelObjects = new List<Invader>();
        private static PlayerShip _ship;

        public static void Main()
        {
            Reset();
            var inputThread = new Thread(InputManagerLoop) { IsBackground = false };
            var mainLoopThread = new Thread(MainGameLoop) { IsBackground = false };
            inputThread.Start();
            mainLoopThread.Start();
        }

        public static void MainGameLoop()
        {
            while (true)
            {
                CommandExecute();
                Render();
                Thread.Sleep(250);
            }
        }

        private static void Reset()
        {
            _ship = new PlayerShip(Console.WindowWidth / 2, Console.WindowHeight - 2);
            _levelObjects = new List<Invader>();
            for (int i = 0; i < 10; i++)
            {
                _levelObjects.Add(new Invader(2 * i, 10, new InvaderUi1()));
            }
        }

        private static void Render()
        {
            Console.Clear();
            Console.SetCursorPosition(_ship.X, _ship.Y);
            _ship.Renderable.Render();

            foreach (var invader in _levelObjects)
            {
                Console.SetCursorPosition(invader.X, invader.Y);
                invader.Renderable.Render();
            }
        }

        private static void CommandExecute()
        {
            while (_userCommands.TryDequeue(out ICommand result))
            {
                if (result is IShipCommand)
                {
                    _ship.Execute(result as IShipCommand);
                }
            }
        }

        public static void InputManagerLoop()
        {
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        _userCommands.Enqueue(new MoveLeft());
                        break;

                    case ConsoleKey.RightArrow:
                        _userCommands.Enqueue(new MoveRight());
                        break;

                    case ConsoleKey.Spacebar:
                        _userCommands.Enqueue(new Shoot());
                        break;

                    case ConsoleKey.Escape:
                        _userCommands.Enqueue(new EscapeCommand());
                        return;

                    default:
                        break;
                }
                Thread.Sleep(Constants.LoopWaitingTimeMs);
            }
        }
    }
}