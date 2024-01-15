using UnityEngine;

namespace Game.Model
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private LevelModelObject[] levels;
 
        public LevelModelObject GetFirstLevel()
        {
            if (levels.Length > 0)
            {
                return levels[0];
            }

            Debug.LogError("Error, no levels data");
            return null;
        }
    }
}
