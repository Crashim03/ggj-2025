using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }
    private GameObject[] turretPlots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        turretPlots = GameObject.FindGameObjectsWithTag("TurretPlot");
    }
}
