using System.Threading.Tasks;
using UnityEngine;

public class WaterSource : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;

    public FarmerGOAP Assignee { get => null; set => value = null; }
    public float PriorityScore(FarmerGOAP farmer)
    {
        return farmer.HasWater ? -1f : 30f; // Medium priority if low on water
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        farmer.CollectWater();
        Debug.Log($"{farmer.name} collected water at {gameObject.name}");
    }
}
