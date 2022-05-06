using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public Vector2 attackRange;
    public Vector2 closeWalkRange;
    public Vector2 shootRange;
    public Vector2 remoteWalkRange;

    private Command attack;
    private Command shoot;
    private Command teleport;
    private Command walk;
    private Command weak;
    private TranslationCommand move;
    private EnemyAttackConsciousness enemyAttackConsciousness;

    void Start()
    {
        attack = new SwordCommand();
        shoot = new ShootCommand();
        teleport = new TeleportCommand();
        walk = new WalkCommand();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
    }

    public TranslationCommand HandleTranslationInput()
    {
        move = new MoveCommand(enemyAttackConsciousness.WalkDir(), 0);
        return move;
    }

    public Command HandleActionInput()
    {
        if (weak != null)
        {
            return weak;
        }
        if (enemyAttackConsciousness.HeroDistance() <= attackRange.y)
        {
            return attack;
        }
        if (enemyAttackConsciousness.HeroDistance() <= closeWalkRange.y)
        {
            return walk;
        }
        if (enemyAttackConsciousness.HeroDistance() <= shootRange.y)
        {
            return shoot;
        }
        if (enemyAttackConsciousness.HeroDistance() <= remoteWalkRange.y)
        {
            return walk;
        }
        return null;
    }

    public void SetWeak()
    {
        if (weak == null)
        {
            walk = new WeakCommand();
        }
    }

    public void DestroyWeak()
    {
        if (weak == null)
        {
            walk = null;
        }
    }
}
