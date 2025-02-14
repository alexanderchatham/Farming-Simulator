using System.Threading.Tasks;
using UnityEngine;

public class SeedSource : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;

    public FarmerGOAP Assignee { get => null; set => value = null; }
    public float PriorityScore(FarmerGOAP farmer)
    {
        return farmer.HasSeeds ? -1f : 30f; // Medium priority if low on seeds
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        farmer.CollectSeeds();
        Debug.Log($"{farmer.name} collected seeds at {gameObject.name}");
    }
}
