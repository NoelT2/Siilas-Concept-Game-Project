using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 1.5f;

    public void SpawnEnemies(Vector2 position, int amount)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab IS NULL or DESTROYED!");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefab, position + offset, Quaternion.identity);
        }
    }
}
