using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }
    private List<GameObject> turretPlots = new List<GameObject>();
    private bool buyingEnabled = false;
    [SerializeField]
    private Toggle toggleButton;
    private int[] turretPrices = { 10, 25, 50, 100, 200 };
    private int currentTurretPriceIndex = 0;

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
        if (buyingEnabled && CheckCurrency())
        {
            var plotsCopy = new List<GameObject>(turretPlots);

        foreach (GameObject plot in plotsCopy)
        {
            if (plot == tower)
            {
                tower.GetComponent<TowerBehaviour>().EnableTurret();
                turretPlots.Remove(tower);
            }
        }

        tower.GetComponent<TowerBehaviour>().EnableTurret();
        toggleButton.isOn = false;
        GameManager.Instance.TowerBuy(turretPrices[currentTurretPriceIndex]);
        if (currentTurretPriceIndex < turretPrices.Length)
        {
            currentTurretPriceIndex++;
        }
        ToggleTowerBuy();
        }
    }

    // public void ToggleTowerBuy()
    // {
    //     buyingEnabled = toggleButton.isOn;

    //     foreach (GameObject tower in turretPlots)
    //     {
    //         tower.GetComponent<TowerBehaviour>().ToggleBuy();
    //     }
    // }

    public void ToggleTowerBuy()
    {
        buyingEnabled = toggleButton.isOn;
        Debug.Log(turretPrices[currentTurretPriceIndex]);

        foreach (GameObject tower in turretPlots)
        {
            tower.GetComponent<TowerBehaviour>().ToggleBuy();
        }
    }

    private bool CheckCurrency()
    {
        return (GameManager.Instance.GetCurrency() >= turretPrices[currentTurretPriceIndex]);
    }
}
