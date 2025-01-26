using UnityEngine;
using System.Collections.Generic;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }
    private List<GameObject> turretPlots = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        turretPlots.AddRange(GameObject.FindGameObjectsWithTag("TurretPlot"));
    }


    public void TowerCheck(GameObject tower)
    {
        var plotsCopy = new List<GameObject>(turretPlots);

        foreach (GameObject plot in plotsCopy)
        {
            if (plot == tower)
            {
                tower.GetComponent<TowerBehaviour>().EnableTurret();
                turretPlots.Remove(plot);
            }
        }

        tower.GetComponent<TowerBehaviour>().EnableTurret();
    }
}
