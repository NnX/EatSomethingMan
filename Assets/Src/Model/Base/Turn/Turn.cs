using System.Collections.Generic;

namespace Game.Model
{
    public partial class ModelBase
    {
        protected interface ITurn
        {
            void Push(ICommand command);
        }

        interface ITurnInternal : ITurn
        {
            void Exec(IContextWritable context);
        }

        // ##################################

        class Turn : ITurn, ITurnInternal
        {
            List<ICommand> _commands = new List<ICommand>();

            // ======= ITurn ===============

            void ITurn.Push(ICommand command)
            { _commands.Add(command); }

            // ======= ITurnInternal =======

            void ITurnInternal.Exec(IContextWritable context)
            {
                foreach (ICommand curCommand in _commands)
                    curCommand.Exec(context);
            }
        }
    }
}