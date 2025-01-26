using UnityEditor;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    private Transform currentTarget;
    [SerializeField]
    private LayerMask enemyLayer;

    private bool isTowerBought = false;
    [SerializeField]
    private GameObject turretBase;
    [SerializeField]
    private Transform shootingPoint;
    [SerializeField]
    private float rotationSpeed = 200f;
    private float turrretRange = 3f;
    private float fireRate = 1f;
    private float timeUntilFire;
    
    private void FixedUpdate()
    {
        if (isTowerBought)
        {
            if (currentTarget == null)
            {
                FindTarget();
                return;
            }

            RotateToTarget();

            if (!IsTargetInRange())
            {
                currentTarget = null;
            }
            else
            {
                timeUntilFire += Time.deltaTime;
                if (timeUntilFire >= 1f / fireRate)
                {
                    Shoot();
                    timeUntilFire = 0f;
                }
            }
        }
    }

    public void EnableTurret()
    {
        turretBase.GetComponent<SpriteRenderer>().enabled = true;
        isTowerBought = true;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Collided with alguma merda");
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         Debug.Log("Found target");
    //         currentTarget = other.transform;
    //     }
    // }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, turrretRange, (Vector2)transform.position, 0f, enemyLayer);
    
        if (hits.Length > 0)
        {
            currentTarget = hits[0].transform;
        }
    }

    private bool IsTargetInRange()
    {
        return Vector2.Distance(transform.position, currentTarget.position) <= turrretRange;
    }

    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y, currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretBase.transform.rotation = Quaternion.RotateTowards(turretBase.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        //HEALTH SCORE BADJUBS?
        Destroy(currentTarget.gameObject);
        GameManager.Instance.EnemyKilled();
    }

    void OnMouseDown()
    {
        if (!isTowerBought)
        TurretManager.Instance.TowerCheck(gameObject);
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.forward, turrretRange);
        #endif
    }
}
