using UnityEngine;
using UnityEngine.Events;
namespace Game.View
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
        [SerializeField]
        PacMan _pacManPrefab;
        [SerializeField]
        GhostA _ghostAPrefab;
        [SerializeField]
        GhostB _ghostBPrefab;

        // ========== ICharactersFactory ============

        IPacMan ICharactersFactory.CreatePacMan(Transform parentTransform, Vector2 position, UnityEvent unityEvent)
        { return _pacManPrefab.CloneMe(parentTransform, position, unityEvent); }

        IGhostA ICharactersFactory.CreateGhostA(Transform parentTransform, Vector2 position)
        { return _ghostAPrefab.CloneMe(parentTransform, position); }

        IGhostB ICharactersFactory.CreateGhostB(Transform parentTransform, Vector2 position)
        { return _ghostBPrefab.CloneMe(parentTransform, position); }
    }
}
