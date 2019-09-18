using System;
using System.Drawing;

namespace Doom3d
{
    public class RenderTarget
    {
        private char[,] _surface;

        public RenderTarget(Size size)
        {
            _surface = new char[size.Width, size.Height];
        }

        public void Put(int x, int y, char symbol)
        {
            if (x < 0 || y < 0)
                return;

            if (x >= _surface.GetLength(0) || y >= _surface.GetLength(1))
                return;

            _surface[x, y] = symbol;
        }

        public void Put(int x, int y, string line)
        {
            for (int i = 0; i < line.Length; ++i)
                Put(x + i, y, line[i]);
        }

        public void Put(Point pos, Image image)
        {
            var rows = image.Rows;
            for (int j = 0; j < rows.Length; ++j)
                Put(pos.X, pos.Y + j, rows[j]);
        }

        public void Present()
        {
            Console.CursorVisible = false;
            for (int i = 0; i < _surface.GetLength(0); ++i)
                for (int j = 0; j < _surface.GetLength(1); ++j)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(_surface[i, j]);
                }
        }
    }
}