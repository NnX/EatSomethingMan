using Game.Misc;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdMovePacMan : ICommand
        {
            private readonly eDirection _direction;

            public CmdMovePacMan(eDirection direction) {               
                _direction = direction;
            }

            void ICommand.Exec(IContextWritable context)
            {
                var pacMan = context.CharactersContainer.Get<IPacManWritable>();
                var isCanMove = context.Field.IsCanMove(pacMan.X, pacMan.Y, _direction);

                if (isCanMove)
                {
                    var nextPosition = Direction.GetNextPosition(pacMan.X, pacMan.Y, _direction);
                    pacMan.UpdatePosition(nextPosition.x, nextPosition.y);
                    context.EventManager.Get<IPacManEventsWritable>().UpdatePacManPosition(nextPosition.x, nextPosition.y);
                }  
            }
        }
    }
}