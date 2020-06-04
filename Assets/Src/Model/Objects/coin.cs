using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.View;

public class coin : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; } 
    public CoinCollectedEvent _coinEvent;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("PacMan(Clone)"))
        {
            _coinEvent.Invoke(X,Y);
            Destroy(this.gameObject);
        }
    } 
}
