using System;

namespace Doom3d
{
    public class PlayerShip : GameObject
    {
        public PlayerShip(int initialX, int initialY) : base(initialX, initialY, new PlayerShipUi())
        {
        }

        public void Execute(IShipCommand command)
        {
            if ((command is MoveLeft) && (X >= 1))
            {
                X = X - 1;
            }
            else if ((command is MoveRight) && (X <= (Console.WindowWidth - 7)))
            {
                X = X + 1;
            }
        }
    }
}