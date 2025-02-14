using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


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

