namespace Game.Model
{
    public partial class ModelPacMan
    {
        private class CmdCreatePacMan : ICommand
        {
            private readonly int _x;
            private readonly int _y;

            public CmdCreatePacMan(int x, int y)
            {
                _x = x;
                _y = y;
            }

            void ICommand.Exec(IContextWritable context)
            {
                context.CharactersContainer.Add<IPacManWritable>(new PacMan(_x, _y));
                context.EventManager.Get<IPacManEventsWritable>().CreatePacMan(_x, _y);
            }
        }
    }
}
