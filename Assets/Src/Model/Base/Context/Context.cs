namespace Game.Model
{
    public abstract partial class ModelBase
    {
        public interface IContext
        {
            IField Field { get; }
        }

        public interface IContextWritable : IContext
        {
            new IField Field { get; }
            ICharactersContainer CharactardsContainer { get; }

            IEventManagerInternal EventManager { get; }
        }

        // #############################################

        class Context : IContext, IContextWritable
        {
            ICharactersContainer _charactersContainer;
            IField _field;
            IEventManagerInternal _eventManager;

            // =======================================

            public Context(ICharactersContainer characterContainer, IField field, IEventManagerInternal eventManager)
            {
                _charactersContainer = characterContainer;
                _field = field;
                _eventManager = eventManager;
                _field.InitWalls();
            }

            // ============== IContext ================

            IField IContext.Field => _field;

            // ========== IContextWritable ============

            IField IContextWritable.Field => _field;
            ICharactersContainer IContextWritable.CharactardsContainer => _charactersContainer;
            IEventManagerInternal IContextWritable.EventManager => _eventManager;
        }
    }
}