using UnityEngine;

public class enemy_health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.SpawnEnemies(transform.position, 2);
        }

        DisableEnemy();

        Destroy(gameObject, 0.1f);
    }

    void DisableEnemy()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;

        var attack = GetComponent<enemy_combat>();
        if (attack) attack.enabled = false;
    }
}
