using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleGame {
    public class GluedBubble: Bubble {
        public Adjacents adjacents;
        public Vector3Int position;
        [SerializeField] private float bubblePopTime = 0.1f;
        public bool onCeiling = false;

        public GluedBubble() {
            adjacents.TopLeft = null;
            adjacents.TopRight = null;
            adjacents.Right = null;
            adjacents.Left = null;
            adjacents.BottomLeft = null;
            adjacents.BottomRight = null;
        }

        public void Fall(bool recursive, List<GluedBubble> context) {
            StartCoroutine(Falling(recursive, context));
        }

        public List<GluedBubble> GetSameColorBubbles() {
            List<GluedBubble> bubbles = new();
            GetSameColorBubbles(bubbles);
            return bubbles;
        }

        public bool CanFall(List<GluedBubble> context) {
            context.Add(this);
            if (onCeiling)
                return false;

            List<GluedBubble> adjacents = GetAdjacentBubbles();

            foreach (GluedBubble bubble in adjacents)
            {
                if (context.Contains(bubble))
                    continue;

                if (!bubble.CanFall(context))
                    return false;
            }
            return true;
        }

        private void RemoveBubbleInAdjacents(GluedBubble bubble) {
            if (this == bubble.adjacents.TopLeft)
                bubble.adjacents.TopLeft = null;
            else if (this == bubble.adjacents.TopRight)
                bubble.adjacents.TopRight = null;
            else if (this == bubble.adjacents.BottomLeft)
                bubble.adjacents.BottomLeft = null;
            else if (this == bubble.adjacents.BottomRight)
                bubble.adjacents.BottomRight = null;
            else if (this == bubble.adjacents.Left)
                bubble.adjacents.Left = null;
            else if (this == bubble.adjacents.Right)
                bubble.adjacents.Right = null;
        }
        
        private IEnumerator Falling(bool recursive, List<GluedBubble> context) {
            bubbleGrid.SetPoppingTimer();
            context.Add(this);
            yield return new WaitForSeconds(bubblePopTime);
            GetComponent<SpriteRenderer>().enabled = false;

            List<GluedBubble> adjacentBubbles = GetAdjacentBubbles();
            foreach (GluedBubble bubble in adjacentBubbles) {
                if (context.Contains(bubble))
                    continue;

                RemoveBubbleInAdjacents(bubble);
                if (bubble.bubbleColor == bubbleColor && recursive)
                    bubble.Fall(true, context);
            }

            foreach (GluedBubble bubble in adjacentBubbles) {
                if (context.Contains(bubble))
                    continue;

                if (bubble.CanFall(new List<GluedBubble>()))
                    bubble.Fall(false, context);
            }

            GameManager.Instance.Pop(bubbleColor);
            bubbleGrid.bubblesToDestroy.Add(gameObject);
        }

        private void GetSameColorBubbles(List<GluedBubble> bubbles) {
            List<GluedBubble> adjacents = GetAdjacentBubbles();
            bubbles.Add(this);

            foreach (GluedBubble bubble in adjacents) {
                if (bubbles.Contains(bubble) || bubble.bubbleColor != bubbleColor)
                    continue;

                bubble.GetSameColorBubbles(bubbles);
            }
        }

        public override bool Equals(object obj) {
            if (obj is GluedBubble otherBubble)
            {
                return position == otherBubble.position;
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        private List<GluedBubble> GetAdjacentBubbles() {
            List<GluedBubble> bubbles = new();

            if (adjacents.TopLeft != null)
                bubbles.Add(adjacents.TopLeft);

            if (adjacents.TopRight != null)
                bubbles.Add(adjacents.TopRight);

            if (adjacents.Right != null)
                bubbles.Add(adjacents.Right);

            if (adjacents.Left != null)
                bubbles.Add(adjacents.Left);

            if (adjacents.BottomLeft != null)
                bubbles.Add(adjacents.BottomLeft);

            if (adjacents.BottomRight != null)
                bubbles.Add(adjacents.BottomRight);

            return bubbles;
        }

    }
    public struct Adjacents {
        public GluedBubble TopLeft;
        public GluedBubble TopRight;
        public GluedBubble Right;
        public GluedBubble Left;
        public GluedBubble BottomRight;
        public GluedBubble BottomLeft;

    }
}