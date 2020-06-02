namespace Game.Model
{
    public partial class ModelBase
    {
        protected interface IGhostA
        {
            int X { get; }
            int Y { get; }
        }

        protected interface IGhostAWritable : IGhostA
        {
            void UpdatePositionA(int x, int y);
            void MovingA(bool isMoving);
        }
    }
}