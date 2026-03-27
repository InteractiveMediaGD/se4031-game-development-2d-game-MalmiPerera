using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healAmount = 1;
    public float lifetime = 7f; // Destroyed after this time if player misses it

    void Start()
    {
        // Requirement: destroyed shortly after if player goes past them
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move with the world at current speed
        transform.Translate(Vector2.left * GameManager.Instance.currentSpeed * Time.deltaTime);

        // Also destroy if it goes off screen
        if (transform.position.x < -11f)
            Destroy(gameObject);
    }

    // Requirement: destroyed upon collision with the player
    // Healing handled in PlayerController.OnTriggerEnter2D
}