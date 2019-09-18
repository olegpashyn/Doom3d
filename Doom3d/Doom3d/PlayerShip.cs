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
            else if ((command is MoveRight) && (X <= (Console.WindowWidth - 7)))
            {
                X = X + 1;
            }
        }

        public void Explode()
        {
            Exploded = true;
            //Renderable = new PlayerShipExploded();
        }

        public bool Exploded { get; private set; }
    }
}