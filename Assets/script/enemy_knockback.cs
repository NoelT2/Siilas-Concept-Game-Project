using System.Collections;
using UnityEngine;

public class enemy_knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private enemy_movement enemy_Movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy_Movement = GetComponent<enemy_movement>();
        
    }
    public void Knockback(Transform playerTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        enemy_Movement.isKnockedBack = true;
        StartCoroutine(KnockbackRoutine(knockbackTime, stunTime));
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        Debug.Log("knockback applied.");
    }

    
    IEnumerator KnockbackRoutine(float knockbackTime, float stunTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(stunTime);
        enemy_Movement.isKnockedBack = false;
    }
}
