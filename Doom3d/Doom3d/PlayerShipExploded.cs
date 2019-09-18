using System;

namespace Doom3d
{
    internal class PlayerShipExploded : IRenderable
    {
        public int Width => 1;

        public int Height => 1;

        public void Render()
        {
            Console.Write("-");
        }
    }
}