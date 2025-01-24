using System;
using System.Collections.Generic;

namespace BubbleGame {
    public class GluedBubble: Bubble {
        private Adjacents adjacents;

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
            List<GluedBubble> adjacents = GetAdjacentBubbles();

            foreach (GluedBubble bubble in adjacents) {
                if (bubble.color == color)
                    bubbles.Add(bubble);

                bubbles.AddRange(bubble.GetSameColorBubbles());
            }
            return bubbles;
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

        private struct Adjacents {
            public GluedBubble TopLeft;
            public GluedBubble TopRight;
            public GluedBubble Right;
            public GluedBubble Left;
            public GluedBubble BottomRight;
            public GluedBubble BottomLeft;

        }
    }
}