using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("PacMan(Clone)"))
        {
            Destroy(this.gameObject);
        }
    } 
}
