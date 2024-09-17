using UnityEngine;
using TMPro;  // Make sure to include this for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Reference to the TextMeshProUGUI component
    private int score = 0;  // The current score

    void Start()
    {
        // Ensure scoreText is assigned in the inspector
        if (scoreText == null)
        {
            Debug.LogError("ScoreText is not assigned in the ScoreManager.");
        }
        
        // Initialize the score display
        UpdateScoreDisplay();
    }

    // Method to add points to the score
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    // Method to update the score display
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    public void resetPoints() { score = 0; }
}
