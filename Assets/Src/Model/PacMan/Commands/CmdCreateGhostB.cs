using Game.Misc;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdCreateGhostB : ICommand
        {
            int _x;
            int _y;

            // ========================================

            public CmdCreateGhostB(int x, int y)
            {
                _x = x;
                _y = y;
            }

             // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            {
                context.CharactardsContainer.Add<IGhostBWritable>(new GhostB(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreateGhostB(_x, _y);

            }
        }
    }
}