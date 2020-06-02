using Game.Misc;

namespace Game.Model
{
    public partial class ModelPacMan
    {
        class CmdCreateGhostA : ICommand
        {
            int _x;
            int _y;

            // ========================================

            public CmdCreateGhostA(int x, int y)
            {
                _x = x;
                _y = y;
            } 

            // ============== ICommand ================

            void ICommand.Exec(IContextWritable context)
            {
                context.CharactardsContainer.Add<IGhostAWritable>(new GhostA(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreateGhostA(_x, _y);
            }
        }
    }
}