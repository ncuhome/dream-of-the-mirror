using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public string storeName;
    public int areaIndex = -1;

    public void EndDialogue()
    {
        if (areaIndex > -1)
        {
            AreaManager.instance.DestroyDoor(areaIndex);
        }
    }
}
