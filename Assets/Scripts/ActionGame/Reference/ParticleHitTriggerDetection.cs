using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHitTriggerDetection : MonoBehaviour
{
    private DevilPhysicsComponent physics;

    public DevilPhysicsComponent Physics
    {
        set{
            physics = value;
        }
    }

    private void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        Component other = ps.trigger.GetCollider(0);
        if (other.gameObject.tag == "Player" && other.GetComponent<Collider2D>() != null)
        {
            physics.EnemyHit(other.GetComponent<Collider2D>());
        }
    }
}
