using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore these tags completely
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("Projectile")) return;
        if (other.CompareTag("ScoreTrigger")) return;
        if (other.CompareTag("HealthPack")) return;

        // Just destroy projectile — enemy handles its own death
        Destroy(gameObject);
    }
}