using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public Vector2 attackRange;
    public Vector2 closeWalkRange;
    public Vector2 shootRange;
    public Vector2 remoteWalkRange;

    private ActionCommand attack;
    private ActionCommand shoot;
    // private ActionCommand teleport;
    private ActionCommand walk;
    private ActionCommand weak;
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

    void Start()
    {
        move = new MoveCommand(0, 0);
        attack = ActionCommand.Sword;
        shoot = ActionCommand.Shoot;
        // teleport = ActionCommand.Teleport;
        walk = ActionCommand.Walk;
        weak = ActionCommand.None;
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
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
            return weak;
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
            return shoot;
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
        weak = ActionCommand.Weak;
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
