﻿using System;
using System.Drawing;

namespace Doom3d
{
    internal class PlayerShipExploded : IRenderable
    {
        public int Width => 1;

        public int Height => 1;

        public void Render(RenderTarget renderTarget, Point position)
        {
            throw new NotImplementedException();
        }
    }
}