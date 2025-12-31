using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public string gameSceneName = "Game";   // put your gameplay scene name here

    public void PlayGame()
    {
        Time.timeScale = 1f; // ensure game unpaused
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
