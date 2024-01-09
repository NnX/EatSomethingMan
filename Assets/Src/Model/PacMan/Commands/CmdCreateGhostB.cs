namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdCreateGhostB : ICommand
        {
            private readonly int _x;
            private readonly int _y;

            public CmdCreateGhostB(int x, int y)
            {
                _x = x;
                _y = y;
            }

            void ICommand.Exec(IContextWritable context)
            {
                context.CharactersContainer.Add<IGhostBWritable>(new GhostB(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreateGhostB(_x, _y);
            }
        }
    }
}