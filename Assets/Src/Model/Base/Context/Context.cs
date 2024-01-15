namespace Game.Model
{
    public abstract partial class ModelBase
    {
        protected interface IContext
        {
            public IField Field { get; }
        }

        protected interface IContextWritable : IContext
        {
            public new IField Field { get; }
            public void InitWalls(LevelModelObject levelData);
            public ICharactersContainer CharactersContainer { get; }

            public IEventManagerInternal EventManager { get; }
        }

        private class Context : IContextWritable
        {
            private readonly ICharactersContainer _charactersContainer;
            private readonly IField _field;
            private readonly IEventManagerInternal _eventManager;

            public Context(ICharactersContainer characterContainer, IField field, IEventManagerInternal eventManager)
            {
                _charactersContainer = characterContainer;
                _field = field;
                _eventManager = eventManager;
                
            }

            public void InitWalls(LevelModelObject levelData)
            {
                _field.InitWalls(levelData);
            }
            IField IContext.Field => _field;
            IField IContextWritable.Field => _field;
            ICharactersContainer IContextWritable.CharactersContainer => _charactersContainer;
            IEventManagerInternal IContextWritable.EventManager => _eventManager;
        }
    }
}