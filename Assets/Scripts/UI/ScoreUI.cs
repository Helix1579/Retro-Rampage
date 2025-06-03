using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.playerScore}";
        }
    }
}
