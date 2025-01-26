using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager: MonoBehaviour {
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScoreUI redScore;
    [SerializeField] private ScoreUI blueScore;
    [SerializeField] private ScoreUI greenScore;
    [SerializeField] private ScoreUI yellowScore;
    [SerializeField] private TMP_Text healthScore;
    private int red = 0;
    private int blue = 0;
    private int yellow = 0;
    private int green = 0;
    [SerializeField] private int maxHealthPoints = 25;
    private int healthPoints = 25;

    public void Pop(BubbleColor color) {
        switch (color) {
            case BubbleColor.Red:
                IncrementRed();
                break;
            case BubbleColor.Green:
                IncrementGreen();
                break;
            case BubbleColor.Yellow:
                IncrementYellow();
                break;
            case BubbleColor.Blue:
                IncrementBlue();
                break;
        }
    }

    private void IncrementRed() {
        red += 1;
        Debug.Log(red);
        redScore.SetText(red);
    }

    private void IncrementGreen() {
        green += 1;
        Debug.Log(green);
        greenScore.SetText(green);
    }

    private void IncrementYellow() {
        yellow += 1;
        Debug.Log(yellow);
        yellowScore.SetText(yellow);
    }

    private void IncrementBlue() {
        blue += 1;
        Debug.Log(blue);
        blueScore.SetText(blue);
    }

    public void DecreaseHealth() {
        healthPoints -= 1;
        UpdateHealth();
        CheckHealth();
    }

    private void UpdateHealth() {
        healthScore.text = healthPoints.ToString()+" / "+maxHealthPoints.ToString();
    }

    private void CheckHealth() {
        if (healthPoints <= 0) {
            GameLose();
        }
    }

    public void GameLose() {
        SceneManager.LoadScene("Finish Bubble Game");
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        healthPoints = maxHealthPoints;
        UpdateHealth();
    }
}