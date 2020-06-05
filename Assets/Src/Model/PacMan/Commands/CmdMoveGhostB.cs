using Game.Misc;
using System.Collections.Generic;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdMoveGhostB : ICommand
        {
            eDirection _direction = eDirection.RIGHT;
            bool _isScared = false;

            // ========================================

            public CmdMoveGhostB(eDirection direction, bool isScared) {
                _direction = direction;
                _isScared = isScared;
            }

            public void setDirection(eDirection direction)
            {
                _direction = direction;
            }

            // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            {
                IGhostBWritable ghostB = context.CharactardsContainer.Get<IGhostBWritable>();
                if(_isScared)
                {
                    IPacManWritable pacman = context.CharactardsContainer.Get<IPacManWritable>();

                    ePacmanPosition pacmanPosition = Direction.getPacmanPosition(pacman.X, pacman.Y, ghostB.X, ghostB.Y);
                    List<eDirection> directions = Direction.RunFromPacman(pacmanPosition);

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

                    bool isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
                    while(!isCanMove) 
                    {
                        ChangeDirection();
                        isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
                    }
                }
                
                (int x, int y) nextPositon = Direction.GetNextPosition(ghostB.X, ghostB.Y, _direction);
                ghostB.UpdatePositionB(nextPositon.x, nextPositon.y);
                context.EventManager.Get<IPacManEventsWritable>().UpdateGhostBPosition(nextPositon.x, nextPositon.y);
            }

            public eDirection getDirection(eDirection direction, IContextWritable context)
            {
                IGhostBWritable ghostB = context.CharactardsContainer.Get<IGhostBWritable>();
                bool isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, direction);
                while (!isCanMove)
                {
                    ChangeDirection();
                    isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
                }
                return _direction;
            }

            public void ChangeDirection ()
            {
                
                int newDirection = UnityEngine.Random.Range(0, 4);
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