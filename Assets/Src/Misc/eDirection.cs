using System.Collections.Generic;

namespace Game.Misc
{
    public enum eDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    public enum ePacmanPosition
    {
        UpUp, 
        UpRight, 
        RightRight,
        RightDown,
        DownDown, 
        DownLeft,
        LeftLeft,
        LeftUp, 
    }

    public static class Direction
    {
        public static (int x, int y)GetNextPosition(int x, int y, eDirection direction)
        {
            switch (direction)
            {
                
                case eDirection.LEFT:
                    return (x - 1, y);
                case eDirection.RIGHT:
                    return (x + 1, y);
                case eDirection.UP:
                    return (x, y + 1);
                default:
                //case eDirection.DOWN:
                    return (x, y - 1);
            }
        }

        public static List<eDirection> FindPacman(ePacmanPosition pacmanPosition)
        {
            List<eDirection> parts = new List<eDirection>();

            if (pacmanPosition == ePacmanPosition.UpUp) // upup // 1
            {
                parts.Add(eDirection.UP);
                parts.Add(eDirection.LEFT);
                parts.Add(eDirection.RIGHT);
            }
            else if (pacmanPosition == ePacmanPosition.UpRight)    //up right // 2
            {
                parts.Add(eDirection.UP);
                parts.Add(eDirection.RIGHT);
                parts.Add(eDirection.DOWN);
            }
            else if (pacmanPosition == ePacmanPosition.RightRight) // right right // 3
            {
                parts.Add(eDirection.RIGHT);
                parts.Add(eDirection.UP);
                parts.Add(eDirection.DOWN);
            }
            else if (pacmanPosition == ePacmanPosition.RightDown) // right down // 4
            {
                parts.Add(eDirection.RIGHT);
                parts.Add(eDirection.DOWN);
                parts.Add(eDirection.LEFT);
            }
            else if (pacmanPosition == ePacmanPosition.DownDown)    //down down // 5
            {
                parts.Add(eDirection.DOWN);
                parts.Add(eDirection.RIGHT);
                parts.Add(eDirection.LEFT);
            }
            else if (pacmanPosition == ePacmanPosition.DownLeft) // down left // 6
            {
                parts.Add(eDirection.DOWN);
                parts.Add(eDirection.LEFT);
                parts.Add(eDirection.UP);
            }
            else if (pacmanPosition == ePacmanPosition.LeftLeft) // left left // 7
            {
                parts.Add(eDirection.LEFT);
                parts.Add(eDirection.DOWN);
                parts.Add(eDirection.UP);
            }
            else if (pacmanPosition == ePacmanPosition.LeftUp) // left up // 8
            {
                parts.Add(eDirection.UP);
                parts.Add(eDirection.LEFT);
                parts.Add(eDirection.RIGHT);
            }


            return parts;
        }

        public static ePacmanPosition getPacmanPosition(int pacman_x, int pacman_y, int ghost_x, int ghost_y)
        {
            ePacmanPosition pacmanPosition = ePacmanPosition.DownLeft;

            if (pacman_x == ghost_x && pacman_y > ghost_y) // upup // 1
            {
                pacmanPosition = ePacmanPosition.UpUp;
            }
            else if (pacman_x > ghost_x && pacman_y > ghost_y)    //up right // 2
            {
                pacmanPosition = ePacmanPosition.UpRight;
            }
            else if (pacman_x > ghost_x && pacman_y == ghost_y) // right right // 3
            {
                pacmanPosition = ePacmanPosition.RightRight;
            }
            else if (pacman_x > ghost_x && pacman_y < ghost_y) // right down // 4
            {
                pacmanPosition = ePacmanPosition.RightDown;
            }
            else if (pacman_x == ghost_x && pacman_y < ghost_y)    //down down // 5
            {
                pacmanPosition = ePacmanPosition.DownDown;
            }
            else if (pacman_x < ghost_x && pacman_y < ghost_y) // down left // 6
            {
                pacmanPosition = ePacmanPosition.DownLeft;
            }
            else if (pacman_x < ghost_x && pacman_y == ghost_y) // left left // 7
            {
                pacmanPosition = ePacmanPosition.LeftLeft;
            }
            else if (pacman_x < ghost_x && pacman_y > ghost_y) // left up // 8
            {
                pacmanPosition = ePacmanPosition.LeftUp;
            }
            return pacmanPosition;
        }
    }
}