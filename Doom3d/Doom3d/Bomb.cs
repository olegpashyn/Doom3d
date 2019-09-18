using System.Drawing;

namespace Doom3d
{
    internal class Bomb : GameObject, IExplode
    {
        private int _updateCount;

        public Bomb(int x, int y, IRenderable renderable) : base(x, y, renderable)
        {
        }

        public bool Exploded { get; private set; }

        public void Explode()
        {
            Exploded = true;
        }

        public override void Update(RenderTarget renderTarget)
        {
            if ((_updateCount++ % 2) == 0)
            {
                Y = Y + 1;
            }
            Renderable.Render(renderTarget, new Point(X, Y));
        }
    }
}