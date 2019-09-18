using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Threading;
using static Doom3d.Constants;

namespace Doom3d
{
    public class ObjectContainer
    {
        private List<GameObject> _gameObjects;

        public ObjectContainer(IList<GameObject> gameObjects)
        {
            _gameObjects = new List<GameObject>();
            _gameObjects.AddRange(gameObjects);
        }

        public void AddGameObject(GameObject go)
        {
            _gameObjects.Add(go);
        }

        public IList<GameObject> GameObjects
        {
            get
            {
                for (var i = _gameObjects.Count - 1; i >= 0; i--)
                {
                    if (!_gameObjects[i].IsInWindow(Program.RenderSize))
                    {
                        _gameObjects.RemoveAt(i);
                    }
                }
                return _gameObjects;
            }
        }
    }

    public class Program
    {
        private static readonly ConcurrentQueue<ICommand> _userCommands = new ConcurrentQueue<ICommand>();
        private static List<Invader> _invaders = new List<Invader>();
        private static RenderTarget _renderTarget;
        public static ObjectContainer GameObjects;
        private static PlayerShip _ship;

        private static Size _invaderSize = new Size(5, 3);
        private static Size _shipSize = new Size(7, 3);
        public static Size RenderSize = new Size(100, 40);

        public static void Main()
        {
            Console.SetWindowSize(120, 40);
            RenderSize = new Size(Console.WindowWidth, Console.WindowHeight - 1);
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
            var gameobjects = GameObjects.GameObjects;
            for (var i = 0; i < gameobjects.Count; i++)
            {
                for (var j = i + 1; j < gameobjects.Count; j++)
                {
                    var firstGameObject = gameobjects[i];
                    var secondGameObject = gameobjects[j];

                    if (firstGameObject.X <= secondGameObject.X + secondGameObject.Renderable.Width &&
                        firstGameObject.X + firstGameObject.Renderable.Width >= secondGameObject.X &&
                        firstGameObject.Y <= secondGameObject.Y + secondGameObject.Renderable.Height &&
                        firstGameObject.Y + firstGameObject.Renderable.Height >= secondGameObject.Y)
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
            var gameobjects = new List<GameObject>();
            _ship = new PlayerShip(new Point(RenderSize.Width / 2, RenderSize.Height - _shipSize.Height), _shipSize, new[] { ImageLibrary.OpenEyedCat, ImageLibrary.ClosedEyedCat });
            _invaders = new List<Invader>();
            ArrangeInvaders();
            _invaders.ForEach(g => gameobjects.Add(g));
            GameObjects = new ObjectContainer(gameobjects);
            GameObjects.AddGameObject(_ship);
        }

        private static void ArrangeInvaders()
        {
            for (int i = 0; i < 1; i++)
            {
                _invaders.Add(new Invader((_invaderSize.Width + 1) * i, 10, new Animatable(_invaderSize,
                    new[] { ImageLibrary.OpenEyedMouse, ImageLibrary.ClosedEyedMouse })));
            }
        }

        private static void MoveInvaders(Direction direction)
        {
            _invaders.ForEach(inv => inv.Move(direction));
        }

        private static void Render()
        {
            _renderTarget = new RenderTarget(RenderSize);
            var gameobjects = GameObjects.GameObjects;
            foreach (var gameobject in gameobjects)
            {
                gameobject.Update(_renderTarget);
            }

            _renderTarget.Present();
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
                        PlaySound(Sound.Shoot);
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

        public static void PlaySound(Sound sound)
        {
            string fileName = "";

            switch (sound)
            {
                case Sound.Shoot:
                    fileName = "Shoot.wav";
                    break;

                case Sound.Kill:
                    fileName = "mouse_squeek.wav";
                    break;
            }

            var soundLocation = Environment.CurrentDirectory + @"\media\";
            var sp = new SoundPlayer(soundLocation + fileName);
            sp.Play();
        }
    }
}