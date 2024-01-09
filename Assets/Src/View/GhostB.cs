using UnityEngine;
using UnityEngine.SceneManagement;
using WCTools;

namespace Src.View
{ 
    public interface IGhostB
    {
        void UpdatePosition(Vector2 position, float time); // TODO make one interface for ghosts
        bool IsScared { set; }
        bool IsActive { get; set; }
        void UpdateSprite(Sprite sprite);
    }

    // #########################################

    public class GhostB : MonoBehaviour, IGhostB
    {
        private int _coinCounter = 0;
        private bool _isScared;
        private SpriteRenderer _spriteRenderer;

        public IGhostB CloneMe(Transform parent, Vector2 position)
        {
            var objectGhostB = Instantiate(gameObject, parent);
            objectGhostB.AddComponent<BoxCollider2D>();

            var rigid = objectGhostB.AddComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            var ghostB = objectGhostB.GetComponent<GhostB>();
            ghostB.transform.localPosition = position;

            return ghostB;
        }

        // ===================================

        CoroutineInterpolator _positionInterp;

        bool IGhostB.IsScared { set => _isScared = value; }
        public bool IsActive { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

        public void UpdateSprite(Sprite sprite)
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
        }

        private void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        // ========== IPacMan ================

        void IGhostB.UpdatePosition(Vector2 position, float time)
        {
            if(this.gameObject.activeSelf)
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
            if (other.name == "PacMan(Clone)")
            {
                if(_isScared)
                {
                    print("[GhostB]Om nom nom");
                    this.gameObject.SetActive(false);
                } else
                {
                    Debug.Log("Ghost B Haha, GAME OVER!!!");
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
