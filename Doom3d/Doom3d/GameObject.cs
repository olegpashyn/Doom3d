using static Doom3d.Constants;

namespace Doom3d
{
    public interface IExplode
    {
        void Explode();
    }

    public abstract class GameObject
    {
        protected GameObject(int x, int y, IRenderable renderable)
        {
            X = x;
            Y = y;
            Renderable = renderable;
        }

        public int X { get; protected set; }
        public int Y { get; protected set; }

        public IRenderable Renderable { get; protected set; }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    Y--;
                    break;

                case Direction.Down:
                    Y++;
                    break;

                case Direction.Left:
                    X--;
                    break;

                case Direction.Right:
                    X++;
                    break;
            }
        }
    }
}