using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    public float attackRange = 2;
    public float speed;
    public float attackCooldown = 2;
    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerlayer;
    public Transform attackPoint;
    private float attackCooldownTimer;
    private int facingDirection = -1;
    private Animator anim;
    private EnemyState enemyState;
    public bool isKnockedBack;
    public bool isParrying;
    public float parryChance = 0.4f;
    public float parryCooldown = 2f;
    private float parryTimer;
    public float parryDuration = 0.5f;
    private float parryStateTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (parryTimer > 0)
        parryTimer -= Time.deltaTime;

        if (enemyState == EnemyState.Parry)
        {
            rb.linearVelocity = Vector2.zero;
            parryStateTimer -= Time.deltaTime;

            if (parryStateTimer <= 0)
                ChangeState(EnemyState.Idle);

            return;
        }

        if (enemyState == EnemyState.Attacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isKnockedBack) return;

        CheckForPlayer();

        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        else if (enemyState == EnemyState.Chasing)
            Chase();
    }

    void Chase()
    {
        if ((player.position.x > transform.position.x && facingDirection == -1) || 
            (player.position.x < transform.position.x && facingDirection == 1))
            {
                Flip();
            }
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
    }
    void Flip()
    {
        facingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }
    void CheckForPlayer()
    {
        if (enemyState == EnemyState.Parry) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerlayer);
            if (hits.Length == 0)
            {
                ChangeState(EnemyState.Idle);
                rb.linearVelocity = Vector2.zero;
                return;
            }

            player = hits[0].transform;
            float dist = Vector2.Distance(attackPoint.position, player.position);

            if (dist <= attackRange && enemyState == EnemyState.Idle)
            {
                TryParry();
                if (enemyState == EnemyState.Parry) return;
            }
            else if (dist <= attackRange && attackCooldownTimer <= 0)
            {
                FacePlayer();
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            else if (dist > attackRange)
            {
                ChangeState(EnemyState.Chasing);
            }
    }

    public void ChangeState(EnemyState newState)
    {
        if (enemyState == EnemyState.Parry && newState != EnemyState.Idle)
            return;
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsChasing", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsParry", false);

        enemyState = newState;

        switch (enemyState)
        {
            case EnemyState.Idle:
                isParrying = false;
                anim.SetBool("IsIdle", true);
                break;

            case EnemyState.Chasing:
                anim.SetBool("IsChasing", true);
                break;

            case EnemyState.Attacking:
                anim.SetBool("IsAttacking", true);
                break;

            case EnemyState.Parry:
                isParrying = true;
                anim.SetBool("IsParry", true);
                break;
        }
    }
    public void FacePlayer()
    {
        if (!player) return;

        if (player.position.x > transform.position.x && facingDirection == -1)
            Flip();
        else if (player.position.x < transform.position.x && facingDirection == 1)
            Flip();
    }


    public void TryParry()
    {
        Debug.Log("try parry");
        if (enemyState != EnemyState.Idle) return;
        if (parryTimer > 0) return;

        if (Random.value <= parryChance)
        {
             Debug.Log("parry");
            FacePlayer();
            ChangeState(EnemyState.Parry);
            parryTimer = parryCooldown;
            parryStateTimer = parryDuration;
        }
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
    Parry,
}