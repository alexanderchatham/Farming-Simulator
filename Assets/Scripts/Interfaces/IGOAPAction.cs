using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Interface for all GOAP actions
public interface IGOAPAction
{
    string ActionName { get; }
    float Cost { get; }
    float GetCost(FarmerGOAP farmer);
    Task Execute(FarmerGOAP farmer);
}

// Interface for NPC needs
public interface IGOAPNeeds
{
    bool IsHungry { get; }
    bool IsTired { get; }
    bool IsUnmotivated { get; }
    Task Eat();
    Task Sleep();
    Task Rest();
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
public class PlantAction : IGOAPAction
{
    public string ActionName => "Plant Seeds";
    public float Cost => 2f;
    public async Task Execute(FarmerGOAP farmer)
    {
        await farmer.PlantCrop();
    }

    public float GetCost(FarmerGOAP farmer)
    {
        return farmer.HasSeeds ? Cost : 0f;
    }
}

public class WaterAction : IGOAPAction
{
    public string ActionName => "Water Crops";
    public float Cost => 1f;
    public async Task Execute(FarmerGOAP farmer)
    {
        await farmer.WaterCrop();
    }

    public float GetCost(FarmerGOAP farmer)
    {
        return farmer.HasWater ? Cost : 0f;
    }
}

public class HarvestAction : IGOAPAction
{
    public string ActionName => "Harvest Crops";
    public float Cost => 3f;
    public async Task Execute(FarmerGOAP farmer)
    {
        await farmer.HarvestCrop();
    }

    public float GetCost(FarmerGOAP farmer)
    {
        return farmer.CurrentFarmPlot.GetComponent<FarmPlot>().ReadyToHarvest ? Cost : 0f;
    }
}

public class EatFoodAction : IGOAPAction
{
    public string ActionName => "Eat Food";
    public float Cost => 1f;
    public Task Execute(FarmerGOAP farmer)
    {
        farmer.Eat();
        return Task.CompletedTask;
    }

    public float GetCost(FarmerGOAP farmer)
    {
        return farmer.HasFood && farmer.IsHungry ? Cost : 0f;
    }
}

public class SleepAction : IGOAPAction
{
    public string ActionName => "Sleep";
    public float Cost => 3f;
    public Task Execute(FarmerGOAP farmer)
    {
        farmer.Sleep();
        return Task.CompletedTask;
    }

    public float GetCost(FarmerGOAP farmer)
    {
        return farmer.IsTired ? Cost : 0f;
    }
}

public class RestAction : IGOAPAction
{
    public string ActionName => "Rest";
    public float Cost => 2f;
    public Task Execute(FarmerGOAP farmer)
    {
        farmer.Rest();
        return Task.CompletedTask;
    }

    public float GetCost(FarmerGOAP farmer)
    {
        return farmer.IsUnmotivated ? Cost : 0f;
    }
}
