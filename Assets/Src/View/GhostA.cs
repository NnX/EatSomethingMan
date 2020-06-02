using Game.Misc;
using UnityEngine;
using WCTools;
using UnityEngine.SceneManagement;
namespace Game.View
{ 
    public interface IGhostA
    {
        void UpdatePosition(Vector2 position, float time);
        void Rotate(float degrees);
    }

    // #########################################

    public class GhostA : MonoBehaviour, IGhostA
    {
        int CoinCounter = 0;
        public IGhostA CloneMe(Transform parent, Vector2 position)
        {
            GameObject newObj = Instantiate(gameObject, parent);
            BoxCollider2D boxCollider = newObj.AddComponent<BoxCollider2D>();

            //Rigidbody2D rigid = newObj.AddComponent<Rigidbody2D>();
            GhostA ghostA = newObj.GetComponent<GhostA>();
            ghostA.transform.localPosition = position;

                
            return ghostA;
        }

        // ===================================

        CoroutineInterpolator _positionInterp;

        void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        // ========== IPacMan ================

        void IGhostA.UpdatePosition(Vector2 position, float time)
        {
                _positionInterp.Interpolate(transform.localPosition, position, time,
                    (Vector2 pos) =>
                    {
                        transform.localPosition = pos;
                    });
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("IGhostA trigger = " + other.name);
            if (other.name.Equals("PacMan(Clone)"))
            {
                    Debug.Log("IGhostA Haha, GAME OVER!!!");
                SceneManager.LoadScene("lost", LoadSceneMode.Single);
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
