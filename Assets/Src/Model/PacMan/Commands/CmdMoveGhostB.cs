using Game.Misc;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdMoveGhostB : ICommand
        {
            eDirection _direction = eDirection.RIGHT;

            // ========================================

            public CmdMoveGhostB(eDirection direction) {
                _direction = direction;
            }

            public void setDirection(eDirection direction)
            {
                _direction = direction;
            }

            // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            {
                IGhostBWritable ghostB = context.CharactardsContainer.Get<IGhostBWritable>();
                bool isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
                while(!isCanMove) 
                {
                    ChangeDirection();
                    isCanMove = context.Field.IsCanMove(ghostB.X, ghostB.Y, _direction);
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