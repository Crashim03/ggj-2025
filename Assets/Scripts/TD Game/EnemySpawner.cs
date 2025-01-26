using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition;
    [SerializeField]
    private Transform endPosition;
    [SerializeField]
    private GameObject enemyObject;
    [SerializeField]
    private float spawnerCooldown = 0.5f;
    [SerializeField]
    private float waveCooldown = 5f;
    [SerializeField]
    private int spawnCount = 4;
    [SerializeField]
    private int enemyStartingHealth = 1;
    private int enemyHealth;

    private void Start()
    {
        StartCoroutine(WaveSpawner());
        enemyHealth = enemyStartingHealth;
    }


    private IEnumerator WaveSpawner()
    {
        WaitForSeconds enemyCooldown = new WaitForSeconds(spawnerCooldown);
        WaitForSeconds timeToWait = new WaitForSeconds(waveCooldown);
        while (true)
        {
            yield return timeToWait;
            for (int i = 0; i < spawnCount; i++)
            {
                yield return enemyCooldown;
                GameObject enemy = Instantiate(enemyObject, spawnPosition);
                var enemyScript = enemy.GetComponent<EnemyBehaviour>();
                enemyScript.SetEndPoint(endPosition);
                enemyScript.SetEnemyHealth(enemyHealth);
            }
            enemyHealth++;
        }
    }
}
