using UnityEngine.Events;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdCreatePacMan : ICommand
        {
            private readonly int _x;
            private readonly int _y;
            private readonly UnityEvent _cherryEvent;

            public CmdCreatePacMan(int x, int y, UnityEvent cherryEvent)
            {
                _cherryEvent = cherryEvent;
                _x = x;
                _y = y;
            }

            void ICommand.Exec(IContextWritable context)
            { 
                context.CharactersContainer.Add<IPacManWritable>(new PacMan(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreatePacMan(_x, _y, _cherryEvent);
            }
        }
    }
}