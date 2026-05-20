using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int trashToWin = 999;
    public float gameDuration = 60f;

    public TMP_Text[] scoreTexts;
    public TMP_Text timerText;
    public GameObject winPanel;
    public TMP_Text winText;

    // Lighting
    public Light sunLight;           // drag your directional light here
    public float minIntensity = 0.01f;  // how dark at start
    public float maxIntensity = 1.5f;   // how bright at end
    public int totalTrashInScene = 20;  // set this to how many trash objects you have

    private int[] scores = new int[4];
    private string[] names = { "Player", "AI Red", "AI Yellow", "AI Purple" };
    private bool gameOver = false;
    private float timeRemaining;
    private int totalCollected = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeRemaining = gameDuration;
        if (winPanel != null)
            winPanel.SetActive(false);
        if (sunLight != null)
            sunLight.intensity = minIntensity;
        UpdateUI();
    }

    void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;

        if (timerText != null)
        {
            float t = Mathf.Max(timeRemaining, 0f);
            int mins = Mathf.FloorToInt(t / 60f);
            int secs = Mathf.FloorToInt(t % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", mins, secs);
        }

        if (timeRemaining <= 0f)
            EndGameByTimer();
    }

    public void AddScore(int playerIndex)
    {
        if (gameOver) return;
        scores[playerIndex]++;
        totalCollected++;
        UpdateLighting();
        UpdateUI();
        if (scores[playerIndex] >= trashToWin)
            EndGame(playerIndex);
    }

    void UpdateLighting()
    {
        if (sunLight == null) return;
        float t = Mathf.Clamp01((float)totalCollected / totalTrashInScene);
        sunLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }

    void UpdateUI()
    {
        for (int i = 0; i < scores.Length; i++)
            if (i < scoreTexts.Length && scoreTexts[i] != null)
                scoreTexts[i].text = names[i] + ": " + scores[i];
    }

    void EndGame(int winner)
    {
        gameOver = true;
        if (winPanel != null) winPanel.SetActive(true);
        if (winText != null) winText.text = names[winner] + " WINS!";
        Time.timeScale = 0f;
    }

    void EndGameByTimer()
    {
        gameOver = true;
        int highScore = -1;
        int winner = -1;
        bool tie = false;

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > highScore)
            {
                highScore = scores[i];
                winner = i;
                tie = false;
            }
            else if (scores[i] == highScore)
            {
                tie = true;
            }
        }

        if (winPanel != null) winPanel.SetActive(true);
        if (winText != null)
            winText.text = tie ? "It's a TIE!" : names[winner] + " WINS!";

        Time.timeScale = 0f;
    }

    public bool IsGameOver() => gameOver;
}