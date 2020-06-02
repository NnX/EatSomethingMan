using System;
using System.Collections.Generic;

namespace Game.Model
{
    public interface IEventManager
    {
        T Get<T>() where T : class;
    }

    public interface IEventManagerInternal : IEventManager
    {
        void Register<T_internal, T_external>(T_internal evManager) where T_internal : class
                                                                    where T_external : class;
    }

    //################################################

    public class EventManager : IEventManagerInternal
    {
        Dictionary<Type, object> _evManagers = new Dictionary<Type, object>();

        //------------------------ IEventManager_internal -----------------------------

        void IEventManagerInternal.Register<T_internal, T_external>(T_internal evManager)
        {
            Type idInternal = typeof(T_internal);
            Type idExternal = typeof(T_external);

            T_external extObj = evManager as T_external;

            _evManagers[idInternal] = evManager;
            _evManagers[idExternal] = extObj;
        }

        //------------------------ ILogicEventManager -----------------------------

        T IEventManager.Get<T>()
        {
            Type id = typeof(T);

            T result = null;
            object r;
            if (_evManagers.TryGetValue(id, out r))
                result = r as T;

            return result;
        }
    }
}