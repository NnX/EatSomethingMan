using UnityEngine;

namespace Src.View
{
    public interface IGhost
    {
        void UpdatePosition(Vector2 position, float time);
        bool IsScared { set; }
        bool IsActive { get; set; }
        void UpdateSprite(Sprite sprite);
    }
}

