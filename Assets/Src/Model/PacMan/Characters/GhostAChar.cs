namespace Game.Model
{
    public partial class ModelPacMan
    {
        protected class GhostA : IGhostAWritable
        {
            private int _x;
            private int _y;

            public GhostA(int x, int y)
            {
                _x = x;
                _y = y;
            }

            int IGhostA.X => _x;
            int IGhostA.Y => _y;

            void IGhostAWritable.MovingA(bool isMoving)
            {
            }

            void IGhostAWritable.UpdatePositionA(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }
    }
}