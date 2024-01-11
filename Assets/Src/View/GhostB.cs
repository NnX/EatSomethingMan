using UnityEngine;
using UnityEngine.SceneManagement;
using WCTools;

namespace Src.View
{ 
    public class GhostB : MonoBehaviour, IGhost
    {
        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        bool IGhost.IsScared { set => _isScared = value; }
        
        private bool _isScared;
        private SpriteRenderer _spriteRenderer;
        private CoroutineInterpolator _positionInterp;

        public IGhost CloneMe(Transform parent, Vector2 position)
        {
            var objectGhostB = Instantiate(gameObject, parent);
            objectGhostB.AddComponent<BoxCollider2D>();

            var rigid = objectGhostB.AddComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            if (objectGhostB.TryGetComponent<GhostB>(out var ghostB))
            {
                ghostB.transform.localPosition = position; 
            }

            return ghostB;
        }

        public void UpdateSprite(Sprite sprite)
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
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
                if(_isScared)
                {
                    print("[GhostB]Om nom nom");
                    gameObject.SetActive(false);
                } else
                {
                    Debug.Log("Ghost B Haha, GAME OVER!!!");
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
