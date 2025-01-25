using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BubbleGame {
    public class ThrowingBubble: Bubble {
        [SerializeField] private float _range = 5f;
        [SerializeField] private float _maxVelocity = 5f;
        [SerializeField] private float _minDistanceToThrow = 11f;
        [SerializeField] private float _timeToThrow = 5f;
        public Transform basePosition;
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
            Vector3 velocity = basePosition.position - transform.position;
            float maxDistance = _range;

            float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),basePosition.position);

            float speed = distance * _maxVelocity / maxDistance;

            velocity.Normalize();
            velocity *= speed;
            rigidBody.AddForce(velocity, ForceMode2D.Impulse);
            _thrown = true;
            _velocity = velocity;
            StartCoroutine(ThrowTimer());
        }

        public void Stick() {
            rigidBody.bodyType = RigidbodyType2D.Static;

            GluedBubble gluedBubble = gameObject.AddComponent<GluedBubble>();
            gluedBubble.bubbleColor = bubbleColor;
            gluedBubble.bubbleGrid = bubbleGrid;

            bubbleGrid.AddGluedBubble(gluedBubble);
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

        private IEnumerator ThrowTimer() {
            float timer = _timeToThrow;
            while (timer > 0) {
                timer -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            _velocity = Vector3.zero;
            _thrown = false;
            transform.position = basePosition.position;
            rigidBody.linearVelocity = Vector3.zero;
        }

        private void Update() {
            _distance = Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), basePosition.position);

            if (!_holding || _thrown)
                return;
            
            if (Input.GetMouseButtonUp(0)) {
                if (_distance < _minDistanceToThrow) {
                    _holding = false;
                    transform.position = basePosition.position;
                } else 
                    Throw();
            }
            
            Vector3 position;
            if ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - basePosition.position).sqrMagnitude < Mathf.Pow(_range, 2))
                position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            else {
                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - basePosition.position;
                position = basePosition.position + direction.normalized * _range;
            }

            rigidBody.MovePosition(position);
        }

        private void Start() {
            SetRandomColor();
        }
    }
}