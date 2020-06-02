using UnityEngine;

public class cherry : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("PacMan(Clone)"))
        {
            Destroy(this.gameObject);
        }
    }
}
