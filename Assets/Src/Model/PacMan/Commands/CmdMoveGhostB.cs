using Game.Misc;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdMoveGhostB : ICommand
        {
            private eDirection _direction;
            private readonly bool _isScared;

            public CmdMoveGhostB(eDirection direction, bool isScared) {
                _direction = direction;
                _isScared = isScared;
            }

            public void SetDirection(eDirection direction)
            {
                _direction = direction;
            }

            void ICommand.Exec(IContextWritable context)
            {
                var ghostB = context.CharactersContainer.Get<IGhostBWritable>();
                if(_isScared)
                {
                    var pacman = context.CharactersContainer.Get<IPacManWritable>();

                    var pacmanPosition = Direction.GetPacmanPosition(pacman.X, pacman.Y, ghostB.X, ghostB.Y);
                    var directions = Direction.RunFromPacman(pacmanPosition);

                    foreach (eDirection direction in directions)
                    {
                        if (context.Field.IsCanMove(ghostB.X, ghostB.Y, direction))
                        {
                            _direction = direction;
                            break;
                        }
                    }
                } else
                {

                    var isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
                    while(!isCanMove) 
                    {
                        ChangeDirection();
                        isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
                    }
                }
                
                var nextPosition = Direction.GetNextPosition(ghostB.X, ghostB.Y, _direction);
                ghostB.UpdatePositionB(nextPosition.x, nextPosition.y);
                context.EventManager.Get<IPacManEventsWritable>().UpdateGhostBPosition(nextPosition.x, nextPosition.y);
            }

            public eDirection GetDirection(eDirection direction, IContextWritable context)
            {
                var ghostB = context.CharactersContainer.Get<IGhostBWritable>();
                var isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, direction);
                while (!isCanMove)
                {
                    ChangeDirection();
                    isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
                }
                return _direction;
            }

            public void ChangeDirection ()
            {
                var newDirection = UnityEngine.Random.Range(0, 4);
                switch(newDirection)
                {
                    case 0:
                        _direction = eDirection.DOWN;
                        break;
                    case 1:
                        _direction = eDirection.UP;
                        break;
                    case 2:
                        _direction = eDirection.LEFT;
                        break;
                    default :
                        _direction = eDirection.RIGHT;
                        break;
                }
            } 
        }
    }
}