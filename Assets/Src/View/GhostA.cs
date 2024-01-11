using UnityEngine;
using UnityEngine.SceneManagement;
using WCTools;

namespace Src.View
{ 
    public class GhostA : MonoBehaviour, IGhost
    {
        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        bool IGhost.IsScared { set => _isScared = value; }
        
        private CoroutineInterpolator _positionInterp;
        private bool _isScared;
        
        public IGhost CloneMe(Transform parent, Vector2 position)
        {
            var gameObjectGhostA = Instantiate(gameObject, parent).AddComponent<BoxCollider2D>();

            if(gameObjectGhostA.TryGetComponent<GhostA>(out var ghostA))
            {
                ghostA.transform.localPosition = position;  
            }
            return ghostA;
        }

        public void UpdateSprite(Sprite sprite)
        {
            if (gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                
                spriteRenderer.sprite = sprite;
            }
        }

        private void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        void IGhost.UpdatePosition(Vector2 position, float time)
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
            if(other.TryGetComponent<PacMan>(out _))
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
            transform.rotation = degrees is 180 or 0 ? Quaternion.Euler(0,degrees,0) : Quaternion.Euler(0, 0, degrees);
        }
 
    }
}
