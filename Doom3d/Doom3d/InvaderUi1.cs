using System;

namespace Doom3d
{
    public class InvaderUi1 : IRenderable
    {
        public int Width => 1;

        public int Height => 1;

        public void Render() => Console.Write("O");
    }
}