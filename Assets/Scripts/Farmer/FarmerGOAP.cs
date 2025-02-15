using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Farmer GOAP (Handles Farmer AI decision-making)
public class FarmerGOAP : MonoBehaviour, IGOAPNeeds, IGOAPNavigation, IGOAPResourceHandler, IGOAPFarmTasks
{
    public enum FarmerState { Idle, Moving, Planting, Watering, Harvesting, Eating, Sleeping, Resting }
    public FarmerState CurrentState { get; private set; } = FarmerState.Idle;

    public GameObject FarmerUIPrefab;
    private GameObject farmerUI;
    private FarmerNeeds needs => GetComponent<FarmerNeeds>();
    private FarmManager manager => FarmManager.Instance;
    public Transform CurrentFarmPlot { get; set; } // Tracks the current farm plot

    private Dictionary<string, bool> worldState => FarmManager.Instance.worldState;
    private NavMeshAgent agent; 
    public List<IInteractable> interactables = new List<IInteractable>(); // List of all interactables


    // Needs
    public bool IsHungry => hunger < 20f;
    public bool IsTired => fatigue < 20f;
    public bool IsUnmotivated => motivation < 20f;
    public float hunger => needs.hunger;
    public float fatigue => needs.fatigue;
    public float motivation => needs.motivation;
    public int seeds;
    public int water;
    public int food;
    public int maxFood => manager.maxFood;
    public int maxSeeds => manager.maxSeeds;
    public int maxWater => manager.maxWater;

    public async Task Eat() { CurrentState = FarmerState.Eating; await Task.Delay(3000); }
    public async Task Sleep() { needs.fatigue = 100f; CurrentState = FarmerState.Sleeping; await Task.Delay(8000); }
    public async Task Rest() { needs.motivation = 100f; CurrentState = FarmerState.Resting; await Task.Delay(5000); }

    // Navigation
    public async Task MoveTo(Vector3 position)
    {
        CurrentState = FarmerState.Moving;
        agent.SetDestination(position);
        while (agent!= null && !HasReachedDestination()) await Task.Yield();
        CurrentState = FarmerState.Idle;
    }
    public bool HasReachedDestination() { 
        if( agent==null || !agent.isActiveAndEnabled)
            return false;
        return !agent.pathPending && agent.remainingDistance < 1f; }

    // Resources
    public bool HasSeeds => seeds>0;
    public bool HasWater => water>0;
    public bool HasFood => food>0;
    public void CollectSeeds() { seeds = maxSeeds;}
    public void CollectWater() { water = maxWater;}
    public void CollectFood() { food++; if(food<=5)transform.GetChild(food - 1).gameObject.SetActive(true); }

    public void DepositFood()
    {
        FarmManager.Instance.AddFood(food); food = 0; foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    // Farming Tasks
    public async Task PlantCrop() {CurrentState = FarmerState.Planting; Debug.Log("Planting Crop"); CurrentFarmPlot.tag = "Planted"; CurrentFarmPlot.GetComponent<FarmPlot>().Plant(this); CurrentState = FarmerState.Idle; }
    public async Task WaterCrop() {CurrentState = FarmerState.Watering; Debug.Log("Watering Crop"); CurrentFarmPlot.tag = "Watered"; CurrentFarmPlot.GetComponent<FarmPlot>().Water(this); CurrentState = FarmerState.Idle;  }
    public async Task HarvestCrop() {CurrentState = FarmerState.Harvesting; Debug.Log("Harvesting Crop"); CurrentFarmPlot.tag = "EmptyPlot"; CurrentFarmPlot.GetComponent<FarmPlot>().Harvest(this); CurrentState = FarmerState.Idle; }

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        farmerUI = Instantiate(FarmerUIPrefab, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        farmerUI.GetComponent<FarmerUI>().SetFarmer(transform);

        // Find all interactable objects
        interactables.Clear();
        interactables.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>());
        ShowUI(false);
        ThinkAndAct();
    }
    private async Task ThinkAndAct()
    {
        while (true)
        {
            print("Thinking..."+interactables.Count+" options");
            // Choose the most important interaction
            IInteractable bestTarget = interactables
                .OrderByDescending(interactable => interactable.PriorityScore(this))
                .FirstOrDefault();

            if (bestTarget != null)
            {
                bestTarget.Assignee = this;
                await MoveTo(bestTarget.Transform.position);
                await bestTarget.Interact(this);
            }

            await Task.Delay(1000); // Wait before re-evaluating

            // Find all interactable objects
            interactables.Clear();
            interactables.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>());
        }
    }
    public void ShowUI(bool b)
    {
        farmerUI.SetActive(b);
    }
    public void SetWorldState(string key, bool value) { worldState[key] = value; }
}
