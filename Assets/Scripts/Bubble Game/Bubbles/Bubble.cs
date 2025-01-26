using System;
using UnityEngine;

namespace BubbleGame {
    public class Bubble: MonoBehaviour {
        public BubbleColor bubbleColor;
        public BubbleGrid bubbleGrid;
        public Sprite redSprite;
        public Sprite blueSprite;
        public Sprite greenSprite;
        public Sprite yellowSprite;
        protected Rigidbody2D rigidBody;

        private void Awake() {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        public void SetColor(BubbleColor bubbleColor) {
            this.bubbleColor = bubbleColor;
            
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            switch(bubbleColor) {
                case BubbleColor.Red:
                    spriteRenderer.sprite = redSprite;
                    break;
                case BubbleColor.Blue:
                    spriteRenderer.sprite = blueSprite;
                    break;
                case BubbleColor.Yellow:
                    spriteRenderer.sprite = yellowSprite;
                    break;
                case BubbleColor.Green:
                    spriteRenderer.sprite = greenSprite;
                    break;
            }
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