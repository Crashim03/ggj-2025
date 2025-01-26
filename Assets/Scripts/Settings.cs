using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings: MonoBehaviour {
    private bool _paused = false;

    public void Pause() {
        _paused = !_paused;

        if (_paused)
             Time.timeScale = 0;
        else
             Time.timeScale = 1;
    }

    public void ReturnToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}