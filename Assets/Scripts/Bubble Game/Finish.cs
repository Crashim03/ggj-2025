using UnityEngine;
using UnityEngine.SceneManagement;

namespace BubbleGame {
    public class Finish : MonoBehaviour {
        private bool _finish = false;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.TryGetComponent<GluedBubble>(out _) && !_finish) {
                Debug.Log("Lose");
                _finish = true;
                GameManager.Instance.GameLose();
            }
        }   
    }
}