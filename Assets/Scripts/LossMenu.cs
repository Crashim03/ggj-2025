using UnityEngine;
using UnityEngine.SceneManagement;

public class LossMenu: MonoBehaviour {
    public void ReturnToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void Retry() {
        SceneManager.LoadScene("Main");
    }
}