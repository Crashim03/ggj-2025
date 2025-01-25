using UnityEngine;
using UnityEngine.SceneManagement;

namespace BubbleGame {
    public class Finish : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.TryGetComponent<GluedBubble>(out _)) {
                Debug.Log("Lose");
                SceneManager.LoadScene("Finish Bubble Game");
            }
        }   
    }
}