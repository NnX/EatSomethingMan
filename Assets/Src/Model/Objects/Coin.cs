using Src.View;
using UnityEngine;

namespace Src.Model.Objects
{
    public class Coin : MonoBehaviour
    {
        public int X { get; set; }
        public int Y { get; set; } 
        public CoinCollectedEvent coinEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PacMan>(out _))
            {
                coinEvent.Invoke(X,Y);
                Destroy(gameObject); 
            }
        } 
    }
}
