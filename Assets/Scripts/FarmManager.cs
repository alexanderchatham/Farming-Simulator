using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }

    public int amountOfWorkers = 5;
    public int PlotLength = 3;
    public int PlotWidth = 3;
    public int totalFood = 3;
    public int totalGold = 0;
    public List<FarmPlot> farmPlots = new List<FarmPlot>();
    public Dictionary<string, bool> worldState = new Dictionary<string, bool>();
    public GameObject farmerPrefab;
    public GameObject farmPlotPrefab;
    public Transform farmerParent;
    public Transform plotParent;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI goldText;
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
        // Find all farm plots in the scene
        farmPlots.AddRange(FindObjectsOfType<FarmPlot>());
        for (int i = 0; i < PlotLength; i++)
        {
            for (int j = 0; j < PlotWidth; j++)
            {
                Instantiate(farmPlotPrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity,plotParent);
            }
        }
        for (int i = 0; i < amountOfWorkers; i++)
        {
            SpawnFarmer();
        }
        UpdateUI();
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

    public void UpdateUI()
    {
        foodText.text = "Food: " + totalFood;
        goldText.text = "Gold: " + totalGold;
    }
}
