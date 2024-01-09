using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WCTools;

namespace Src.View
{ 
    public interface IPacMan
    {
        void UpdatePosition(Vector2 position, float time);
        void Rotate(float degrees);
    }

    // #########################################

    public class PacMan : MonoBehaviour, IPacMan
    {
        private int _coinCounter;
        private UnityEvent _unityEvent;
        
        public IPacMan CloneMe(Transform parent, Vector2 position, UnityEvent unityEvent)
        {
            var newObj = Instantiate(gameObject, parent);
            var boxCollider = newObj.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            var rigid = newObj.AddComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            var pacMan = newObj.GetComponent<PacMan>();
            pacMan.transform.localPosition = position;
            pacMan._unityEvent = unityEvent;
            return pacMan;
        }

        // ===================================

        private CoroutineInterpolator _positionInterp;

        private void Awake()
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

        private void OnTriggerEnter2D(Collider2D other)
        {

            switch (other.name) {
                case "Coin(Clone)":
                    _coinCounter++;
                    if (_coinCounter == Constant.FieldSize - 1)
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
