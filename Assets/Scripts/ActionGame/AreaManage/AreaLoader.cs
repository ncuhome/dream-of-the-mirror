using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoader : MonoBehaviour
{
    [Tooltip("是否为装载器/卸载器")]
    public bool isLoader=false;
    public int loadIndex;
    public bool isUnloader = false;
    public int unloadIndex;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!isLoader) return;
        if (collision.transform.tag == "Player") {
            AreaManager.GetAreaObjectByIndex(loadIndex).SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!isUnloader) return;
        if (collision.transform.tag == "Player") {
            AreaManager.GetAreaObjectByIndex(unloadIndex).SetActive(false);
        }
    }
}
