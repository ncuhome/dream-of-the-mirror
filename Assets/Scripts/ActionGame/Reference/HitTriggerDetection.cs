using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTriggerDetection : MonoBehaviour
{
    public DevilPhysicsComponent physics;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            physics.EnemyHit(other);
        }   
    }
}
