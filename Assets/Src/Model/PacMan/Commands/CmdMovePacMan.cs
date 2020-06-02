using Game.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdMovePacMan : ICommand
        {
            eDirection _direction;

            // ========================================

            public CmdMovePacMan(eDirection direction) {               
                _direction = direction;
            }

            // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            {
                IPacManWritable pacMan = context.CharactardsContainer.Get<IPacManWritable>();
                bool isCanMove = context.Field.IsCanMove(pacMan.X, pacMan.Y, _direction);

                if (isCanMove)
                {
                    (int x, int y) nextPositon = Direction.GetNextPosition(pacMan.X, pacMan.Y, _direction);
                    pacMan.UpdatePosition(nextPositon.x, nextPositon.y);
                    context.EventManager.Get<IPacManEventsWritable>().UpdatePacManPosition(nextPositon.x, nextPositon.y);
                }  

            }
        }
    }
}