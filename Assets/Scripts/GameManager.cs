using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Health — Requirement 1")]
    public int maxHealth = 3;
    public int currentHealth;
    public Slider healthBar;         // Health displayed as bar on UI
    public Image healthBarFill;      // The fill image for gradient color

    [Header("Score — Requirement 2")]
    public int score = 0;
    public TMP_Text scoreText;          // Score displayed as number on UI

    [Header("Speed — Requirement 6")]
    public float baseSpeed = 3f;
    public float currentSpeed;
    public float speedIncreaseRate = 0.01f; // Noticeable but not too fast
    public float maxSpeed = 8f;             // Speed cap

    [Header("UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private PlayerController player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        // Requirement 1: start at max health
        currentHealth = maxHealth;
        // Requirement 6: start at base speed
        currentSpeed = baseSpeed;
        UpdateHealthUI();
        UpdateScoreUI();
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // Requirement 6: speed increases over time, capped at maxSpeed
        currentSpeed = Mathf.Min(baseSpeed + score * speedIncreaseRate + Time.timeSinceLevelLoad * 0.03f, maxSpeed);
    }

    // Requirement 1: health decreases on obstacle collision
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Creative Feature: screen shake on hit
        ScreenShake.Instance?.Shake();

        // Requirement 1: stop player ONLY when health is 0
        if (currentHealth <= 0)
            player.Die();
    }

    // Requirement 3: increase health up to max value
    public void HealPlayer(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    // Requirement 2: score increments on passing through gap OR killing enemy
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            // Requirement 1: health displayed as bar on UI
            healthBar.value = (float)currentHealth / maxHealth;

            // Creative Feature (Req 7): gradient color — constantly updates with health
            // Green (full) → Yellow (half) → Red (empty), smooth continuous update
            if (healthBarFill != null)
            {
                float t = (float)currentHealth / maxHealth;
                Color healthColor;
                if (t > 0.5f)
                    healthColor = Color.Lerp(Color.yellow, Color.green, (t - 0.5f) * 2f);
                else
                    healthColor = Color.Lerp(Color.red, Color.yellow, t * 2f);
                healthBarFill.color = healthColor;
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
        if (finalScoreText) finalScoreText.text = "Final Score: " + score;
        Time.timeScale = 0f;
    }

    // Requirement 1 & 2: reset health to max, reset score to 0 on restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}