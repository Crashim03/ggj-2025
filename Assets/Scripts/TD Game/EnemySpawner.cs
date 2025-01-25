using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition;
    [SerializeField]
    private GameObject enemyObject;
    [SerializeField]
    private float spawnerCooldown = 0.5f;
    [SerializeField]
    private float waveCooldown;
    [SerializeField]
    private int spawnCount;

    private void Start()
    {
        StartCoroutine(WaveSpawner());
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
            }
        }
    }
}
