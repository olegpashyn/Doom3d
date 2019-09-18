using System.Drawing;

namespace Doom3d
{
    public class Bullet : GameObject, IExplode
    {
        public Bullet(int x, int y, IRenderable renderable) : base(x, y, renderable)
        {
        }

        public bool Exploded { get; private set; }

        public void Explode()
        {
            Exploded = true;
        }

        public override void Update(RenderTarget renderTarget)
        {
            Y = Y - 1;
            Renderable.Render(renderTarget, new Point(X, Y));
        }
    }
}