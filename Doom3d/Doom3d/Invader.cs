namespace Doom3d
{
    public class Invader : GameObject, IExplode
    {
        public Invader(int initialX, int initialY, IRenderable renderable) : base(initialX, initialY, renderable)
        {
        }

        public void Execute()
        {
        }

        public void Explode()
        {
            throw new System.NotImplementedException();
        }
    }
}