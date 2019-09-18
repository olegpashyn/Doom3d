namespace Doom3d
{
    public interface IRenderable
    {
        int Width { get; }
        int Height { get; }

        void Render();
    }
}