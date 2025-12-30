using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI collectibleText;
    public GameObject endPanel;
    public TextMeshProUGUI endMessage;

    [Header("Collectibles")]
    private int totalCollectibles;
    private int collected = 0;

    void Start()
{
    // Count existing collectibles in the scene (Unity 6+ friendly)
    totalCollectibles = Object.FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;

    UpdateCollectibleText();

    // Make sure panel starts hidden
    endPanel.SetActive(false);
}


    public void Collect()
    {
        collected++;
        UpdateCollectibleText();

        if (collected >= totalCollectibles)
        {
            ShowEndScreen("You Win!");
        }
    }

    public void PlayerDied()
    {
        ShowEndScreen("You Died!");
    }

    void ShowEndScreen(string message)
    {
        endMessage.text = message;
        endPanel.SetActive(true);

        // Pause game logic
        Time.timeScale = 0f;
    }

    void UpdateCollectibleText()
    {
        collectibleText.text = $"Collectibles: {collected}/{totalCollectibles}";
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
