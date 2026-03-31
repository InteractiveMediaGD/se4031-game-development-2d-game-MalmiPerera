using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Rock Prefabs")]
    public GameObject[] rockPrefabs; // assign all 5 rocks here

    [Header("Spawn Settings")]
    public float minInterval = 1.5f;  // min time between spawns
    public float maxInterval = 3.0f;  // max time between spawns
    public int maxRocksAtOnce = 3;    // never more than 3 on screen

    private float timer;
    private float nextSpawn;

    void Start()
    {
        // First spawn after random delay
        nextSpawn = Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawn)
        {
            timer = 0f;
            nextSpawn = Random.Range(minInterval, maxInterval);
            TrySpawnRocks();
        }
    }

    void TrySpawnRocks()
    {
        // Count rocks already on screen
        GameObject[] existing = GameObject.FindGameObjectsWithTag("Wall");
        if (existing.Length > maxRocksAtOnce + 6) return; // too many, skip

        // Spawn 1 or 2 rocks per wave
        int count = Random.Range(1, 3);

        // Store used Y positions to space rocks apart
        float[] usedY = new float[count];

        for (int i = 0; i < count; i++)
        {
            float y = GetSafeY(usedY, i);
            usedY[i] = y;

            // Pick random rock type
            int rockIndex = Random.Range(0, rockPrefabs.Length);

            // Slight random X offset so rocks dont all line up
            float xOffset = Random.Range(0f, 1.5f);

            // Random Z rotation so each rock looks different
            float zRot = Random.Range(0f, 360f);
            Quaternion rot = Quaternion.Euler(0, 0, zRot);

            Vector3 spawnPos = new Vector3(11f + xOffset, y, 0f);
            Instantiate(rockPrefabs[rockIndex], spawnPos, rot);
        }
    }

    float GetSafeY(float[] usedY, int currentIndex)
    {
        float y;
        bool tooClose;
        int tries = 0;

        do
        {
            // Random Y — stays within playable area
            // -3.5 to 3.5 leaves room for player to pass
            y = Random.Range(-3.5f, 3.5f);
            tooClose = false;

            // Check distance from other rocks this wave
            for (int j = 0; j < currentIndex; j++)
            {
                if (Mathf.Abs(y - usedY[j]) < 2.5f)
                {
                    tooClose = true;
                    break;
                }
            }

            tries++;
        }
        while (tooClose && tries < 15);

        return y;
    }
}
