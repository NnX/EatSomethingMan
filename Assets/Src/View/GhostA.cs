using Game.Misc;
using UnityEngine;
using WCTools;
using UnityEngine.SceneManagement;

namespace Game.View
{ 
    public interface IGhostA
    {
        void UpdatePosition(Vector2 position, float time); // TODO make one interface for ghosts
        bool isScared { set; }
        bool isActive { get;  set; }
        void UpdateSprite(Sprite sprite);
    }

    // #########################################

    public class GhostA : MonoBehaviour, IGhostA
    {
        int CoinCounter = 0;
        bool _isScared;
        SpriteRenderer _spriteRenderer;

        public IGhostA CloneMe(Transform parent, Vector2 position)
        {
            GameObject _gameObjectGhostA = Instantiate(gameObject, parent);
            BoxCollider2D boxCollider = _gameObjectGhostA.AddComponent<BoxCollider2D>();

            GhostA ghostA = _gameObjectGhostA.GetComponent<GhostA>();
            _spriteRenderer = _gameObjectGhostA.GetComponent<SpriteRenderer>();
            ghostA.transform.localPosition = position;

            return ghostA;
        }

        // ===================================

        CoroutineInterpolator _positionInterp;

        bool IGhostA.isScared { set => _isScared = value; }
        public bool isActive { get => this.gameObject.activeSelf;  set => this.gameObject.SetActive(value); }

        public void UpdateSprite(Sprite sprite) {
            SpriteRenderer _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = sprite;
        }

        void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        // ========== IPacMan ================

        void IGhostA.UpdatePosition(Vector2 position, float time)
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

        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.name.Equals("PacMan(Clone)"))
            {
                if (_isScared)
                {
                    print("[GhostA] Om nom nom");
                    this.gameObject.SetActive(false);
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
