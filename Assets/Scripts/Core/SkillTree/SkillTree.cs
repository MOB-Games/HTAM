using UnityEditor;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public GameObject skillTreeNode;
    private void Start()
    {
        PrefabUtility.InstantiatePrefab(skillTreeNode, gameObject.transform);
    }
    
}
