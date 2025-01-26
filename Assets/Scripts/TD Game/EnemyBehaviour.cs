using NavMeshPlus.Extensions;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform endPoint;
    private NavMeshAgent agent;
    private int health = 1;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        agent.SetDestination(endPoint.position);
    }

    public void SetEndPoint(Transform point)
    {
        endPoint = point;
    }

    public void SetEnemyHealth(int hp)
    {
        this.health = hp;
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.EnemyKilled();
        }
    }
}
