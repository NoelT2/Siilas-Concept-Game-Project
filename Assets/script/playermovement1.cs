using System.Collections;
using UnityEngine;

public class playermovement1 : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    public int facingDirection = -1;
    public player_combat player_Combat;
    private bool isKnockBacked;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();

        StatsManager stats = StatsManager.Instance;
        if (stats != null)
        {
            speed = stats.moveSpeed;
        }
        
    }
    void Update()
    {
        if (Input.GetButtonDown("attack1"))
        {
            player_Combat.Attack();
        }
        if (Input.GetButtonDown("parry"))
        {
            player_Combat.Parry();
        }
        if (Input.GetButtonDown("skill1"))
        {
            player_Combat.Skill1();
        }
        if (Input.GetButtonDown("skill2"))
        {
            player_Combat.Skill2();
        }
        if (Input.GetButtonDown("skill3"))
        {
            player_Combat.Skill3();
        }
        if (Input.GetButtonDown("dashf"))
        {
            player_Combat.Dashf();
        }
        if (Input.GetButtonDown("dashb"))
        {
            player_Combat.Dashb();
        }
        if (Input.GetButtonDown("guard1"))
        {
            player_Combat.Guard();
        }
    }

    private void FixedUpdate()
    {
        if (isKnockBacked || player_Combat.currentState != player_combat.CombatState.Idle) return;
        
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if ((horizontal > 0 && facingDirection == -1) ||
                (horizontal < 0 && facingDirection == 1))
            {
                Flip();
            }

            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
        
    }

    void Flip()
    {
        facingDirection *= -1;
        sr.flipX = facingDirection == 1;
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockBacked = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force;
        StartCoroutine(knockbackCounter(stunTime));
    }

    IEnumerator knockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockBacked = false;
    }

    public void RefreshStats()
    {
        speed = StatsManager.Instance.moveSpeed;
    }
    
}
