using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IndexBorder
{
    public Vector2 leftTopPoint;
    public Vector2 rightBottomPoint;
};

public class AreaManager : MonoBehaviour
{
    public List<GameObject> AreaList;
    public static AreaManager instance=null;

    // [Header("各区域边界")]
    [SerializeField]
    private IndexBorder[] indexBorders;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("More than one AreaManager Working,it must be a singleton.");
        }
    }

    public static bool CheckInstance() {
        return instance != null;
    }

    public static GameObject GetAreaObjectByIndex(int index) {
        if (!CheckInstance()) {
            Debug.LogError("No AreaManager Working,you must set a Instance.");  
        }
        return instance.AreaList[index];
    }

    public IndexBorder GetBorder(int index)
    {
        return indexBorders[index];
    }
}
