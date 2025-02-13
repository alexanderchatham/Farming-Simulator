using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

// Farmer GOAP (Handles Farmer AI decision-making)
public class FarmerGOAP : MonoBehaviour, IGOAPNeeds, IGOAPNavigation, IGOAPResourceHandler, IGOAPFarmTasks
{
    public Transform CurrentFarmPlot { get; set; } // Tracks the current farm plot
    public Transform SeedStorage;
    public Transform WaterSource;
    public Transform FoodSource;
    public List<IGOAPAction> availableActions = new List<IGOAPAction>();
    public Dictionary<string, bool> worldState = new Dictionary<string, bool>();
    private GOAPPlanner planner = new GOAPPlanner();
    private List<IGOAPAction> actionQueue;
    private NavMeshAgent agent;

    // Needs
    public bool IsHungry => hunger < 20f;
    public bool IsTired => fatigue < 20f;
    public bool IsUnmotivated => motivation < 20f;
    private float hunger = 100f;
    private float fatigue = 100f;
    private float motivation = 100f;
    public void Eat() { hunger = 100f; }
    public void Sleep() { fatigue = 100f; }
    public void Rest() { motivation = 100f; }

    // Navigation
    public async Task MoveTo(Vector3 position) { agent.SetDestination(position); while (!HasReachedDestination()) await Task.Yield(); }
    public bool HasReachedDestination() { return !agent.pathPending && agent.remainingDistance < 0.5f; }

    // Resources
    public bool HasSeeds => worldState.ContainsKey("HasSeeds") && worldState["HasSeeds"];
    public bool HasWater => worldState.ContainsKey("HasWater") && worldState["HasWater"];
    public bool HasFood => worldState.ContainsKey("HasFood") && worldState["HasFood"];
    public async void CollectSeeds() { await MoveTo(SeedStorage.position); worldState["HasSeeds"] = true; }
    public async void CollectWater() {await MoveTo(WaterSource.position); worldState["HasWater"] = true; }
    public async void CollectFood()  {await MoveTo(FoodSource.position); worldState["HasFood"] = true; }

    // Farming Tasks
    public async Task PlantCrop()   {await MoveTo(CurrentFarmPlot.position); Debug.Log("Planting Crop"); CurrentFarmPlot.tag = "Planted"; }
    public async Task WaterCrop()   {await MoveTo(CurrentFarmPlot.position); Debug.Log("Watering Crop"); CurrentFarmPlot.tag = "Watered"; }
    public async Task HarvestCrop() {await MoveTo(CurrentFarmPlot.position); Debug.Log("Harvesting Crop"); CurrentFarmPlot.tag = "EmptyPlot"; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        availableActions.Add(new PlantAction());
        availableActions.Add(new WaterAction());
        availableActions.Add(new HarvestAction());
        availableActions.Add(new EatFoodAction());
        availableActions.Add(new SleepAction());
        availableActions.Add(new RestAction());

        CurrentFarmPlot = GameObject.FindWithTag("EmptyPlot").transform;

        worldState["HasSeeds"] = true;
        worldState["PlotEmpty"] = true;
        worldState["NeedsWater"] = false;
        worldState["ReadyToHarvest"] = false;
        StartCoroutine(ThinkAndAct());
    }

    private System.Collections.IEnumerator ThinkAndAct()
    {
        while (true)
        {
            actionQueue = planner.Plan(availableActions, worldState, "FarmManagement");
            if (actionQueue.Count > 0)
            {
                foreach (IGOAPAction action in actionQueue)
                {
                    action.Execute(this);
                    yield return new WaitForSeconds(2f);
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }
}