using System;

namespace Game.Model
{
    public abstract partial class ModelBase
    {
        protected readonly IContextWritable _context;

        protected ModelBase()
        {
            _context = new Context(new CharactersContainer(), new Field(), new EventManager());
            RegisterEvents(_context.EventManager);
        }

        protected abstract void RegisterEvents(IEventManagerInternal eventManager);

        protected void CreateAndExecuteTurn(Action<ITurn> onInitTurn)
        {
            ITurnInternal turn = new Turn();
            onInitTurn?.Invoke(turn);
            turn.Exec(_context);
        }

        protected IEventManager EventManager => _context.EventManager;
    }
}