using System;
using System.Collections.Generic;
using Game.Misc;
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
            void InitWalls(LevelModelObject levelData);
        }

        [Serializable]
        public readonly struct Wall : IEquatable<Wall>
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

            public bool Equals(Wall other)
            {
                return FromPosX == other.FromPosX
                    && FromPosY == other.FromPosY
                    && ToPosX == other.ToPosX
                    && ToPosY == other.ToPosY;
            }

            public override bool Equals(object obj)
            {
                return obj is Wall other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(FromPosX, FromPosY, ToPosX, ToPosY);
            }
        }

        [Serializable]
        public class WallData
        {
            public WalPosition[] positions;

            [Serializable]
            public struct WalPosition
            {
                public int fromX;
                public int toX;
                public int fromY;
                public int toY;
            }
        }

        private class Field : IField
        {
            private readonly HashSet<Wall> _walls = new();
            private IField GameField => this;

            private bool IsOutOfRange(int x, int y)
            {
                return x < 0 || y < 0 || x >= GameField.Width || y >= GameField.Height;
            }

            private bool IsWall(int fromX, int fromY, int toX, int toY)
            {
                return _walls.Contains(new Wall(fromX, fromY, toX, toY));
            }

            int IField.Width => Constant.FieldWidth;
            int IField.Height => Constant.FieldHeight;

            bool IField.IsCanMove(int x, int y, eDirection direction)
            {
                var nextPosition = Direction.GetNextPosition(x, y, direction);
                if (IsWall(x, y, nextPosition.x, nextPosition.y))
                {
                    return false;
                }

                return !IsOutOfRange(nextPosition.x, nextPosition.y);
            }

            public void InitWalls(LevelModelObject levelData)
            {
                _walls.Clear();
                foreach (var pos in levelData.GetWalls().positions)
                {
                    _walls.Add(new Wall(pos.fromX, pos.fromY, pos.toX, pos.toY));
                }
            }
        }
    }
}
