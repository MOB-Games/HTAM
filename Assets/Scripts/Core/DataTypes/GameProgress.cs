using UnityEngine;

namespace Core.DataTypes
{
    [CreateAssetMenu]
    public class GameProgress : ScriptableObject
    {
        public int currentPath;
        public int currentStage;
        public int lastTown;
        public int maxClearedPath;

        public void Init()
        {
             currentPath = lastTown = 0;
             maxClearedPath = currentStage =-1;
        }
    }
}
