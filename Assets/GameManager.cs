using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI collectibleText;
    public TextMeshProUGUI timerText;
    public GameObject endPanel;
    public TextMeshProUGUI endMessage;

    [Header("Collectibles")]
    private int totalCollectibles;
    private int collected = 0;

    [Header("Timer")]
    public float startTime = 30f;
    private float timeRemaining;
    private bool ended = false;

    [Header("Audio")]
    public AudioClip coinSound;
    private AudioSource audioSource;

    void Start()
    {
        // Count collectibles using Unity 6+ API
        totalCollectibles = Object.FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;

        timeRemaining = startTime;

        UpdateCollectibleText();
        UpdateTimerText();

        if (endPanel != null)
            endPanel.SetActive(false);

        // reset pause state (important when restarting)
        Time.timeScale = 1f;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (ended)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerText();
            ShowEndScreen("FAILED!");
            return;
        }

        UpdateTimerText();
    }

    public void Collect()
    {
        if (ended)
            return;

        // play sound
        if (audioSource != null && coinSound != null)
            audioSource.PlayOneShot(coinSound);

        collected++;
        UpdateCollectibleText();

        if (collected >= totalCollectibles)
        {
            ShowEndScreen("You Win!");
        }
    }

    public void PlayerDied()
    {
        if (ended)
            return;

        ShowEndScreen("You Died!");
    }

    void ShowEndScreen(string message)
    {
        ended = true;

        if (endMessage != null)
            endMessage.text = message;

        if (endPanel != null)
            endPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void UpdateCollectibleText()
    {
        if (collectibleText != null)
            collectibleText.text = $"Collectibles: {collected}/{totalCollectibles}";
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = timeRemaining.ToString("F1");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
