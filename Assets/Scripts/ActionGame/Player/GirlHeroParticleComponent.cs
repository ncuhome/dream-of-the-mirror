using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHeroParticleComponent : MonoBehaviour
{

    [Header("粒子")]
    public GameObject dustEffect;
    public GameObject sparkEffect;

    private ParticleSystem dust;
    // private bool spawnLandDust = false;

    void Start()
    {
        var dustPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        var dustPrefab = Instantiate(dustEffect, dustPos, Quaternion.identity);
        dustPrefab.transform.SetParent(transform);
        dust = dustPrefab.GetComponent<ParticleSystem>();   
    }

    public void CreateDust()
    {
        dust.Play();
    }

    public void CreateSpark(Vector2 damagePos)
    {
        Instantiate(sparkEffect, damagePos, Quaternion.identity);
    }
}
