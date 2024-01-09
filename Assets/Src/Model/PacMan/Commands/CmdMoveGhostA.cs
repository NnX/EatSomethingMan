using Game.Misc;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdMoveGhostA : ICommand
        {
            private eDirection _direction;
            private ePacmanPosition _pacmanPosition;
            private readonly bool _isScared;

            public CmdMoveGhostA(eDirection direction, ePacmanPosition pacmanPosition, bool isScared) {               
                _direction = direction;
                _pacmanPosition = pacmanPosition;
                _isScared = isScared;
            }

            public void SetDirection(eDirection direction)
            {
                _direction = direction;
            }

            void ICommand.Exec(IContextWritable context)
            {

                if(Constant.IsTwoPlayers)
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

                    var pacmanPosition = Direction.getPacmanPosition(pacman.X, pacman.Y, ghostA.X, ghostA.Y);
                    List<eDirection> directions;

                    if(_isScared)
                    {
                        directions = Direction.RunFromPacman(pacmanPosition);
                    } 
                    else
                    {
                        directions = Direction.FindPacman(pacmanPosition);
                    }
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