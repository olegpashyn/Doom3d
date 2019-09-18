namespace Doom3d
{
    public class Constants
    {
        public const int LoopWaitingTimeMs = 25;
        public const int LoopWaitingBound = 5;
        public const int InvadersMoveSize = 10;

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