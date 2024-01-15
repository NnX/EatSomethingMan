using System;
using UnityEngine;

namespace Game.Model
{
    public abstract partial class ModelBase
    {
        protected readonly IContextWritable ContextWritable;

        protected ModelBase()
        {
            ContextWritable = new Context(new CharactersContainer(), new Field(), new EventManager());
            RegisterEvents(ContextWritable.EventManager);
        }

        protected void InitWalls(LevelModelObject levelData)
        {
            ContextWritable.InitWalls(levelData);
        }

        protected abstract void RegisterEvents(IEventManagerInternal eventManager);

        protected void CreateAndExecuteTurn(Action<ITurn> onInitTurn)
        {
            ITurnInternal turn = new Turn();
            onInitTurn?.Invoke(turn);
            turn.Exec(ContextWritable);
        }

        protected IEventManager EventManager => ContextWritable.EventManager;
    }
}