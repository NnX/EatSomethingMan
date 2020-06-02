namespace Game.Model
{
    public partial class ModelPacMan
    {
        protected class GhostA : IGhostAWritable
        {
            int _x;
            int _y;
            bool _is_moving = true;

            // =============================

            public GhostA(int x, int y)
            {
                _x = x;
                _y = y;
            }

            // ======= IPacMan =============

            int IGhostA.X => _x;
            int IGhostA.Y => _y;

            void IGhostAWritable.MovingA(bool isMoving)
            {
                _is_moving = isMoving;
            }

            // ====== ICharacterWritable ==

            void IGhostAWritable.UpdatePositionA(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }
    }
}