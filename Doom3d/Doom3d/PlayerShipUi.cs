using System;
using System.Drawing;

namespace Doom3d
{
    public class Animatable : IRenderable
    {
        private readonly char _fillSymbol;

        public Animatable(int width, int height, char fillSymbol)
        {
            Width = width;
            Height = height;
            _fillSymbol = fillSymbol;
        }

        public int Width { get; }

        public int Height { get; }

        public void Render(RenderTarget renderTarget, Point position)
        {
            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                    renderTarget.Put(position.X + i, position.Y + j, _fillSymbol);
        }
    }
}