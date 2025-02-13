using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Interface for all GOAP actions
public interface IGOAPAction
{
    string ActionName { get; }
    float Cost { get; }
    Dictionary<string, bool> Preconditions { get; }
    Dictionary<string, bool> Effects { get; }
    bool ArePreconditionsMet(Dictionary<string, bool> worldState);
    void Execute(FarmerGOAP farmer);
}

// Interface for NPC needs
public interface IGOAPNeeds
{
    bool IsHungry { get; }
    bool IsTired { get; }
    bool IsUnmotivated { get; }
    void Eat();
    void Sleep();
    void Rest();
}

// Interface for movement and navigation
public interface IGOAPNavigation
{
    Task MoveTo(Vector3 position);
    bool HasReachedDestination();
}

// Interface for resource management
public interface IGOAPResourceHandler
{
    bool HasSeeds { get; }
    bool HasWater { get; }
    bool HasFood { get; }
    void CollectSeeds();
    void CollectWater();
    void CollectFood();
}

// Interface for farm tasks
public interface IGOAPFarmTasks
{
    Task PlantCrop();
    Task WaterCrop();
    Task HarvestCrop();
}
// Actions

// Actions
public class PlantAction : IGOAPAction
{
    public string ActionName => "Plant Seeds";
    public float Cost => 2f;
    public Dictionary<string, bool> Preconditions { get; } = new Dictionary<string, bool> { { "HasSeeds", true }, { "PlotEmpty", true } };
    public Dictionary<string, bool> Effects { get; } = new Dictionary<string, bool> { { "CropPlanted", true } };
    public bool ArePreconditionsMet(Dictionary<string, bool> worldState)
    {
        foreach (var precondition in Preconditions)
        {
            if (!worldState.ContainsKey(precondition.Key) || worldState[precondition.Key] != precondition.Value)
            {
                return false;
            }
        }
        return true;
    }
    public async void Execute(FarmerGOAP farmer) {
        await farmer.PlantCrop();
        farmer.worldState["PlotEmpty"] = false;   // Mark plot as occupied
        farmer.worldState["CropPlanted"] = true;  // A crop now exists
    }
}

public class WaterAction : IGOAPAction
{
    public string ActionName => "Water Crops";
    public float Cost => 1f;
    public Dictionary<string, bool> Preconditions { get; } = new Dictionary<string, bool> { { "CropPlanted", true }, { "NeedsWater", true } };
    public Dictionary<string, bool> Effects { get; } = new Dictionary<string, bool> { { "CropsWatered", true } };
    public bool ArePreconditionsMet(Dictionary<string, bool> worldState)
    {
        foreach (var precondition in Preconditions)
        {
            if (!worldState.ContainsKey(precondition.Key) || worldState[precondition.Key] != precondition.Value)
            {
                return false;
            }
        }
        return true;
    }
    public async void Execute(FarmerGOAP farmer) {
        await farmer.WaterCrop();
        farmer.worldState["needsWater"] = false;
    }
}

public class HarvestAction : IGOAPAction
{
    public string ActionName => "Harvest Crops";
    public float Cost => 3f;
    public Dictionary<string, bool> Preconditions { get; } = new Dictionary<string, bool> { { "CropPlanted", true }, { "CropsWatered", true }, { "ReadyToHarvest", true } };
    public Dictionary<string, bool> Effects { get; } = new Dictionary<string, bool> { { "HarvestComplete", true } };
    public bool ArePreconditionsMet(Dictionary<string, bool> worldState)
    {
        foreach (var precondition in Preconditions)
        {
            if (!worldState.ContainsKey(precondition.Key) || worldState[precondition.Key] != precondition.Value)
            {
                return false;
            }
        }
        return true;
    }
    public async void Execute(FarmerGOAP farmer) {
        await farmer.HarvestCrop();
    }
}

public class EatFoodAction : IGOAPAction
{
    public string ActionName => "Eat Food";
    public float Cost => 1f;
    public Dictionary<string, bool> Preconditions { get; } = new Dictionary<string, bool> { { "HasFood", true } };
    public Dictionary<string, bool> Effects { get; } = new Dictionary<string, bool> { { "NotHungry", true } };
    public bool ArePreconditionsMet(Dictionary<string, bool> worldState)
    {
        foreach (var precondition in Preconditions)
        {
            if (!worldState.ContainsKey(precondition.Key) || worldState[precondition.Key] != precondition.Value)
            {
                return false;
            }
        }
        return true;
    }
    public void Execute(FarmerGOAP farmer) { farmer.Eat(); }
}

public class SleepAction : IGOAPAction
{
    public string ActionName => "Sleep";
    public float Cost => 3f;
    public Dictionary<string, bool> Preconditions { get; } = new Dictionary<string, bool> { { "HasBed", true } };
    public Dictionary<string, bool> Effects { get; } = new Dictionary<string, bool> { { "NotTired", true } };
    public bool ArePreconditionsMet(Dictionary<string, bool> worldState)
    {
        foreach (var precondition in Preconditions)
        {
            if (!worldState.ContainsKey(precondition.Key) || worldState[precondition.Key] != precondition.Value)
            {
                return false;
            }
        }
        return true;
    }
    public void Execute(FarmerGOAP farmer) { farmer.Sleep(); }
}

public class RestAction : IGOAPAction
{
    public string ActionName => "Rest";
    public float Cost => 2f;
    public Dictionary<string, bool> Preconditions { get; } = new Dictionary<string, bool> { { "HasRestArea", true } };
    public Dictionary<string, bool> Effects { get; } = new Dictionary<string, bool> { { "Motivated", true } };
    public bool ArePreconditionsMet(Dictionary<string, bool> worldState)
    {
        foreach (var precondition in Preconditions)
        {
            if (!worldState.ContainsKey(precondition.Key) || worldState[precondition.Key] != precondition.Value)
            {
                return false;
            }
        }
        return true;
    }
    public void Execute(FarmerGOAP farmer) { farmer.Rest(); }
}
