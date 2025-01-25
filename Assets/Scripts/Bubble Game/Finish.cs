using UnityEngine;
using UnityEngine.SceneManagement;

namespace BubbleGame {
    public class Finish : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("Lose");
            if (other.gameObject.TryGetComponent<GluedBubble>(out _))
                SceneManager.LoadScene("Finish Bubble Game");
        }   
    }
}