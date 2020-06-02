using Game.Misc;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdCreatePacMan : ICommand
        {
            int _x;
            int _y;

            // ========================================

            public CmdCreatePacMan(int x, int y)
            {
                _x = x;
                _y = y;
            }

            // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            {
                context.CharactardsContainer.Add<IPacManWritable>(new PacMan(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreatePacMan(_x, _y);
            }
        }
    }
}