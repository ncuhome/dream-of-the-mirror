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
    public List<GameObject> areaList;
    public List<GameObject> doorList;
    public static AreaManager instance=null;

    [SerializeField] private List<IndexBorder> indexBorders;

    private int currentIndex;

    public int CurrentIndex
    {
        get{
            return currentIndex;
        }
    }

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
        return instance.areaList[index];
    }

    public IndexBorder GetBorder()
    {
        return indexBorders[currentIndex];
    }

    // public IndexBorder GetBorder(int index)
    // {
    //     return indexBorders[index];
    // }

    public void CalAreaIndex(Vector2 pos)
    {
        int index = 0;
        while (index < indexBorders.Count)
        {
            if (pos.x > indexBorders[index].leftTopPoint.x && pos.x < indexBorders[index].rightBottomPoint.x
                && pos.y > indexBorders[index].rightBottomPoint.y && pos.y < indexBorders[index].leftTopPoint.y)
            {
                currentIndex = index;
                return;
            }
            index++;
        }
        Debug.Log("The pos is not in scene:" + pos);
    }

    public void DestroyDoor(int index)
    {
        doorList[index].SetActive(false);
    }
}
