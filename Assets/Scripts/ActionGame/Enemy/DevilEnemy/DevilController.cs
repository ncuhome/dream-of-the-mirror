using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilController : MonoBehaviour
{
    public Vector2 attackRange;
    public Vector2 closeWalkRange;
    public Vector2 shootRange;
    public Vector2 remoteWalkRange;

    private ActionCommand attack;
    private ActionCommand shoot;
    private ActionCommand walk;
    private ActionCommand weak;
    private ActionCommand sprint;
    private MoveCommand move;
    private MoveCommand ? repel;
    public MoveCommand Repel
    {
        get
        {
            return repel.Value;
        }
    }
    private EnemyAttackConsciousness enemyAttackConsciousness;
    private DevilHealth devilHealth;
    private bool firstJump = false;
    private bool secondJump = false;

    void Start()
    {
        move = new MoveCommand(0, 0);
        attack = ActionCommand.Sword;
        shoot = ActionCommand.Shoot;
        sprint = ActionCommand.Sprint;
        walk = ActionCommand.Walk;
        weak = ActionCommand.None;
        devilHealth = GetComponent<DevilHealth>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
        firstJump = false;
        secondJump = false;
    }

    void Update()
    {
        if ((devilHealth.maxHealth * 0.66 >= devilHealth.CurrentHealth))
        {
            if (!firstJump)
            {
                SetWeak();
                firstJump = true;
            }
        }
        if ((devilHealth.maxHealth * 0.33 >= devilHealth.CurrentHealth))
        {
            if (!secondJump)
            {
                SetWeak();
                secondJump = true;
            }
        } 
    }

    public MoveCommand HandleTranslationInput()
    {
        if (repel != null)
        {
            return repel.Value;
        }
        move.horizontal = enemyAttackConsciousness.WalkDir();
        return move;
    }

    public ActionCommand HandleActionInput()
    {
        if (weak != ActionCommand.None)
        {
            DestroyWeak();
            return ActionCommand.Weak;
        }
        if (enemyAttackConsciousness.HeroDistance().magnitude <= attackRange.y)
        {
            return attack;
        }
        if (enemyAttackConsciousness.HeroDistance().magnitude <= closeWalkRange.y)
        {
            return walk;
        }
        if (enemyAttackConsciousness.HeroDistance().magnitude <= shootRange.y)
        {
            int x = Random.Range(0, 3);
            if (x == 0)
            {
                return sprint;
            }
            else
            {
                return shoot;
            }
        }
        if (enemyAttackConsciousness.HeroDistance().magnitude <= remoteWalkRange.y)
        {
            return walk;
        }
        return ActionCommand.None;
    }

    public void SetWeak()
    {
        if (weak == ActionCommand.None)
        {
            weak = ActionCommand.Weak;
        }
    }

    public void DestroyWeak()
    {
        weak = ActionCommand.None;
    }

    public void SetRepel(MoveCommand repel_)
    {
        if (repel_.type == MoveCommand.MoveType.repel)
        {
            repel = repel_;
        }
    }
    public void DestroyRepel()
    {
        repel = null;
    }
}
