using System;
using System.Drawing;

namespace Doom3d
{
    public class PlayerShip : GameObject, IExplode
    {
        public PlayerShip(Point pos, Size size, Image[] images)
            : base(pos.X, pos.Y, new Animatable(size, images))
        {
        }

        public override void Update(RenderTarget renderTarget)
        {
            Renderable.Render(renderTarget, new Point(X, Y));
        }

        public void Execute(IShipCommand command)
        {
            if ((command is MoveLeft) && (X >= 1))
            {
                X = X - 1;
            }
            else if ((command is MoveRight) && (X + Renderable.Width <= (Console.WindowWidth - 1)))
            {
                X = X + 1;
            }
            else if (command is Shoot)
            {
                Program.GameObjects.AddGameObject(new Bullet(X + Renderable.Width / 2, Y - Renderable.Height / 2 - 1, new BulletUi()));
            }
        }

        public void Explode()
        {
            Exploded = true;
            Renderable = new PlayerShipExploded();
        }

        public bool Exploded { get; private set; }
    }
}