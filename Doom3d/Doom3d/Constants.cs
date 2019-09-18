namespace Doom3d
{
    public class Constants
    {
        public const int LoopWaitingTimeMs = 20;
        public const int LoopWaitingBound = 5;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public enum Sound
        {
            Shoot,
            Kill,
            Lost,
            Win,
            Theme
        }
    }
}