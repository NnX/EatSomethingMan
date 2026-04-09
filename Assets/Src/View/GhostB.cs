using UnityEngine;
using UnityEngine.SceneManagement;
using WCTools;

namespace Src.View
{
    public class GhostB : MonoBehaviour, IGhost
    {
        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        bool IGhost.IsScared { set => _isScared = value; }

        private bool _isScared;
        private CoroutineInterpolator _positionInterp;

        public IGhost CloneMe(Transform parent, Vector2 position)
        {
            var ghostObject = Instantiate(gameObject, parent);
            if (ghostObject.TryGetComponent<GhostB>(out var ghostB))
            {
                ghostB.transform.localPosition = position;
            }

            return ghostB;
        }

        public void UpdateSprite(Sprite sprite)
        {
            if (gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.sprite = sprite;
            }
        }

        private void Awake()
        {
            _positionInterp = new CoroutineInterpolator(this);
        }

        void IGhost.UpdatePosition(Vector2 position, float time)
        {
            if (gameObject.activeSelf)
            {
                var startPosition = transform.localPosition;
                var targetPosition = new Vector3(position.x, position.y, startPosition.z);

                _positionInterp.Interpolate(startPosition, targetPosition, time,
                    (Vector3 pos) =>
                    {
                        transform.localPosition = pos;
                    });
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PacMan>(out _))
            {
                if (_isScared)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    SceneManager.LoadScene("lost", LoadSceneMode.Single);
                }
            }
        }

        public void Rotate(float degrees)
        {
            transform.rotation = degrees is 180 or 0 ? Quaternion.Euler(0, degrees, 0) : Quaternion.Euler(0, 0, degrees);
        }
    }
}
