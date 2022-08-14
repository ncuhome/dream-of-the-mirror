using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public string triggerName;
    public GameObject enemy;
    
    public void MakeEnemyMove()
    {
        switch(triggerName)
        {
            case "deathBegin":
                enemy.GetComponent<Death>().IsEndDialogue = true;
                break;
            case "devilBegin":
                enemy.GetComponent<Devil>().IsEndDialogue = true;
                AreaManager.instance.SetDoor(0);
                break;
        }
    }
}
