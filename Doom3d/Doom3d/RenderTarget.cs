using System;
using System.Drawing;
using System.Text;

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
            int width = _surface.GetLength(0);
            StringBuilder sb = new StringBuilder();
            sb.Length = width;
            Console.SetCursorPosition(0, 0);
            for (int j = 0; j < _surface.GetLength(1); ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    char ch = _surface[i, j];
                    sb[i] = ch >= ' ' ? ch : ' ';
                }
                Console.Write(sb.ToString());
            }
        }
    }
}