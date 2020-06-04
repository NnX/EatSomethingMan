using Game.Misc;
using UnityEngine;
using WCTools;
using UnityEngine.SceneManagement;

namespace Game.View
{ 
    public interface IGhostB
    {
        void UpdatePosition(Vector2 position, float time);
        bool isScared { set; }
    }

    // #########################################

    public class GhostB : MonoBehaviour, IGhostB
    {
        int CoinCounter = 0;
        bool _isScared;
        GameObject _objectGhostB;
        public IGhostB CloneMe(Transform parent, Vector2 position)
        {
            _objectGhostB = Instantiate(gameObject, parent);
            BoxCollider2D boxCollider = _objectGhostB.AddComponent<BoxCollider2D>();

            Rigidbody2D rigid = _objectGhostB.AddComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            GhostB ghostB = _objectGhostB.GetComponent<GhostB>();
            ghostB.transform.localPosition = position;

                
            return ghostB;
        }

        // ===================================

        CoroutineInterpolator _positionInterp;

        bool IGhostB.isScared { set => _isScared = value; }

        void Awake()
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

        void OnTriggerEnter2D(Collider2D other)
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
