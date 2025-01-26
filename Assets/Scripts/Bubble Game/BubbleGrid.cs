using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BubbleGame {
    public class BubbleGrid : MonoBehaviour {
        public Dictionary<Vector3Int, GluedBubble> gridHash = new();
        public List<GameObject> bubblesToDestroy = new();
        [SerializeField] private float _cellSize = 0.8f;
        [SerializeField] private int _initialLines = 4;
        [SerializeField] private float _addLineTime = 10f;
        [SerializeField] private float _maxPoppingTime = 0.8f;
        [SerializeField] private float _poppingTime = 0f;
        [SerializeField] private Bubble _nextBubble;
        [SerializeField] private GameObject _throwBubble;
        [SerializeField] private GameObject _glueBubble;
        [SerializeField] private int _ceilingRow = 0;
        [SerializeField] private Transform _basePosition;
        [SerializeField] private int _bubblesToFall = 5;
        [SerializeField] private int _minCollumn = -5;
        [SerializeField] private int _maxCollumn = 6;
        private Grid _grid;

        public void SetPoppingTimer() {
            _poppingTime = _maxPoppingTime;
        }

        public IEnumerator SpawnBubble() {
            while (_poppingTime > 0) {
                _poppingTime -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
                
            
            foreach (GameObject b in bubblesToDestroy) {
                gridHash.Remove(b.GetComponent<GluedBubble>().position);
                Destroy(b);
            }

            bubblesToDestroy.Clear();

            ThrowingBubble bubble = Instantiate(_throwBubble, _basePosition.position, new Quaternion(), _basePosition).GetComponent<ThrowingBubble>();
            Vector3 worldCellSize = Vector3.Scale(_grid.cellSize, _grid.transform.lossyScale);

            Vector3 bubbleOriginalSize = bubble.GetComponent<Renderer>().bounds.size;
            Vector3 scaleFactor = new(
                worldCellSize.x / bubbleOriginalSize.x,
                worldCellSize.y / bubbleOriginalSize.y,
                worldCellSize.z / bubbleOriginalSize.z
            );

            bubble.transform.localScale = Vector3.Scale(bubble.transform.localScale, scaleFactor);

            bubble.bubbleGrid = this;
            bubble.SetColor(_nextBubble.bubbleColor);
            _nextBubble.SetRandomColor();
            bubble.basePosition = _basePosition;
        }

        public void AddLine() {
            _grid.transform.position = new Vector3(_grid.transform.position.x, _grid.transform.position.y - _cellSize, _grid.transform.position.z);
            
            Vector3 worldCellSize = Vector3.Scale(_grid.cellSize, _grid.transform.lossyScale);

            _ceilingRow += 1;

            foreach (GluedBubble bubble in gridHash.Values)
                bubble.onCeiling = false;

            int maxCollumn = _maxCollumn;

            if (_ceilingRow % 2 != 0)
                maxCollumn -= 1;

             // Add new bubbles
            for (int i = _minCollumn; i <= maxCollumn; i++) {
                Vector3Int position = new(i, _ceilingRow, 0);
                GluedBubble bubble = Instantiate(_glueBubble, _grid.GetCellCenterWorld(position), new Quaternion(), transform).GetComponent<GluedBubble>();

                Vector3 bubbleOriginalSize = bubble.GetComponent<Renderer>().bounds.size;
                Vector3 scaleFactor = new(
                    worldCellSize.x / bubbleOriginalSize.x,
                    worldCellSize.y / bubbleOriginalSize.y,
                    worldCellSize.z / bubbleOriginalSize.z
                );

                bubble.transform.localScale = Vector3.Scale(bubble.transform.localScale, scaleFactor);

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
            bubble.transform.parent = transform;
            Vector3Int cellPosition = _grid.WorldToCell(bubble.transform.position);
            bubble.transform.position = _grid.GetCellCenterWorld(cellPosition);

            bubble.position = cellPosition;
            Debug.Log(cellPosition);
            gridHash.TryAdd(cellPosition, bubble);

            if (cellPosition.y == _ceilingRow) {
                bubble.onCeiling = true;
            }

            AddAdjacents(bubble);

            List<GluedBubble> sameColor = bubble.GetSameColorBubbles();
            

            if (sameColor.Count >= _bubblesToFall) {
                bubble.Fall(true, new List<GluedBubble>());
            }
            StartCoroutine(SpawnBubble());
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

        private IEnumerator AddLineTimer() {
            float timer = _addLineTime;
            while (true) {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0) {
                    while (_poppingTime > 0)
                        yield return new WaitForFixedUpdate();
                        
                    AddLine();
                    timer = _addLineTime;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private void Start() {
            StartCoroutine(SpawnBubble());
            
            for (int i = 0; i < _initialLines; i++) {
                AddLine();
            }
            
            _nextBubble.SetRandomColor();
            StartCoroutine(AddLineTimer());
        }

        private void Awake() {
            _grid = GetComponent<Grid>();
        }
    }
}