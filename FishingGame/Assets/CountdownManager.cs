using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading;

public class CountdownManager : MonoBehaviour
{
    public Text countdownText; //reference to the UI text component
    public int countdownTime = 3;

    private void Start()
    {
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

        StartGameplay();
    }

    private void StartGameplay()
    {
        //where we enable game logic to set behaviors to active
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
