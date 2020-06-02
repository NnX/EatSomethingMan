using Game.Misc;
using UnityEngine;
using WCTools;
using UnityEngine.SceneManagement;

namespace Game.View
{ 
    public interface IPacMan
    {
        void UpdatePosition(Vector2 position, float time);
        void Rotate(float degrees);
    }

    // #########################################

    public class PacMan : MonoBehaviour, IPacMan
    {
        int CoinCounter = 0;
        public IPacMan CloneMe(Transform parent, Vector2 position)
        {
            GameObject newObj = Instantiate(gameObject, parent);
            BoxCollider2D boxCollider = newObj.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            Rigidbody2D rigid = newObj.AddComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            PacMan pacMan = newObj.GetComponent<PacMan>();
            pacMan.transform.localPosition = position;
                
            return pacMan;
        }

        // ===================================

        CoroutineInterpolator _positionInterp;

        void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        // ========== IPacMan ================

        void IPacMan.UpdatePosition(Vector2 position, float time)
        {
            _positionInterp.Interpolate(transform.localPosition, position, time,
                (Vector2 pos) =>
                {
                    transform.localPosition = pos;
                });
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name != "Coin(Clone)")
            {
                Debug.Log("PacMan trigger = " + other.name);
            }

            if (other.name == "Coin(Clone)")
            {
                CoinCounter++;
                if(CoinCounter == Constant.FieldSize - 1)
                {
                    Debug.Log("YOU WIN!!!");
                    SceneManager.LoadScene("win", LoadSceneMode.Single);
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
