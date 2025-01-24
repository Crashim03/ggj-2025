using System.Collections.Generic;
using UnityEngine;

namespace BubbleGame {
    public class Bubble: MonoBehaviour {
        public Color color;
        protected Rigidbody2D rigidBody;

        private void Awake() {
            rigidBody = GetComponent<Rigidbody2D>();
        }
    }
    
    public enum Color {
        Red,
        Blue,
        Yellow,
        Green
    }
}
