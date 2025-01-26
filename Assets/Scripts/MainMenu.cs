using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {
    [SerializeField] private Transform _highscorePanel;
    [SerializeField] private GameObject _highscoreObject;
    [SerializeField] private int _maxScores = 5;

    public void StartGame() {
        SceneManager.LoadScene("Main");
    }

    public void Quit() {
        Application.Quit();
    }

    private void Start() {
        HighscoreList highscoreList = SaveSystem.GetHighscore();

        List<Highscore> sortedScores = highscoreList.scores.OrderByDescending(o => o.score).ToList();

        int displayedCount = 0;
        foreach (Highscore highscore in sortedScores) {
            if (displayedCount >= 3)
                break;

            GameObject highScoreObject = Instantiate(_highscoreObject, _highscorePanel);

            highScoreObject.GetComponent<TMP_Text>().text = $"{highscore.score} - {highscore.dateTime}";

            displayedCount++;
        }
    }
}