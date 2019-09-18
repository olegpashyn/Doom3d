using System;

namespace Doom3d
{
    public class PlayerShipUi : IRenderable
    {
        public int Width { get; } = 3;

        public int Height { get; } = 2;

        public void Render()
        {
            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                    Console.Write("X");
        }
    }
}