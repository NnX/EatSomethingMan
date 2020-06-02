using Game.Misc;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace Game.Model
{
    public partial class ModelBase
    {
        public interface IField
        {
            int Width { get; }
            int Height { get; }

            bool IsCanMove(int x, int y, eDirection direction);
            void setWalls(ArrayList Walls);
            void InitWalls();
        }

        // ##############################################

        class Wall
        {
            public int fromPosX { get; }
            public int fromtPosY { get; }
            public int toPosX { get; }
            public int toPosY { get; }

            public Wall(int fromX, int fromY, int toX, int toY)
            {
                fromPosX = fromX;
                fromtPosY = fromY;
                toPosX = toX;
                toPosY = toY;
            }
        }

        class Field : IField
        {
            ArrayList _Walls = new ArrayList();
            IField IField { get { return this; } }

            bool IsOutOfRange(int x, int y)
            { return x < 0 || y < 0 || x >= IField.Width || y >= IField.Height; }

            bool IsWall(int fromX, int fromY, int toX, int toY)
            {
                foreach (Wall wall in _Walls)
                {
                    if(fromX == wall.fromPosX && fromY == wall.fromtPosY && toX == wall.toPosX && toY == wall.toPosY)
                    {
                        //Debug.Log("Is Wall - STOP!");
                        return true;
                    }
                }
                return false;
            }


            // ============ IField ======================

            int IField.Width => Constant.FieldWidth;
            int IField.Height => Constant.FieldHeight;

            bool IField.IsCanMove(int x, int y, eDirection direction)
            {
                (int x, int y) nextPositon = Direction.GetNextPosition(x, y, direction);
                if(IsWall(x, y, nextPositon.x, nextPositon.y))
                {
                    return false;
                }
                return !IsOutOfRange(nextPositon.x, nextPositon.y);
            }

            public void setWalls(ArrayList Walls)
            {
                _Walls = Walls;
            }

            public void InitWalls()
            {
                // horizontal walls
                // 1 line
                _Walls.Add(new Wall(3, 1, 3, 2));
                _Walls.Add(new Wall(4, 1, 4, 2));
                _Walls.Add(new Wall(5, 1, 5, 2));
                _Walls.Add(new Wall(6, 1, 6, 2));
                _Walls.Add(new Wall(9, 1, 9, 2));
                _Walls.Add(new Wall(10, 1, 10, 2));
                _Walls.Add(new Wall(11, 1, 11, 2));
                _Walls.Add(new Wall(12, 1, 12, 2));

                _Walls.Add(new Wall(3, 2, 3, 1));
                _Walls.Add(new Wall(4, 2, 4, 1));
                _Walls.Add(new Wall(5, 2, 5, 1));
                _Walls.Add(new Wall(6, 2, 6, 1));
                _Walls.Add(new Wall(9, 2, 9, 1));
                _Walls.Add(new Wall(10, 2, 10, 1));
                _Walls.Add(new Wall(11, 2, 11, 1));
                _Walls.Add(new Wall(12, 2, 12, 1));

                // 2 line
                _Walls.Add(new Wall(2, 4, 2, 5));
                _Walls.Add(new Wall(3, 4, 3, 5));
                _Walls.Add(new Wall(5, 4, 5, 5));
                _Walls.Add(new Wall(6, 4, 6, 5));
                _Walls.Add(new Wall(9, 4, 9, 5));
                _Walls.Add(new Wall(10, 4, 10, 5));
                _Walls.Add(new Wall(12, 4, 12, 5));
                _Walls.Add(new Wall(13, 4, 13, 5));

                _Walls.Add(new Wall(2, 5, 2, 4));
                _Walls.Add(new Wall(3, 5, 3, 4));
                _Walls.Add(new Wall(5, 5, 5, 4));
                _Walls.Add(new Wall(6, 5, 6, 4));
                _Walls.Add(new Wall(9, 5, 9, 4));
                _Walls.Add(new Wall(10, 5, 10, 4));
                _Walls.Add(new Wall(12, 5, 12, 4));
                _Walls.Add(new Wall(13, 5, 13, 4));

                // 3 line
                _Walls.Add(new Wall(2, 6, 2, 7));
                _Walls.Add(new Wall(3, 6, 3, 7));
                _Walls.Add(new Wall(7, 6, 7, 7));
                _Walls.Add(new Wall(8, 6, 8, 7));
                _Walls.Add(new Wall(12, 6, 12, 7));
                _Walls.Add(new Wall(13, 6, 13, 7));

                _Walls.Add(new Wall(2, 7, 2, 6));
                _Walls.Add(new Wall(3, 7, 3, 6));
                _Walls.Add(new Wall(7, 7, 7, 6));
                _Walls.Add(new Wall(8, 7, 8, 6));
                _Walls.Add(new Wall(12, 7, 12, 6));
                _Walls.Add(new Wall(13, 7, 13, 6));

                // 4 line
                _Walls.Add(new Wall(6, 7, 6, 8));
                _Walls.Add(new Wall(7, 7, 7, 8));
                _Walls.Add(new Wall(8, 7, 8, 8));

                _Walls.Add(new Wall(6, 8, 6, 7));
                _Walls.Add(new Wall(7, 8, 7, 7));
                _Walls.Add(new Wall(8, 8, 8, 7));
                // 5 line 
                _Walls.Add(new Wall(6, 9, 6, 10));
                _Walls.Add(new Wall(8, 9, 8, 10));
                _Walls.Add(new Wall(6, 10, 6, 9));
                _Walls.Add(new Wall(8, 10, 8, 9));

                // 6 line
                _Walls.Add(new Wall(4, 10, 4, 11));
                _Walls.Add(new Wall(5, 10, 5, 11));
                _Walls.Add(new Wall(10, 10, 10, 11));
                _Walls.Add(new Wall(11, 10, 11, 11));

                _Walls.Add(new Wall(4, 11, 4, 10));
                _Walls.Add(new Wall(5, 11, 5, 10));
                _Walls.Add(new Wall(10, 11, 10, 10));
                _Walls.Add(new Wall(11, 11, 11, 10));

                // Vertical
                // 1 line
                _Walls.Add(new Wall(2, 10, 3, 10));
                _Walls.Add(new Wall(2, 9, 3, 9));
                _Walls.Add(new Wall(2, 8, 3, 8));
                _Walls.Add(new Wall(2, 7, 3, 7));

                _Walls.Add(new Wall(3, 10, 2, 10));
                _Walls.Add(new Wall(3, 9, 2, 9));
                _Walls.Add(new Wall(3, 8, 2, 8));
                _Walls.Add(new Wall(3, 7, 2, 7));

                // 2 line
                _Walls.Add(new Wall(3, 10, 4, 10));
                _Walls.Add(new Wall(3, 9, 4, 9));
                _Walls.Add(new Wall(3, 4, 4, 4));
                _Walls.Add(new Wall(3, 3, 4, 3));

                _Walls.Add(new Wall(4, 10, 3, 10));
                _Walls.Add(new Wall(4, 9, 3, 9));
                _Walls.Add(new Wall(4, 4, 3, 4));
                _Walls.Add(new Wall(4, 3, 3, 3));

                // 3 line
                _Walls.Add(new Wall(4, 2, 5, 2));
                _Walls.Add(new Wall(4, 3, 5, 3));
                _Walls.Add(new Wall(4, 6, 5, 6));
                _Walls.Add(new Wall(4, 7, 5, 7));

                _Walls.Add(new Wall(5, 2, 4, 2));
                _Walls.Add(new Wall(5, 3, 4, 3));
                _Walls.Add(new Wall(5, 6, 4, 6));
                _Walls.Add(new Wall(5, 7, 4, 7));

                // 4 line
                _Walls.Add(new Wall(5, 8, 6, 8));
                _Walls.Add(new Wall(5, 9, 6, 9));
                _Walls.Add(new Wall(6, 8, 5, 8));
                _Walls.Add(new Wall(6, 9, 5, 9));

                // 5 line
                _Walls.Add(new Wall(7, 4, 8, 4));
                _Walls.Add(new Wall(7, 5, 8, 5));
                _Walls.Add(new Wall(7, 6, 8, 6));

                _Walls.Add(new Wall(8, 4, 7, 4));
                _Walls.Add(new Wall(8, 5, 7, 5));
                _Walls.Add(new Wall(8, 6, 7, 6));

                // 6 line
                _Walls.Add(new Wall(8, 8, 9, 8));
                _Walls.Add(new Wall(8, 9, 9, 9));
                _Walls.Add(new Wall(9, 8, 8, 8));
                _Walls.Add(new Wall(9, 9, 8, 9));

                // 7 line
                _Walls.Add(new Wall(10, 2, 11, 2));
                _Walls.Add(new Wall(10, 3, 11, 3));
                _Walls.Add(new Wall(10, 6, 11, 6));
                _Walls.Add(new Wall(10, 7, 11, 7));

                _Walls.Add(new Wall(11, 2, 10, 2));
                _Walls.Add(new Wall(11, 3, 10, 3));
                _Walls.Add(new Wall(11, 6, 10, 6));
                _Walls.Add(new Wall(11, 7, 10, 7));

                // 8 line
                _Walls.Add(new Wall(11, 10, 12, 10));
                _Walls.Add(new Wall(11, 9, 12, 9));
                _Walls.Add(new Wall(11, 4, 12, 4));
                _Walls.Add(new Wall(11, 3, 12, 3));

                _Walls.Add(new Wall(12, 10, 11, 10));
                _Walls.Add(new Wall(12, 9, 11, 9));
                _Walls.Add(new Wall(12, 4, 11, 4));
                _Walls.Add(new Wall(12, 3, 11, 3));

                // 9 line
                _Walls.Add(new Wall(12, 10, 13, 10));
                _Walls.Add(new Wall(12, 9, 13, 9));
                _Walls.Add(new Wall(12, 8, 13, 8));
                _Walls.Add(new Wall(12, 7, 13, 7));

                _Walls.Add(new Wall(13, 10, 12, 10));
                _Walls.Add(new Wall(13, 9, 12, 9));
                _Walls.Add(new Wall(13, 8, 12, 8));
                _Walls.Add(new Wall(13, 7, 12, 7));

            }
        }
    }
}