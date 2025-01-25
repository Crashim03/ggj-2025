using System;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleGame {
    public class GluedBubble: Bubble {
        public Adjacents adjacents;
        public Vector3Int position;

        public GluedBubble() {
            adjacents.TopLeft = null;
            adjacents.TopRight = null;
            adjacents.Right = null;
            adjacents.Left = null;
            adjacents.BottomLeft = null;
            adjacents.BottomRight = null;
        }

        public void Fall() {
            // TODO
        }

        public List<GluedBubble> GetSameColorBubbles() {
            List<GluedBubble> bubbles = new();
            GetSameColorBubbles(bubbles);
            return bubbles;
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