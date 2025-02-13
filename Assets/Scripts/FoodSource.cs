using System.Threading.Tasks;
using UnityEngine;

public class FoodSource : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;
    public int availableFood = 10;
    public int maxFood = 10;
    public FarmerGOAP Assignee { get => null; set => value = null; }
    public float PriorityScore(FarmerGOAP farmer)
    {
        return farmer.IsHungry ? 80f : 0f; // High priority if hungry
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        farmer.Eat();
        farmer.GetComponent<FarmerNeeds>().Eat();
        Debug.Log($"{farmer.name} ate food at {gameObject.name}");
    }
}
