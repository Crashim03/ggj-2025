using System;
using UnityEngine;

namespace BubbleGame {
    public class Bubble: MonoBehaviour {
        public BubbleColor bubbleColor;
        public BubbleGrid bubbleGrid;
        protected Rigidbody2D rigidBody;

        private void Awake() {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        public void SetColor(BubbleColor bubbleColor) {
            this.bubbleColor = bubbleColor;
            Color color = Color.red;
            switch(bubbleColor) {
                case BubbleColor.Red:
                    color = Color.red;
                    break;
                case BubbleColor.Blue:
                    color = Color.blue;
                    break;
                case BubbleColor.Yellow:
                    color = Color.yellow;
                    break;
                case BubbleColor.Green:
                    color = Color.green;
                    break;
            }

            GetComponent<SpriteRenderer>().color = color;
        }

        public void SetRandomColor() {
            Array values = Enum.GetValues(typeof(BubbleColor));
            System.Random random = new();
            BubbleColor randomColor = (BubbleColor)values.GetValue(random.Next(values.Length));

            SetColor(randomColor);
        }
    }
    
}

public enum BubbleColor {
    Red,
    Blue,
    Yellow,
    Green
}