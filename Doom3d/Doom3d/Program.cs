using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
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

        private static Size _renderSize = new Size(100, 40);

        public static void Main()
        {
            Console.SetWindowSize(120, 40);
            _renderSize = new Size(Console.WindowWidth, Console.WindowHeight-1);

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
                if (_ship.Exploded)
                {
                    GameOver();
                }
                else
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
                }
                Thread.Sleep(Constants.LoopWaitingTimeMs);
            }
        }

        private static void GameOver()
        {
            throw new NotImplementedException();
        }

        public static void DetectCollisions()
        {
            for (var i = 0; i < _gameobjects.Count; i++)
            {
                for (var j = i + 1; j < _gameobjects.Count; j++)
                {
                    var firstGameObject = _gameobjects[i];
                    var secondGameObject = _gameobjects[j];

                    if (firstGameObject.X < secondGameObject.X + secondGameObject.Renderable.Width &&
                        firstGameObject.X + firstGameObject.Renderable.Width > secondGameObject.X &&
                        firstGameObject.Y > secondGameObject.Y + secondGameObject.Renderable.Height &&
                        firstGameObject.Y + firstGameObject.Renderable.Height < secondGameObject.Y)
                    {
                        if (firstGameObject is IExplode)
                            ((IExplode)firstGameObject).Explode();
                        if (secondGameObject is IExplode)
                            ((IExplode)secondGameObject).Explode();
                    }
                }
            }
        }

        private static void Reset()
        {
            _gameobjects = new List<GameObject>();
            _ship = new PlayerShip(_renderSize.Width / 2, _renderSize.Height - 2);
            _invaders = new List<Invader>();
            ArrangeInvaders();
            _gameobjects.Add(_ship);
            _invaders.ForEach(g => _gameobjects.Add(g));
        }

        private static void ArrangeInvaders()
        {
            for (int i = 0; i < 10; i++)
            {
                _invaders.Add(new Invader(4 * i, 10, new Animatable(3, 3, 'O')));
            }
        }

        private static void MoveInvaders(Direction direction)
        {
            _invaders.ForEach(inv => inv.Move(direction));
        }

        private static void Render()
        {
            var renderTarget = new RenderTarget(_renderSize);
            _ship.Update(renderTarget);

            foreach (var invader in _invaders)
            {
                invader.Update(renderTarget);
            }

            renderTarget.Present();
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