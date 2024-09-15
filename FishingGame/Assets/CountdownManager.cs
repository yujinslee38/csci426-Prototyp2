using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading;

public class CountdownManager : MonoBehaviour
{
    public Text countdownText; //reference to the UI text component\
    public Button startButton;
    public int countdownTime = 3;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonPressed);
        countdownText.gameObject.SetActive(false);
    }
    private void OnStartButtonPressed()
    {
        // Hide the button and start the countdown
        startButton.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }
    private IEnumerator StartCountdown()
    {
        for(int i = countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();

            yield return new WaitForSeconds(1);
        }
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        StartGameplay();
    }

    private void StartGameplay()
    {
        Debug.Log("StartGameplay called");
        //where we enable game logic to set behaviors to active
        SceneManager.LoadScene("FishingGameScene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
