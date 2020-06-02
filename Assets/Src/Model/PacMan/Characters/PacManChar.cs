namespace Game.Model
{
    public partial class ModelPacMan
    {
        protected class PacMan : IPacManWritable
        {
            int _x;
            int _y;
            bool _is_moving = true;

            // =============================

            public PacMan(int x, int y)
            {
                _x = x;
                _y = y;
            }

            // ======= IPacMan =============

            int IPacMan.X => _x;
            int IPacMan.Y => _y;

            void IPacManWritable.Moving(bool isMoving)
            {
                _is_moving = isMoving;
            }

            // ====== ICharacterWritable ==

            void IPacManWritable.UpdatePosition(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }
    }
}