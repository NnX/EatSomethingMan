using Src.Misc;
using Src.Model.Objects;
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

    public class PacMan : MonoBehaviour, IPacMan
    {
        private int _coinCounter;
        private UnityEvent _unityEvent;
        private CoroutineInterpolator _positionInterp;
        
        public IPacMan CloneMe(Transform parent, Vector2 position, UnityEvent unityEvent)
        {
            var newObj = Instantiate(gameObject, parent);
            var boxCollider = newObj.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            var rigid = newObj.AddComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            if (newObj.TryGetComponent<PacMan>(out var pacMan))
            {
                pacMan.transform.localPosition = position;
                pacMan._unityEvent = unityEvent;
            }
            return pacMan;
        }

        private void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

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
            if (other.TryGetComponent<Coin>(out _))
            {
                ConsumeCoin();
                return;
            }
            if (other.TryGetComponent<Cherry>(out _))
            {
                _unityEvent.Invoke();
            }
        }

        private void ConsumeCoin()
        {
            _coinCounter++;
            if (_coinCounter == Constant.FieldSize - 1)
            {
                SceneManager.LoadScene("win", LoadSceneMode.Single);
            }
        }

        public void Rotate(float degrees)
        {
            transform.rotation = degrees is 180 or 0 ? Quaternion.Euler(0,degrees,0) : Quaternion.Euler(0, 0, degrees);
        }
    }
}
