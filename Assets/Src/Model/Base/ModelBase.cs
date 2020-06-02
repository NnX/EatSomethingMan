using System;

namespace Game.Model
{
    public abstract partial class ModelBase
    {
        public IContextWritable _context;

        // ==========================================

        public ModelBase()
        {
            _context = new Context(new CharactersContainer(), new Field(), new EventManager());
            RegisterEvents(_context.EventManager);
        }

        // ==========================================

        protected abstract void RegisterEvents(IEventManagerInternal eventManager);

        // ==========================================

        protected void CreateAndExecuteTurn(Action<ITurn> onInitTurn)
        {
            ITurnInternal turn = new Turn();
            onInitTurn?.Invoke(turn);
            turn.Exec(_context);
        }

        protected IEventManager EventManager => _context.EventManager;
    }
}