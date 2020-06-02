using Game.Misc;
using System.Collections.Generic;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdMoveGhostA : ICommand
        {
            eDirection _direction;
            ePacmanPosition _pacmanPosition;

            // ========================================

            public CmdMoveGhostA(eDirection direction, ePacmanPosition pacmanPosition) {               
                _direction = direction;
                _pacmanPosition = pacmanPosition;
            }

            // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            {

                if(Constant.IsTwoPlayers)
                {
                    IGhostAWritable ghostA = context.CharactardsContainer.Get<IGhostAWritable>();
                    bool isCanMove = context.Field.IsCanMove(ghostA.X, ghostA.Y, _direction);

                    if (isCanMove)
                    {
                        (int x, int y) nextPositon = Direction.GetNextPosition(ghostA.X, ghostA.Y, _direction);
                        ghostA.UpdatePositionA(nextPositon.x, nextPositon.y);
                        context.EventManager.Get<IPacManEventsWritable>().UpdateGhostAPosition(nextPositon.x, nextPositon.y);
                    }
                } else
                {
                    IGhostAWritable ghostA = context.CharactardsContainer.Get<IGhostAWritable>();
                    IPacManWritable pacman = context.CharactardsContainer.Get<IPacManWritable>();

                    ePacmanPosition pacmanPosition = Direction.getPacmanPosition(pacman.X, pacman.Y, ghostA.X, ghostA.Y);
                    List<eDirection> directions = Direction.FindPacman(pacmanPosition);
                    foreach (eDirection direction in directions)
                    {
                        if (context.Field.IsCanMove(ghostA.X, ghostA.Y, direction))
                        {
                            _direction = direction;
                            break;
                        }
                    }

                    (int x, int y) nextPositon = Direction.GetNextPosition(ghostA.X, ghostA.Y, _direction);
                    ghostA.UpdatePositionA(nextPositon.x, nextPositon.y);
                    context.EventManager.Get<IPacManEventsWritable>().UpdateGhostAPosition(nextPositon.x, nextPositon.y);
                }
            }
        }
    }
}