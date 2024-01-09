using UnityEngine;
using UnityEngine.SceneManagement;
using WCTools;

namespace Src.View
{ 
    public interface IGhostA
    {
        void UpdatePosition(Vector2 position, float time); // TODO make one interface for ghosts
        bool IsScared { set; }
        bool IsActive { get;  set; }
        void UpdateSprite(Sprite sprite);
    }

    // #########################################

    public class GhostA : MonoBehaviour, IGhostA
    {
        /*private int _coinCounter = 0;*/
        private bool _isScared;
        /*private SpriteRenderer _spriteRenderer;*/

        public IGhostA CloneMe(Transform parent, Vector2 position)
        {
            var gameObjectGhostA = Instantiate(gameObject, parent).AddComponent<BoxCollider2D>();

            if(gameObjectGhostA.TryGetComponent<GhostA>(out var ghostA))
            {
                ghostA.transform.localPosition = position;  
            }
            /*if(gameObjectGhostA.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                _spriteRenderer = spriteRenderer;
            }*/

            return ghostA;
        }

        // ===================================

        private CoroutineInterpolator _positionInterp;

        bool IGhostA.IsScared { set => _isScared = value; }
        public bool IsActive { get => gameObject.activeSelf;  set => gameObject.SetActive(value); }

        public void UpdateSprite(Sprite sprite)
        {
            if (gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                
                spriteRenderer.sprite = sprite;
            }
            //_spriteRenderer = spriteRenderer;
        }

        private void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        // ========== IPacMan ================

        void IGhostA.UpdatePosition(Vector2 position, float time)
        {
            if(gameObject.activeSelf)
            {
                _positionInterp.Interpolate(transform.localPosition, position, time,
                    (Vector2 pos) =>
                    {
                        transform.localPosition = pos;
                    });
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.name.Equals("PacMan(Clone)"))
            {
                if (_isScared)
                {
                    print("[GhostA] Om nom nom");
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("IGhostA Haha, GAME OVER!!!");
                    SceneManager.LoadScene("lost", LoadSceneMode.Single);

                }
            }
        }

        public void Rotate(float degrees)
        {
            if(degrees == 180 || degrees == 0)
            {
                transform.rotation = Quaternion.Euler(0,degrees,0); // flip 
            } else
            {
                transform.rotation = Quaternion.Euler(0, 0, degrees);
            }
        }
 
    }
}
