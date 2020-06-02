using System;
using System.Collections.Generic;

namespace Game.Model
{
    public partial class ModelBase
    {
        public interface ICharactersContainer
        {
            T Get<T>() where T : class;
            void Add<T>(T obj) where T : class;
        }

        // #########################################

        class CharactersContainer : ICharactersContainer
        {
            Dictionary<Type, List<object>> _characters = new Dictionary<Type, List<object>>();

            // ======== ICharactersContainer =======

            T ICharactersContainer.Get<T>()
            {
                Type id = typeof(T);
                List<object> objectsList;
                if (_characters.TryGetValue(id, out objectsList))
                {
                    if (objectsList.Count > 0)
                        return objectsList[0] as T;
                }

                return null;
            }

            void ICharactersContainer.Add<T>(T obj)
            {
                Type id = typeof(T);
                List<object> objectsList;

                if (_characters.TryGetValue(id, out objectsList))
                    objectsList.Add(obj);
                else
                {
                    objectsList = new List<object>();
                    objectsList.Add(obj);
                    _characters[id] = objectsList;
                }
            }
        }
    }
}