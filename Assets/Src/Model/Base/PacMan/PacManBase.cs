namespace Game.Model
{
    public partial class ModelBase
    {
        protected interface IPacMan
        {
            int X { get; }
            int Y { get; }
        }

        protected interface IPacManWritable : IPacMan
        {
            void UpdatePosition(int x, int y);
            void Moving(bool isMoving);
        }
    }
}