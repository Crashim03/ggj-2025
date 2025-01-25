using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BubbleGame {
    public class ThrowingBubble: Bubble {
        [SerializeField] private float _range = 5f;
        [SerializeField] private float _maxVelocity = 5f;
        [SerializeField] private float _minDistanceToThrow = 11f;
        [SerializeField] private float _angleToThrow = 160f;
        public Transform _basePosition;
        private bool _holding = false;
        private bool _thrown = false;
        private Vector2 _velocity;
        private float _distance;
        

        private void OnMouseDown() {
            if (_thrown)
                return;

            _holding = true;
        }

        public void Throw()
        {
            _holding = false;
            Vector3 velocity = _basePosition.position - transform.position;
            float maxDistance = _range;

            float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),_basePosition.position);

            float speed = distance * _maxVelocity / maxDistance;

            velocity.Normalize();
            velocity *= speed;
            rigidBody.AddForce(velocity, ForceMode2D.Impulse);
            _thrown = true;
            _velocity = velocity;
        }

        public void Stick() {
            rigidBody.bodyType = RigidbodyType2D.Static;

            GluedBubble gluedBubble = gameObject.AddComponent<GluedBubble>();
            gluedBubble.bubbleColor = bubbleColor;
            gluedBubble.bubbleGrid = bubbleGrid;

            bubbleGrid.AddGluedBubble(gluedBubble);
            bubbleGrid.SpawnBubble();
            
            Destroy(this);
        }

        private void OnCollisionEnter2D(Collision2D other) {

            if (other.gameObject.TryGetComponent<GluedBubble>(out _) || other.gameObject.CompareTag("Top")) {
                Stick();
                return;
            }
            _velocity = new Vector2(-_velocity.x, _velocity.y);
            rigidBody.linearVelocity = _velocity;
        }

        private void Update() {
            _distance = Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), _basePosition.position);

            if (!_holding || _thrown)
                return;
            
            if (Input.GetMouseButtonUp(0)) {
                if (_distance < _minDistanceToThrow) {
                    _holding = false;
                    transform.position = _basePosition.position;
                } else 
                    Throw();
            }
            
            Vector3 position;
            if ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - _basePosition.position).sqrMagnitude < Mathf.Pow(_range, 2))
                position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            else {
                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _basePosition.position;
                position = _basePosition.position + direction.normalized * _range;
            }

            float angle = Vector3.SignedAngle(Vector2.down, position - _basePosition.position, Vector3.right);

            if (angle > _angleToThrow / 2) {
                Debug.Log(angle - _angleToThrow / 2);
            }
            rigidBody.MovePosition(position);
        }

        private void Start() {
            SetRandomColor();
        }
    }
}