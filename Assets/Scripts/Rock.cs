using UnityEngine;
using static UnityEngine.UIElements.VisualElement;

public class Rock : MonoBehaviour
{
    public float lifetime = 8f;
    public float rotationSpeed = 0f;

    void Start()
    {
        // Each rock gets random rotation speed
        // makes them look natural floating in space
        rotationSpeed = Random.Range(-20f, 20f);

        // Auto destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move left with current game speed
        transform.Translate(
            Vector2.left * GameManager.Instance.currentSpeed * Time.deltaTime,
            Space.World
        );

        // Slowly rotate — asteroid tumbling effect
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Destroy when off screen
        if (transform.position.x < -12f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Player hits rock — take damage
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(1);
        }

        // Projectile hits rock — destroy projectile only
        // rocks are solid, don't get destroyed by shooting
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
        }
    }
}
