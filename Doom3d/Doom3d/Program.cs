﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
                    if (!_gameObjects[i].IsInWindow(Program.RenderSize) || ((_gameObjects[i] as IExplode)?.Exploded ?? false))
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
        private static ICommand _moveCommand = null;
        private static ICommand _shootCommand = null;

        //private static List<Invader> _invaders = new List<Invader>();
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
            var mainLoopThread = new Thread(MainGameLoop) { IsBackground = false };
            mainLoopThread.Start();
        }

        public static void MainGameLoop()
        {
            var invMovingDirections = Direction.Right;
            var invMovingCounter = InvadersMoveSize;
            var loopCounter = LoopWaitingBound;

            while (true)
            {
                if (_ship.Exploded)
                {
                    GameOver();
                    while (Console.ReadKey(true).Key != ConsoleKey.Escape)
                        ;
                    break;
                }
                else
                {
                    if (--loopCounter <= 0)
                    {
                        loopCounter = LoopWaitingBound;

                        CommandExecute();
                        if (--invMovingCounter > 0)
                        {
                            MoveInvaders(invMovingDirections);
                        }
                        else
                        {
                            invMovingCounter = InvadersMoveSize;
                            invMovingDirections = invMovingDirections == Direction.Left ? Direction.Right : Direction.Left;
                            MoveInvaders(Direction.Down);
                        }
                    }
                    InvadersBomb();
                    DetectCollisions();
                    Render();
                }
                Thread.Sleep(LoopWaitingTimeMs);

                CheckKey();
            }
        }

        private static void InvadersBomb()
        {
            var botRowInvaders = new List<Invader>();
            var invaders = GameObjects.GameObjects.OfType<Invader>();
            foreach (var invader in invaders)
            {
                if (botRowInvaders.Any(b => b.X == invader.X))
                    continue;

                Invader maxY = invader;
                foreach (var inv in invaders)
                {
                    if (inv.X == invader.X && inv.Y < invader.Y)
                        continue;
                    else if (inv.X == invader.X)
                        maxY = inv;
                }
                botRowInvaders.Add(maxY);
            }

            var randomGen = new Random();
            for (var i = 0; i < botRowInvaders.Count; i++)
            {
                if (randomGen.Next() % 200 == 13)
                    botRowInvaders[i].Bomb();
            }
        }

        private static void GameOver()
        {
            Console.Clear();
            var file = File.ReadAllText(@"media\sad cat.txt");
            Console.WriteLine(file);
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
            var invaders = ArrangeInvaders();
            invaders.ForEach(g => gameobjects.Add(g));
            GameObjects = new ObjectContainer(gameobjects);
            GameObjects.AddGameObject(_ship);
        }

        private static List<Invader> ArrangeInvaders()
        {
            var invaders = new List<Invader>();
            for (int i = 0; i < 10; i++)
            {
                invaders.Add(new Invader((_invaderSize.Width + 1) * i, 10, new Animatable(_invaderSize,
                    new[] { ImageLibrary.OpenEyedMouse, ImageLibrary.ClosedEyedMouse })));
            }
            return invaders;
        }

        private static void MoveInvaders(Direction direction)
        {
            GameObjects.GameObjects.OfType<Invader>().ToList().ForEach(inv => inv.Move(direction));
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
            if (_shootCommand != null)
            {
                _ship.Execute(_shootCommand as IShipCommand);
            }
            _shootCommand = null;

            if (_moveCommand != null)
            {
                _ship.Execute(_moveCommand as IShipCommand);
            }
            _moveCommand = null;
        }

        private static void CheckKey()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }

            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    _moveCommand = new MoveLeft();
                    break;

                case ConsoleKey.RightArrow:
                    _moveCommand = new MoveRight();
                    break;

                case ConsoleKey.Spacebar:
                    PlaySound(Sound.Shoot);
                    _shootCommand = new Shoot();
                    break;

                case ConsoleKey.Escape:
                    _moveCommand = new EscapeCommand();
                    return;

                default:
                    break;
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

                case Sound.Lost:
                    fileName = "lost.wav";
                    break;

                case Sound.Win:
                    fileName = "win.wav";
                    break;

                case Sound.Theme:
                    fileName = "theme.wav";
                    break;
            }

            var soundLocation = Environment.CurrentDirectory + @"\media\";
            var sp = new SoundPlayer(soundLocation + fileName);
            sp.Play();
        }
    }
}