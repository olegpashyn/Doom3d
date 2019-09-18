using System;
using System.Drawing;

namespace Doom3d
{
    public class Animatable : IRenderable
    {
        private readonly Image[] _images;
        private int _index;

        public Animatable(Size size, Image[] images)
        {
            Width = size.Width;
            Height = size.Height;
            _images = images;
        }

        public int Width { get; }

        public int Height { get; }

        public void Render(RenderTarget renderTarget, Point position)
        {
            renderTarget.Put(position, _images[_index]);
            _index = (_index + 1) % _images.Length;
        }
    }
}