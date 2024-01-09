namespace Game.Model
{
    public partial class ModelPacMan
    {
        protected class PacMan : IPacManWritable
        {
            private int _x;
            private int _y;

            public PacMan(int x, int y)
            {
                _x = x;
                _y = y;
            }

            int IPacMan.X => _x;
            int IPacMan.Y => _y;

            void IPacManWritable.Moving(bool isMoving)
            {
            }

            void IPacManWritable.UpdatePosition(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }
    }
}