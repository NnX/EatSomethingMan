using UnityEngine;

namespace Src.Model.Objects
{
    public class Cherry : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name.Equals("PacMan(Clone)"))
            {
                Destroy(gameObject);
            }
        }
    }
}
