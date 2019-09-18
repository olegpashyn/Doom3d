namespace Doom3d
{
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

        public IRenderable Renderable { get; }
    }
}