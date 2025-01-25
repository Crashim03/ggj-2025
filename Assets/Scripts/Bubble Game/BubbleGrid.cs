using System;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleGame {
    public class BubbleGrid : MonoBehaviour {
        public Dictionary<Vector3Int, GluedBubble> gridHash = new();
        [SerializeField] private GameObject _throwBubble;
        [SerializeField] private Transform _basePosition;
        [SerializeField] private int bubblesToFall = 5;
        private Grid grid;

        public void SpawnBubble() {
            GameObject bubble = Instantiate(_throwBubble, _basePosition.position, new Quaternion(), null);
            bubble.GetComponent<ThrowingBubble>().bubbleGrid = this;
            bubble.GetComponent<ThrowingBubble>().SetRandomColor();
            bubble.GetComponent<ThrowingBubble>()._basePosition = _basePosition;
        }

        public void AddGluedBubble(GluedBubble bubble) {
            Vector3Int cellPosition = grid.WorldToCell(bubble.transform.position);
            bubble.transform.position = grid.GetCellCenterWorld(cellPosition);
            bubble.position = cellPosition;
            gridHash.Add(cellPosition, bubble);

            Vector3Int bottomRight;
            Vector3Int bottomLeft;
            Vector3Int topRight;
            Vector3Int topLeft;

            if (cellPosition.y % 2 == 0) {
                bottomRight = new(cellPosition.x, cellPosition.y - 1);
                bottomLeft = new(cellPosition.x - 1, cellPosition.y - 1);
                topRight = new(cellPosition.x, cellPosition.y + 1);
                topLeft = new(cellPosition.x - 1, cellPosition.y + 1);

            } else {
                bottomRight = new(cellPosition.x + 1, cellPosition.y - 1);
                bottomLeft = new(cellPosition.x, cellPosition.y - 1);
                topRight = new(cellPosition.x + 1, cellPosition.y + 1);
                topLeft = new(cellPosition.x, cellPosition.y + 1);
            }

            Vector3Int right = new(cellPosition.x + 1, cellPosition.y);
            Vector3Int left = new(cellPosition.x - 1, cellPosition.y);
            
            var neighborMappings = new (Vector3Int, Action<GluedBubble>)[]
            {
                (bottomRight, (b) => { bubble.adjacents.BottomRight = b; b.adjacents.TopLeft = bubble; }),
                (bottomLeft, (b) => { bubble.adjacents.BottomLeft = b; b.adjacents.TopRight = bubble; }),
                (topRight, (b) => { bubble.adjacents.TopRight = b; b.adjacents.BottomLeft = bubble; }),
                (topLeft, (b) => { bubble.adjacents.TopLeft = b; b.adjacents.BottomRight = bubble; }),
                (right, (b) => { bubble.adjacents.Right = b; b.adjacents.Left = bubble; }),
                (left, (b) => { bubble.adjacents.Left = b; b.adjacents.Right = bubble; })
            };

            foreach (var (position, action) in neighborMappings)
            {
                if (gridHash.TryGetValue(position, out var neighbor))
                {
                    action(neighbor);
                }
            }

            List<GluedBubble> sameColor = bubble.GetSameColorBubbles();
            
            if (sameColor.Count >= bubblesToFall) {
                bubble.Fall(true);
            }

            Debug.Log(sameColor.Count);
        }

        private void Start() {
            SpawnBubble();
        }

        private void Awake() {
            grid = GetComponent<Grid>();
        }
    }
}