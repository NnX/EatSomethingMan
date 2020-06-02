namespace Game.Model
{
    public partial class ModelPacMan
    {
        protected class GhostB : IGhostBWritable
        {
            int _x;
            int _y;
            bool _is_moving = true;

            // =============================

            public GhostB(int x, int y)
            {
                _x = x;
                _y = y;
            }

            // ======= IPacMan =============

            int IGhostB.X => _x;
            int IGhostB.Y => _y;

            void IGhostBWritable.MovingB(bool isMoving)
            {
                _is_moving = isMoving;
            }

            // ====== ICharacterWritable ==

            void IGhostBWritable.UpdatePositionB(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }
    }
}