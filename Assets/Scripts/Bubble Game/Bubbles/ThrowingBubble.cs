using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BubbleGame {
    public class ThrowingBubble: Bubble {
        [SerializeField] private Transform _basePosition;
        [SerializeField] private float _range = 5f;
        [SerializeField] private float _maxVelocity = 5f;
        private bool _holding = false;
        private bool _thrown = false;
        

        private void OnMouseDown() {
            if (_thrown)
                return;

            _holding = true;
        }

        public void Throw()
        {
            _holding = false;
            Vector3 direction = _basePosition.position - transform.position;
            float maxDistance = _range;

            float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),_basePosition.position);

            float velocity = distance * _maxVelocity / maxDistance;

            direction.Normalize();
            direction *= velocity;

            Debug.Log(velocity);
            rigidBody.AddForce(direction, ForceMode2D.Impulse);
            _thrown = true;
        }

        public void Stick() {
            // TODO
        }

        private void Update() {
            if (!_holding || _thrown)
                return;
            
            if (Input.GetMouseButtonUp(0))
                Throw();
            
            if ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - _basePosition.position).sqrMagnitude < Mathf.Pow(_range, 2))
                rigidBody.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            else {
                Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _basePosition.position;
                rigidBody.MovePosition(_basePosition.position + direction.normalized * _range);
            }
        }
    }
}