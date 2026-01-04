using UnityEngine;

public class enemy_combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public LayerMask playerLayer;
    public float knockbackForce;
    public float stunTime;
   
    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        if (hits.Length > 0)
        {
            hits[0].GetComponent<playerhealth>().ChangeHealth(-damage);
            hits[0].GetComponent<playermovement1>().Knockback(transform, knockbackForce, stunTime);
        }
    }
}
