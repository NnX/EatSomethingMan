using UnityEngine;
using UnityEngine.Events;

namespace Src.View
{ 
    public interface ICharactersFactory
    {
        IPacMan CreatePacMan(Transform parentTransform, Vector2 position, UnityEvent unityEvent);
        IGhost CreateGhostA(Transform parentTransform, Vector2 position);
        IGhost CreateGhostB(Transform parentTransform, Vector2 position);
    }

    public class CharactersFactory : MonoBehaviour, ICharactersFactory
    {
        [SerializeField] private PacMan pacManPrefab;
        [SerializeField] private GhostA ghostAPrefab;
        [SerializeField] private GhostB ghostBPrefab;

        IPacMan ICharactersFactory.CreatePacMan(Transform parentTransform, Vector2 position, UnityEvent unityEvent)
        {
            return pacManPrefab.CloneMe(parentTransform, position, unityEvent);
        }

        IGhost ICharactersFactory.CreateGhostA(Transform parentTransform, Vector2 position)
        {
            return ghostAPrefab.CloneMe(parentTransform, position);
        }

        IGhost ICharactersFactory.CreateGhostB(Transform parentTransform, Vector2 position)
        {
            return ghostBPrefab.CloneMe(parentTransform, position);
        }
    }
}
