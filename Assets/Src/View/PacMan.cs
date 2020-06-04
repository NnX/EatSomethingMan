using UnityEngine;
using WCTools;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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
        UnityEvent _unityEvent;
        public IPacMan CloneMe(Transform parent, Vector2 position, UnityEvent unityEvent)
        {
            GameObject newObj = Instantiate(gameObject, parent);
            BoxCollider2D boxCollider = newObj.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            Rigidbody2D rigid = newObj.AddComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            PacMan pacMan = newObj.GetComponent<PacMan>();
            pacMan.transform.localPosition = position;
            pacMan._unityEvent = unityEvent;
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

            switch (other.name) {
                case "Coin(Clone)":
                    CoinCounter++;
                    if (CoinCounter == Constant.FieldSize - 1)
                    {
                        Debug.Log("YOU WIN!!!");
                        SceneManager.LoadScene("win", LoadSceneMode.Single);
                    }
                    break;
                case "Cherry(Clone)":
                    /* GetComponent<PacMan>()?.transform.GetComponent<VisualManager>()?.OnCherryConsumed(); //To use this case we should add visual manager 
                    GetComponent<VisualManager>()?.OnCherryConsumed();                                      //script to PacMan prefab in the inspector, and this smells not good */
                    GetComponent<PacMan>()._unityEvent.Invoke(); // TODO find a better way to toss this event in GameMediator
                    break;
                default:
                    Debug.Log("PacMan trigger = " + other.name);
                    break;
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
