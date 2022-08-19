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

        public void Init(SaveSlot saveSlot = null)
        {
            if (saveSlot == null)
            {
                 currentPath = lastTown = 0;
                 maxClearedPath = currentStage =-1;
            }
            else
            {
                currentPath = lastTown = saveSlot.currentPath;
                maxClearedPath = saveSlot.maxClearedPath;
                currentStage = -1;
            }
        }
    }
}
