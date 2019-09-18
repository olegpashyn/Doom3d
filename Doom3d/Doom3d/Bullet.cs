using System.Drawing;

namespace Doom3d
{
    public class Bullet : GameObject
    {
        public Bullet(int x, int y, IRenderable renderable) : base(x, y, renderable)
        {
        }

        public override void Update(RenderTarget renderTarget)
        {
            Y = Y - 1;
            Renderable.Render(renderTarget, new Point(X, Y));
        }
    }
}