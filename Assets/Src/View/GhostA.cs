using Game.Misc;
using UnityEngine;
using WCTools;
using UnityEngine.SceneManagement;
namespace Game.View
{ 
    public interface IGhostA
    {
        void UpdatePosition(Vector2 position, float time);
        bool isScared { set; }
    }

    // #########################################

    public class GhostA : MonoBehaviour, IGhostA
    {
        int CoinCounter = 0;
        GameObject _objectGhostA;
        bool _isScared;
        public IGhostA CloneMe(Transform parent, Vector2 position)
        {
            _objectGhostA = Instantiate(gameObject, parent);
            BoxCollider2D boxCollider = _objectGhostA.AddComponent<BoxCollider2D>();

            GhostA ghostA = _objectGhostA.GetComponent<GhostA>();
            ghostA.transform.localPosition = position;

                
            return ghostA;
        }

        // ===================================

        CoroutineInterpolator _positionInterp;

        bool IGhostA.isScared { set => _isScared = value; }

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
