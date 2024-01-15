using UnityEngine;

namespace Game.Model
{
    [CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/NewLevelData", order = 1)]
    public class LevelModelObject : ScriptableObject
    {
        [SerializeField] public ModelBase.WallData wallsData;

        public ModelBase.WallData GetWalls()
        {  
            return wallsData;
        }
        
    }
}