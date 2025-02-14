using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;

public class FarmPlot : MonoBehaviour, IInteractable
{
    public enum PlotState { Empty, Planted, Watered, ReadyToHarvest }
    public PlotState CurrentState = PlotState.Empty;


    public GameObject WaterObject;
    public GameObject SeedsObject;
    public GameObject PlantObject;

    public bool NeedsWater => CurrentState == PlotState.Planted;
    public bool ReadyToHarvest;

    public Transform Transform => transform;
    private FarmerGOAP assignee;
    public FarmerGOAP Assignee { get =>assignee; set => assignee = value; }

    public float PriorityScore(FarmerGOAP farmer)
    {
        if (assignee != farmer && assignee != null)
        {
            return -1f;
        }
        if (ReadyToHarvest) return 100f; // Highest priority
        if (NeedsWater && farmer.HasWater) return 50f;
        if (CurrentState == PlotState.Empty && farmer.HasSeeds) return 20f; // Lower priority
        return -1f; // No action needed
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        
        if (ReadyToHarvest) Harvest(farmer);
        else if (NeedsWater) Water(farmer);
        else if (CurrentState == PlotState.Empty) Plant(farmer);
        assignee = null;
    }

    public void Plant(FarmerGOAP farmer)
    {
        if (CurrentState == PlotState.Empty)
        {
            farmer.seeds--;
            Debug.Log($"{gameObject.name}: Seeds planted!");
            CurrentState = PlotState.Planted;
            SeedsObject.SetActive(true);
        }
    }

    public void Water(FarmerGOAP farmer)
    {
        if (NeedsWater)
        {
            farmer.water--;
            Debug.Log($"{gameObject.name}: Watered!");
            CurrentState = PlotState.Watered;
            WaterObject.SetActive(true);
            StartCoroutine(Growing());
        }
    }

    IEnumerator Growing()
    {
        yield return new WaitForSeconds(5f);
        ReadyToHarvest = true;
        PlantObject.SetActive(true);
        WaterObject.SetActive(false);
        SeedsObject.SetActive(false);
    }

    public void Harvest(FarmerGOAP farmer)
    {
        if (ReadyToHarvest)
        {
            farmer.CollectFood();
            Debug.Log($"{gameObject.name}: Harvested!");
            ReadyToHarvest = false;
            PlantObject.SetActive(false);
            CurrentState = PlotState.Empty;
        }
    }
}
