using System.Drawing;

namespace Doom3d
{
    internal class BombUi : IRenderable
    {
        public int Width => 1;

        public int Height => 1;

        public void Render(RenderTarget renderTarget, Point position)
        {
            renderTarget.Put(position.X, position.Y, '0');
        }
    }
}