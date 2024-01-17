using System;
using Game.Misc;
using System.Collections;
using Src.Misc;

namespace Game.Model
{
    public partial class ModelBase
    {
        protected interface IField
        {
            int Width { get; }
            int Height { get; }

            bool IsCanMove(int x, int y, eDirection direction);
            void SetWalls(ArrayList walls);
            void InitWalls(LevelModelObject levelData);
        }
        
        [Serializable]
        public class Wall
        {
            public int FromPosX { get; }
            public int FromPosY { get; }
            public int ToPosX { get; }
            public int ToPosY { get; }

            public Wall(int fromX, int fromY, int toX, int toY)
            {
                FromPosX = fromX;
                FromPosY = fromY;
                ToPosX = toX;
                ToPosY = toY;
            }
        }        
        [Serializable]
        public class WallData
        {
            public WalPosition[] positions;
            [Serializable] public struct WalPosition
            {
                public int fromX;  
                public int toX;  
                public int fromY;  
                public int toY;  
            } 
        }

        private class Field : IField
        {
            private ArrayList _walls = new();
            private IField GameField => this;

            private bool IsOutOfRange(int x, int y)
            {
                return x < 0 || y < 0 || x >= GameField.Width || y >= GameField.Height;
            }

            private bool IsWall(int fromX, int fromY, int toX, int toY)
            {
                foreach (Wall wall in _walls)
                {
                    if(fromX == wall.FromPosX && fromY == wall.FromPosY && toX == wall.ToPosX && toY == wall.ToPosY)
                    {
                        return true;
                    }
                }
                return false;
            }

            int IField.Width => Constant.FieldWidth;
            int IField.Height => Constant.FieldHeight;

            bool IField.IsCanMove(int x, int y, eDirection direction)
            {
                var nextPosition = Direction.GetNextPosition(x, y, direction);
                if(IsWall(x, y, nextPosition.x, nextPosition.y))
                {
                    return false;
                }
                return !IsOutOfRange(nextPosition.x, nextPosition.y);
            }

            public void SetWalls(ArrayList walls)
            {
                _walls = walls;
            }

            public void InitWalls(LevelModelObject levelData)
            {
                foreach (var pos in levelData.GetWalls().positions)
                {
                    _walls.Add(new Wall(pos.fromX, pos.fromY, pos.toX, pos.toY));  
                }
            }
        }
    }
}