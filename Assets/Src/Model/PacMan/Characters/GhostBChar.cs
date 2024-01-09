namespace Game.Model
{
    public partial class ModelPacMan
    {
        protected class GhostB : IGhostBWritable
        {
            private int _x;
            private int _y;

            public GhostB(int x, int y)
            {
                _x = x;
                _y = y;
            }

            int IGhostB.X => _x;
            int IGhostB.Y => _y;

            void IGhostBWritable.MovingB(bool isMoving)
            {
            }
            void IGhostBWritable.UpdatePositionB(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }
    }
}