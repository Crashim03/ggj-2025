using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleGame {
    public class BubbleGrid : MonoBehaviour {
        public Dictionary<Vector3Int, GluedBubble> gridHash = new();
        public int popping = 0;
        public List<GameObject> bubblesToDestroy = new();
        [SerializeField] private GameObject _throwBubble;
        [SerializeField] private GameObject _glueBubble;
        [SerializeField] private int _ceilingRow = 0;
        [SerializeField] private Transform _basePosition;
        [SerializeField] private int bubblesToFall = 5;
        [SerializeField] private int _minCollumn = -5;
        [SerializeField] private int _maxCollumn = 6;

        private Grid grid;

        public IEnumerator SpawnBubble() {
            while (popping > 0) {
                Debug.Log("waiting");
                yield return new WaitForFixedUpdate();
            }
                
            Debug.Log("Got out");
            
            foreach (GameObject b in bubblesToDestroy) {
                Destroy(b);
            }

            ThrowingBubble bubble = Instantiate(_throwBubble, _basePosition.position, new Quaternion(), transform).GetComponent<ThrowingBubble>();
            bubble.bubbleGrid = this;
            bubble.SetRandomColor();
            bubble.basePosition = _basePosition;
        }

        public void AddLine() {
            Dictionary<Vector3Int, GluedBubble> newHash = new();

            // Move one row down
            foreach (Vector3Int position in gridHash.Keys) {
                GluedBubble bubble = gridHash[position];
                bubble.position.y -= 1;
                bubble.onCeiling = false;
                newHash.Add(bubble.position, bubble);
                bubble.transform.position = grid.GetCellCenterWorld(bubble.position);
            }

            gridHash = newHash;

             // Add new bubbles
            for (int i = _minCollumn; i <= _maxCollumn; i++) {
                Vector3Int position = new(i, _ceilingRow, 0);
                GluedBubble bubble = Instantiate(_glueBubble, grid.GetCellCenterWorld(position), new Quaternion(), transform).GetComponent<GluedBubble>();
                bubble.bubbleGrid = this;
                bubble.SetRandomColor();
                bubble.position = position;
                bubble.onCeiling = true;
                gridHash.Add(position, bubble);
            }

            foreach (GluedBubble bubble in gridHash.Values) {
                AddAdjacents(bubble);
            }
        }

        public void AddGluedBubble(GluedBubble bubble) {
            Vector3Int cellPosition = grid.WorldToCell(bubble.transform.position);
            bubble.transform.position = grid.GetCellCenterWorld(cellPosition);
            bubble.position = cellPosition;
            gridHash.Add(cellPosition, bubble);

            Debug.Log(cellPosition);
            if (cellPosition.y == _ceilingRow) {
                bubble.onCeiling = true;
            }

            AddAdjacents(bubble);

            List<GluedBubble> sameColor = bubble.GetSameColorBubbles();
            

            if (sameColor.Count >= bubblesToFall) {
                bubble.Fall(true);
            }
            StartCoroutine(SpawnBubble());

            Debug.Log(sameColor.Count);
        }

        private void AddAdjacents(GluedBubble bubble) {
            Vector3Int bottomRight;
            Vector3Int bottomLeft;
            Vector3Int topRight;
            Vector3Int topLeft;

            if (bubble.position.y % 2 == 0) {
                bottomRight = new(bubble.position.x, bubble.position.y - 1);
                bottomLeft = new(bubble.position.x - 1, bubble.position.y - 1);
                topRight = new(bubble.position.x, bubble.position.y + 1);
                topLeft = new(bubble.position.x - 1, bubble.position.y + 1);

            } else {
                bottomRight = new(bubble.position.x + 1, bubble.position.y - 1);
                bottomLeft = new(bubble.position.x, bubble.position.y - 1);
                topRight = new(bubble.position.x + 1, bubble.position.y + 1);
                topLeft = new(bubble.position.x, bubble.position.y + 1);
            }

            Vector3Int right = new(bubble.position.x + 1, bubble.position.y);
            Vector3Int left = new(bubble.position.x - 1, bubble.position.y);
            
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
        }

        private void Start() {
            StartCoroutine(SpawnBubble());
            AddLine();
            AddLine();
            AddLine();
            AddLine();
        }

        private void Awake() {
            grid = GetComponent<Grid>();
        }
    }
}