using System;
using Src.Misc;
using Src.Model.Objects;
using UnityEngine;
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
        private Action _onCherryConsumed;
        private CoroutineInterpolator _positionInterp;

        public IPacMan CloneMe(Transform parent, Vector2 position, Action onCherryConsumed)
        {
            var newObj = Instantiate(gameObject, parent);
            EnsurePhysicsSetup(newObj);
            if (newObj.TryGetComponent<PacMan>(out var pacMan))
            {
                pacMan.transform.localPosition = position;
                pacMan._onCherryConsumed = onCherryConsumed;
            }

            return pacMan;
        }

        private static void EnsurePhysicsSetup(GameObject pacManObject)
        {
            if (!pacManObject.TryGetComponent<Collider2D>(out var collider))
            {
                collider = pacManObject.AddComponent<BoxCollider2D>();
            }

            collider.isTrigger = true;

            if (!pacManObject.TryGetComponent<Rigidbody2D>(out var rigidBody))
            {
                rigidBody = pacManObject.AddComponent<Rigidbody2D>();
            }

            rigidBody.bodyType = RigidbodyType2D.Kinematic;
        }

        private void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        void IPacMan.UpdatePosition(Vector2 position, float time)
        {
            var startPosition = transform.localPosition;
            var targetPosition = new Vector3(position.x, position.y, startPosition.z);

            _positionInterp.Interpolate(startPosition, targetPosition, time,
                (Vector3 pos) =>
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
                _onCherryConsumed?.Invoke();
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
            transform.rotation = degrees is 180 or 0 ? Quaternion.Euler(0, degrees, 0) : Quaternion.Euler(0, 0, degrees);
        }
    }
}
