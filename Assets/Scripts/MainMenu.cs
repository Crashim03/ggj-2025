using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {
   public void StartGame() {
        SceneManager.LoadScene("Main");
   }

   public void Highscores() {

   }

    public void Quit() {
        Quit();
    }
}