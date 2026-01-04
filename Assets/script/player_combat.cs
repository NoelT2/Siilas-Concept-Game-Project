using UnityEngine;

public class player_combat : MonoBehaviour
{
    public Transform attackPoint;
    public float weaponRange = 1;
    public LayerMask enemyLayer;
    public int damage = 1;
    public Animator anim;
    public float cooldown = 2;
    public Vector2 hitboxOffset;
    public float hitboxRadius;
    public float knockbackForce = 20;
    public float stunTime = 1;
    public float knockbackTime = .2f;
    public enum CombatState { Idle, Attacking, Parrying, Sk1, Sk2, Sk3, Guard, Dashb, Dashf }
    public CombatState currentState = CombatState.Idle;
    public playermovement1 movement;
    private float timer;
    private bool hasDealtDamage;
    private StatsManager stats;

    void Start()
    {
        stats = StatsManager.Instance;

        if (stats == null)
        {
            Debug.LogError("StatsManager NOT FOUND");
            enabled = false;
            return;
        }

        damage = stats.damage;
        cooldown = stats.attackCooldown;
        knockbackForce = stats.knockbackForce;
        knockbackTime = stats.knockbackTime;
        stunTime = stats.stunTime;

        currentState = CombatState.Idle;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (currentState == CombatState.Idle)
        {
            if (timer <= 0)
            {
                StopMovement();
                anim.SetTrigger("isAttacking");
                currentState = CombatState.Attacking;
                hasDealtDamage = false;

            

                timer = stats.attackCooldown;
            }
        }
    }
    
    public void DealDamage()
    {
        Debug.Log("DEAL DAMAGE DIPANGGIL");
        
        if (hasDealtDamage) return;

        float facing = Mathf.Sign(transform.localScale.x);

        Vector2 center =
        (Vector2)transform.position + new Vector2(hitboxOffset.x * facing, hitboxOffset.y);

        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(center, hitboxRadius, enemyLayer);
        

        foreach (Collider2D enemy in enemies)
        {
            enemy_movement em = enemy.GetComponentInParent<enemy_movement>();


            if (em != null && em.isParrying)
        {
            movement.Knockback(
                enemy.transform,
                stats.knockbackForce,
                stats.stunTime
            );

            hasDealtDamage = true;
            break;
        }
            enemy.GetComponent<enemy_health>()?.ChangeHealth(-stats.damage);
            enemy.GetComponent<enemy_knockback>()?.Knockback(
                transform,
                stats.knockbackForce,
                stats.knockbackTime,
                stats.stunTime
            );
        }

        if (enemies.Length > 0)
        {
            hasDealtDamage = true;
        }
    }
    public void FinishAttacking()
    {
        currentState = CombatState.Idle;
        movement.rb.linearVelocity = Vector2.zero;
    }
    void OnDrawGizmos()
    {
        if (!attackPoint) return;

        float facing = Mathf.Sign(transform.localScale.x);

        Vector2 center =
            (Vector2)transform.position +
            new Vector2(hitboxOffset.x * facing, hitboxOffset.y);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, hitboxRadius);
    }

    public void SetHitboxByID(float id)
    {
        if(id == 1)
        {
            hitboxOffset = new Vector2(1,0.5f);
            hitboxRadius = 0.3f;
        }
        else if(id == 2)
        {
            hitboxOffset = new Vector2(1.2f,0.6f);
            hitboxRadius = 0.4f;
        }
    }
    public void Parry()
    {
        if (currentState == CombatState.Idle)
        {
            StopMovement();
            anim.SetTrigger("isParrying");
            currentState = CombatState.Parrying;
        }
    }
    public void Skill1()
    {
        if (currentState == CombatState.Idle)
        {

            anim.SetTrigger("isSkill1");
            currentState = CombatState.Sk1;
        }
    }
    public void StopMovementAnim()
    {
        anim.SetFloat("horizontal", 0);
        anim.SetFloat("vertical", 0);
    }
    public void StopMovement()
    {
        if (movement != null)
        {
            movement.rb.linearVelocity = Vector2.zero;
        }
    }
    
    public void Skill2()
    {
        Debug.Log("SKILL2 INPUT");
        if (currentState == CombatState.Idle)
        {

            StopMovement();
            anim.SetTrigger("isSkill2");
            currentState = CombatState.Sk2;
        }
    }
    public void Skill3()
    {
        Debug.Log("s3INPUT");
        if (currentState == CombatState.Idle)
        {

            StopMovement();
            anim.SetTrigger("isSkill3");
            currentState = CombatState.Sk3;
        }
    }
    public void Guard()
    {
        Debug.Log("gINPUT");
        if (currentState == CombatState.Idle)
        {
            StopMovement();
            anim.SetTrigger("isGuard");
            currentState = CombatState.Guard;
        }
    }
    public void Dashf()
    {
        Debug.Log("dfINPUT");
        if (currentState == CombatState.Idle)
        {
            StopMovement();
            anim.SetTrigger("isDashf");
            currentState = CombatState.Dashf;
        }
    }
    public void Dashb()
    {
        Debug.Log("dbINPUT");
        if (currentState == CombatState.Idle)
        {
            StopMovement();
            anim.SetTrigger("isDashb");
            currentState = CombatState.Dashb;
        }
    }
}
