using UnityEngine;

namespace Core.SkillsAndConditions
{
    public abstract class SkillBase : MonoBehaviour
    {
        public int minLevel;

        public abstract string GetDescription(int level);
        public abstract string GetLevelupDescription(int level);
        public abstract int GetMaxLevel();
    }
}
