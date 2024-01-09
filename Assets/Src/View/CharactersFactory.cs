using UnityEngine;
using UnityEngine.Events;

namespace Src.View
{ 
    public interface ICharactersFactory
    {
        IPacMan CreatePacMan(Transform parentTransform, Vector2 position, UnityEvent unityEvent);
        IGhostA CreateGhostA(Transform parentTransform, Vector2 position);
        IGhostB CreateGhostB(Transform parentTransform, Vector2 position);
    }

    // ##############################################

    public class CharactersFactory : MonoBehaviour, ICharactersFactory
    {
        [SerializeField] private PacMan pacManPrefab;
        [SerializeField] private GhostA ghostAPrefab;
        [SerializeField] private GhostB ghostBPrefab;

        // ========== ICharactersFactory ============

        IPacMan ICharactersFactory.CreatePacMan(Transform parentTransform, Vector2 position, UnityEvent unityEvent)
        {
            return pacManPrefab.CloneMe(parentTransform, position, unityEvent);
        }

        IGhostA ICharactersFactory.CreateGhostA(Transform parentTransform, Vector2 position)
        {
            return ghostAPrefab.CloneMe(parentTransform, position);
        }

        IGhostB ICharactersFactory.CreateGhostB(Transform parentTransform, Vector2 position)
        {
            return ghostBPrefab.CloneMe(parentTransform, position);
        }
    }
}
