using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }

    public int amountOfWorkers = 1;
    public int PlotLength = 1;
    public int PlotWidth = 1;
    public int totalFood = 1;
    public int totalGold = 0;
    public int maxWater = 1;
    public int maxSeeds = 1;
    public int maxFood = 1;
    public int upgradeCostWater = 10;
    public int upgradeCostFood = 10;
    public int upgradeCostSeeds = 10;
    public int farmerCost = 50;
    public int workerCost = 50;
    public int plotCost = 30;
    public Dictionary<string, bool> worldState = new Dictionary<string, bool>();
    public GameObject farmerPrefab;


    public GameObject farmPlotPrefab;
    public Transform farmerParent;
    public Transform plotParent;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI farmerCostText;
    public Transform plotIncreaseYUI;
    public TextMeshProUGUI plotIncreaseYCostText;
    public Transform plotIncreaseXUI;
    public TextMeshProUGUI plotIncreaseXCostText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Initialize world state
        worldState["HasSeeds"] = true;
        worldState["PlotEmpty"] = true;
        worldState["NeedsWater"] = false;
        worldState["ReadyToHarvest"] = false;
    }

    private void Start()
    {
        SpawnPlots();
        SpawnWorkers();
        UpdateUI();
    }

    private void SpawnWorkers()
    {
        for (int i = 0; i < amountOfWorkers; i++)
        {
            SpawnFarmer();
        }
    }

    private void SpawnPlots()
    {
        for (int i = 0; i < PlotLength; i++)
        {
            for (int j = 0; j < PlotWidth; j++)
            {
                Instantiate(farmPlotPrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity, plotParent);
            }
        }
        PlacePlotIncreaseUI();
    }

    public void BuyFarmer()
    {
        if (totalGold >= farmerCost)
        {
            totalGold -= farmerCost;
            SpawnFarmer();
            UpdateUI();
        }

    }

    private void SpawnFarmer()
    {
        var farmer = Instantiate(farmerPrefab, new Vector3(Random.Range(-5,5), 0, Random.Range(-5, 5)), Quaternion.identity,farmerParent);
        farmer.GetComponent<FarmerGOAP>().Initialize();
    }
    public void SellFood()
    {
        if (totalFood > 0)
        {
            totalFood--;
            totalGold += 10;
        }
        UpdateUI();
    }

    public void AddFood(int food)
    {
        totalFood += food;
        UpdateUI();
    }
    public bool UpgradeWater()
    {
        if (totalGold >= upgradeCostWater && maxWater<5)
        {
            totalGold -= upgradeCostWater;
            maxWater++;
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool UpgradeSeeds()
    {
        if (totalGold >= upgradeCostSeeds && maxSeeds < 5)
        {
            totalGold -= upgradeCostSeeds;
            maxSeeds++;
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool UpgradeFood()
    {
        if (totalGold >= upgradeCostFood && maxFood < 15)
        {
            totalGold -= upgradeCostFood;
            maxFood++;
            UpdateUI();
            return true;
        }
        return false;
    }


    public void UpdateUI()
    {
        foodText.text = "Food: " + totalFood;
        goldText.text = "Gold: " + totalGold;
        farmerCostText.text = "Hire Farmer: " + farmerCost;
        plotIncreaseXCostText.text = "Expand: " + plotCost * PlotWidth;
        plotIncreaseYCostText.text = "Expand: " + plotCost * PlotLength;
    }

    private void PlacePlotIncreaseUI()
    {
        //If plotlength or width is 1 we want the value to be 2 
        plotIncreaseXUI.position = new Vector3( PlotLength-1, 0, PlotWidth * 2);
        plotIncreaseYUI.position = new Vector3(PlotLength * 2, 0, PlotWidth-1);
    }

    public void IncreasePlotLength(int y)
    {
        var cost = plotCost * PlotLength;
        if (cost > totalGold)
            return;
        totalGold -= cost;
        for (int i = PlotLength; i < PlotLength+y; i++)
        {
            for (int j = 0; j < PlotWidth; j++)
            {
                Instantiate(farmPlotPrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity, plotParent);
            }
        }
        PlotLength += y;
        PlacePlotIncreaseUI();
        UpdateUI();
    }
    public void IncreasePlotWidth(int x)
    {
        var cost = plotCost * PlotWidth;
        if (cost > totalGold)
            return;
        totalGold -= cost;
        for (int j = PlotWidth; j < PlotWidth+x; j++)
        {
            for (int i = 0; i < PlotLength; i++)
            {
                Instantiate(farmPlotPrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity, plotParent);
            }
        }
        PlotWidth += x;
        PlacePlotIncreaseUI();
        UpdateUI();
    }
}
