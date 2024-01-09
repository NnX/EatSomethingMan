using UnityEngine;

namespace Src.View
{
    public interface IPositionManager
    {
        Vector2 GetPosition(int x, int y);
    }

    // #########################################

    public class PositionManager : MonoBehaviour, IPositionManager
    {
        [SerializeField] Transform _leftBottomMarker;
        [SerializeField] Transform _rightTopMarker;

        [Space] [SerializeField] int _width; 
        [SerializeField] int _height;

        // =========== IPositionManager ========

        Vector2 IPositionManager.GetPosition(int x, int y)
        {
            var resultX = Mathf.Lerp(_leftBottomMarker.localPosition.x, _rightTopMarker.localPosition.x, x / (float)(_width - 1));
            var resultY = Mathf.Lerp(_leftBottomMarker.localPosition.y, _rightTopMarker.localPosition.y, y / (float)(_height - 1));
            return new Vector2(resultX, resultY);
        }
    }
}