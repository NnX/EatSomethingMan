using System.Linq;
using Game.Misc;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdMoveGhostA : ICommand
        {
            private eDirection _direction;
            private readonly bool _isScared;
            private readonly bool _isTwoPlayers;

            public CmdMoveGhostA(eDirection direction, bool isScared, bool isTwoPlayers)
            {
                _direction = direction;
                _isScared = isScared;
                _isTwoPlayers = isTwoPlayers;
            }

            void ICommand.Exec(IContextWritable context)
            {
                if (_isTwoPlayers)
                {
                    var ghostA = context.CharactersContainer.Get<IGhostAWritable>();
                    var isCanMove = context.Field.IsCanMove(ghostA.X, ghostA.Y, _direction);

                    if (!isCanMove)
                    {
                        return;
                    }

                    var nextPosition = Direction.GetNextPosition(ghostA.X, ghostA.Y, _direction);
                    ghostA.UpdatePositionA(nextPosition.x, nextPosition.y);
                    context.EventManager.Get<IPacManEventsWritable>().UpdateGhostAPosition(nextPosition.x, nextPosition.y);
                }
                else
                {
                    var ghostA = context.CharactersContainer.Get<IGhostAWritable>();
                    var pacman = context.CharactersContainer.Get<IPacManWritable>();

                    var pacmanPosition = Direction.GetPacmanPosition(pacman.X, pacman.Y, ghostA.X, ghostA.Y);
                    var directions = _isScared ? Direction.RunFromPacman(pacmanPosition) : Direction.FindPacman(pacmanPosition);
                    foreach (var direction in directions.Where(direction => context.Field.IsCanMove(ghostA.X, ghostA.Y, direction)))
                    {
                        _direction = direction;
                        break;
                    }

                    var nextPosition = Direction.GetNextPosition(ghostA.X, ghostA.Y, _direction);
                    ghostA.UpdatePositionA(nextPosition.x, nextPosition.y);
                    context.EventManager.Get<IPacManEventsWritable>().UpdateGhostAPosition(nextPosition.x, nextPosition.y);
                }
            }
        }
    }
}
