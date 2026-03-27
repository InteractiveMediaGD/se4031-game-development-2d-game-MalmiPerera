using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float flapForce = 6f;
    public GameObject projectilePrefab;

    private Rigidbody2D rb;
    private bool isDead = false;
    private bool isInvincible = false; // ← prevents multiple hits at once
    public float invincibleDuration = 1.5f; // seconds of invincibility after hit

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            rb.velocity = Vector2.up * flapForce;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        float tilt = Mathf.Clamp(rb.velocity.y * 4f, -60f, 60f);
        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    public void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        GameManager.Instance.TriggerGameOver();
    }

    public void ResetPlayer()
    {
        isDead = false;
        isInvincible = false;
        rb.gravityScale = 2f;
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(-4, 0, 0);
        transform.rotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall") && !isInvincible)
        {
            StartCoroutine(TakeHit(col.collider));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthPack"))
        {
            GameManager.Instance.HealPlayer(1);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy") && !isInvincible)
        {
            StartCoroutine(TakeHit(null));
            Destroy(other.gameObject);
        }
    }

    // Handles damage + invincibility + pass through
    IEnumerator TakeHit(Collider2D wallCollider)
    {
        isInvincible = true;

        // Deal 1 damage
        GameManager.Instance.TakeDamage(1);

        // Pass through wall if it was a wall hit
        if (wallCollider != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), wallCollider, true);
        }

        // Flash player to show invincibility
        StartCoroutine(FlashPlayer());

        // Wait before player can take damage again
        yield return new WaitForSeconds(invincibleDuration);

        // Re-enable wall collision
        if (wallCollider != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), wallCollider, false);
        }

        isInvincible = false;
    }

    // Visual feedback — player flashes when hit
    IEnumerator FlashPlayer()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 5; i++)
        {
            sr.color = new Color(1f, 1f, 1f, 0.2f); // nearly invisible
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1f, 1f, 1f, 1f);   // fully visible
            yield return new WaitForSeconds(0.1f);
        }
        sr.color = Color.white;
    }
}