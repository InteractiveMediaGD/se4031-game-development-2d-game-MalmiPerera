using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float lifetime = 7f;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.left * GameManager.Instance.currentSpeed * Time.deltaTime);
        if (transform.position.x < -11f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            GameManager.Instance.AddScore(3);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }
}