using UnityEngine;

namespace BubbleGame {
    public class BubbleGrid : MonoBehaviour {
        public Grid grid;
        [SerializeField] private GameObject _throwBubble;
        [SerializeField] private Transform _basePosition;

        public void SpawnBubble() {
            GameObject bubble = Instantiate(_throwBubble, null, _basePosition);
            bubble.GetComponent<ThrowingBubble>().bubbleGrid = this;
            bubble.GetComponent<ThrowingBubble>().SetRandomColor();
            bubble.GetComponent<ThrowingBubble>()._basePosition = _basePosition;
        }

        private void Start() {
            SpawnBubble();
        }

        private void Awake() {
            grid = GetComponent<Grid>();
        }
    }
}