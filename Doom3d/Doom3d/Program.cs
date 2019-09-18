using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using static Doom3d.Constants;

namespace Doom3d
{
    public class Program
    {
        private static readonly ConcurrentQueue<ICommand> _userCommands = new ConcurrentQueue<ICommand>();
        private static List<Invader> _invaders = new List<Invader>();
        private static List<GameObject> _gameobjects = new List<GameObject>();
        private static PlayerShip _ship;

        public static void Main()
        {
            Reset();
            var inputThread = new Thread(InputManagerLoop) { IsBackground = true };
            var mainLoopThread = new Thread(MainGameLoop) { IsBackground = false };
            inputThread.Start();
            mainLoopThread.Start();
        }

        public static void MainGameLoop()
        {
            var invMovingDirections = Direction.Right;
            var invMovingCounter = InvadersMoveSize;

            while (true)
            {
                CommandExecute();
                if (invMovingCounter-- > 0)
                {
                    MoveInvaders(invMovingDirections);
                }
                else
                {
                    invMovingCounter = InvadersMoveSize;
                    invMovingDirections = invMovingDirections == Direction.Left ? Direction.Right : Direction.Left;
                    MoveInvaders(Direction.Down);
                }
                DetectCollisions();
                Render();
                Thread.Sleep(Constants.LoopWaitingTimeMs);
            }
        }

        public static void DetectCollisions()
        {
            foreach (var gameobject in _gameobjects)
            {
            }
        }

        private static void Reset()
        {
            _gameobjects = new List<GameObject>();
            _ship = new PlayerShip(Console.WindowWidth / 2, Console.WindowHeight - 2);
            _invaders = new List<Invader>();
            ArrangeInvaders();
            _gameobjects.Add(_ship);
            _invaders.ForEach(g => _gameobjects.Add(g));
        }

        private static void ArrangeInvaders()
        {
            for (int i = 0; i < 10; i++)
            {
                _invaders.Add(new Invader(4 * i, 10, new InvaderUi1()));
            }
        }

        private static void MoveInvaders(Direction direction)
        {
            _invaders.ForEach(inv => inv.Move(direction));
        }

        private static void Render()
        {
            Console.Clear();
            Console.SetCursorPosition(_ship.X, _ship.Y);
            _ship.Renderable.Render();

            foreach (var invader in _invaders)
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