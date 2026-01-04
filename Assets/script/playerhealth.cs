using TMPro;
using UnityEngine;

public class playerhealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public TMP_Text healthtext;
    public Animator healthtextanim;

    private bool isDead = false;

    void Start()
    {
        StatsManager stats = StatsManager.Instance;

        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthtextanim.Play("textupdate");
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("PLAYER DEAD");

        GetComponent<playermovement1>().enabled = false;
        GetComponent<player_combat>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Destroy(gameObject, 2f);
    }

    void UpdateUI()
    {
        healthtext.text = "HP: " + currentHealth + " / " + maxHealth;
    }
}
