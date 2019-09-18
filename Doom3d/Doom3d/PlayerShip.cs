using System;
using System.Drawing;

namespace Doom3d
{
    public class PlayerShip : GameObject, IExplode
    {
        public PlayerShip(int initialX, int initialY) : base(initialX, initialY, new Animatable(2, 3, 'X'))
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