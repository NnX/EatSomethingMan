namespace Game.Model
{
    public partial class ModelBase
    {
        protected interface IGhostB
        {
            int X { get; }
            int Y { get; }
        }

        protected interface IGhostBWritable : IGhostB
        {
            void UpdatePositionB(int x, int y);
            void MovingB(bool isMoving);
        }
    }
}