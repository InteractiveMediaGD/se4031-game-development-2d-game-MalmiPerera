using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private bool scored = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !scored)
        {
            scored = true;
            GameManager.Instance.AddScore(1);
        }
    }
}