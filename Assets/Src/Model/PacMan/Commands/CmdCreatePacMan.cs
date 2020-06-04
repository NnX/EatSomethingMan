using Game.Misc;
using UnityEngine.Events;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdCreatePacMan : ICommand
        {
            int _x;
            int _y;
            UnityEvent _cherryEvent;

            // ========================================

            public CmdCreatePacMan(int x, int y, UnityEvent cherryEvent)
            {
                _cherryEvent = cherryEvent;
                _x = x;
                _y = y;
            }

            // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            { 
                context.CharactardsContainer.Add<IPacManWritable>(new PacMan(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreatePacMan(_x, _y, _cherryEvent);
            }
        }
    }
}