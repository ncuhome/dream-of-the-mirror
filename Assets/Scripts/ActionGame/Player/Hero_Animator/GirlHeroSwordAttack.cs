using UnityEngine;

public class GirlHeroSwordAttack : MonoBehaviour
{
    public int attackDamage = 4;
    //攻击圆中心偏移点
    public Vector2 attackOffset;
    //攻击人物坐标
    public Transform attackPoint;
    //攻击圆半径
    public float attackRange = 1f;
    //攻击圆图层
    public LayerMask attackMask;

    public void Attack()
    {
        Vector2 pos = transform.position;
        pos += (Vector2)transform.right * attackOffset.x;
        pos += (Vector2)transform.up * attackOffset.y;

        Collider2D [] colInfo = Physics2D.OverlapCircleAll(pos, attackRange, attackMask);
        if (colInfo.Length != 0)
        {
            foreach (Collider2D col in colInfo)
            {
                //避免同类自残
                if (col.gameObject.tag == "Player")
                {
                    continue;
                }
                if (col.GetComponent<Health>() != null)
                {
                    Vector2 damageDir;
                    damageDir = ((Vector2)col.transform.position - (Vector2)transform.position).normalized;
                    col.GetComponent<Health>().TakeDamage(attackDamage, damageDir);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere((Vector2)attackPoint.position + attackOffset, attackRange); 
    }
}
