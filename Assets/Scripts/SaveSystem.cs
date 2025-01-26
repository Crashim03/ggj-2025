using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem {
    private const string _path = "/highscores.json";

    public static void AddScore(float score) {
        string path = Application.persistentDataPath + _path;
        HighscoreList highscores = new();

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            highscores = JsonUtility.FromJson<HighscoreList>(json);
        } else {
            Debug.LogWarning("File not found. Creating a new one.");
            highscores.scores = new List<Highscore>();
        }

        highscores.scores.Add(new Highscore { score = (int)score, dateTime = DateTime.Now.ToString() });

        string updatedJson = JsonUtility.ToJson(highscores, true);
        File.WriteAllText(path, updatedJson);

        Debug.Log("New highscore added and saved.");
    }

    public static HighscoreList GetHighscore() {
        string path = Application.persistentDataPath + _path;
        if (!File.Exists(path))
            return new HighscoreList();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<HighscoreList>(json);
    }
}

[Serializable]
public struct Highscore {
    public int score;
    public string dateTime;
}

[Serializable]
public class HighscoreList {
    public List<Highscore> scores = new();
}
