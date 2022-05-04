using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Health : MonoBehaviour
{
    public virtual void TakeDamage(Damage damage) {}
    public virtual void Die() {}
}