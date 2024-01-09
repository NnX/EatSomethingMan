using System.Collections.Generic;

namespace Game.Model
{
    public partial class ModelBase
    {
        protected interface ITurn
        {
            void Push(ICommand command);
        }

        protected interface ITurnInternal : ITurn
        {
            void Exec(IContextWritable context);
        }

        private class Turn : ITurn, ITurnInternal
        {
            private readonly List<ICommand> _commands = new();

            void ITurn.Push(ICommand command)
            {
                _commands.Add(command);
            }

            void ITurnInternal.Exec(IContextWritable context)
            {
                foreach (ICommand curCommand in _commands)
                    curCommand.Exec(context);
            }
        }
    }
}