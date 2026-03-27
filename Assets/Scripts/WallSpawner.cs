using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject healthPackPrefab;
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 2.5f;
    public float healthPackChance = 0.35f;
    public float enemyChance = 0.45f;

    private float timer;

    void Update()
    {
        // Spawn interval also gets shorter as speed increases
        // making the game feel faster overall
        spawnInterval = Mathf.Max(1.2f, 2.5f - GameManager.Instance.score * 0.05f);

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnWall();
        }
    }

    void SpawnWall()
    {
        float gapY = Random.Range(-2f, 2f);
        GameObject wall = Instantiate(wallPrefab, new Vector3(10f, gapY, 0f), Quaternion.identity);

        // Pass current speed to wall
        wall.GetComponent<WallMover>().speed = GameManager.Instance.currentSpeed;

        if (Random.value < healthPackChance)
        {
            float hpY = Random.Range(-1.8f, 1.8f);
            Instantiate(healthPackPrefab, new Vector3(10f, hpY, 0f), Quaternion.identity);
        }

        if (Random.value < enemyChance)
        {
            float eY = Random.Range(-1.8f, 1.8f);
            Instantiate(enemyPrefab, new Vector3(11f, eY, 0f), Quaternion.identity);
        }
    }
}