using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading;

public class CountdownManager : MonoBehaviour
{
    public float countdownTime = 60f;  // Set the initial countdown time (e.g., 60 seconds)
    public Text countdownText;         // Reference to the UI Text (or TextMeshPro)

    private float currentTime;

    void Start()
    {
        currentTime = countdownTime;   // Initialize the current time to the countdown time
        UpdateCountdownDisplay();      // Update the UI at the start
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;  // Decrease the timer
            UpdateCountdownDisplay();
        }
        else
        {
            currentTime = 0;
            TimerEnded();                   // Trigger any end of timer event
        }
    }

    void UpdateCountdownDisplay()
    {
        // Format the timer as minutes and seconds, for example: "01:30"
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        // Action when the timer ends, such as showing a message or triggering an event.
        countdownText.text = "00:00";
        Time.timeScale = 0;
    }
}
