using System;
using System.Collections.Generic;

namespace Game.Model
{
    public partial class ModelBase
    {
        public interface ICharactersContainer
        {
            public TCharacterType Get<TCharacterType>() where TCharacterType : class;
            public void Add<TCharacterType>(TCharacterType obj) where TCharacterType : class;
        }

        private class CharactersContainer : ICharactersContainer
        {
            private readonly Dictionary<Type, List<object>> _characters = new();
            TCharacterType ICharactersContainer.Get<TCharacterType>()
            {
                var id = typeof(TCharacterType);
                if (_characters.TryGetValue(id, out var objectsList))
                {
                    if (objectsList.Count > 0)
                    {
                        return objectsList[0] as TCharacterType;
                    }
                        
                }

                return null;
            }

            void ICharactersContainer.Add<T>(T obj)
            {
                var id = typeof(T);

                if (_characters.TryGetValue(id, out var objectsList))
                    objectsList.Add(obj);
                else
                {
                    objectsList = new List<object> { obj };
                    _characters[id] = objectsList;
                }
            }
        }
    }
}