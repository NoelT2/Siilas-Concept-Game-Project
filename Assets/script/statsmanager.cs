using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [Header("Combat Stats")]
    public int damage = 1;
    public float attackCooldown = 2f;
    public float knockbackForce = 20f;
    public float knockbackTime = 0.2f;
    public float stunTime = 1f;

    [Header("Movement Stats")]
    public float moveSpeed = 5f;
    public float dashForce = 10f;

    [Header("Health Stats")]
    public int maxHealth = 10;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
