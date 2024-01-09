namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdCreateGhostA : ICommand
        {
            private readonly int _x;
            private readonly int _y;

            public CmdCreateGhostA(int x, int y)
            {
                _x = x;
                _y = y;
            } 

            void ICommand.Exec(IContextWritable context)
            {
                context.CharactersContainer.Add<IGhostAWritable>(new GhostA(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreateGhostA(_x, _y);
            }
        }
    }
}