using System.Threading.Tasks;
using UnityEngine;

public class FoodSource : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;
    private FarmManager FarmManager => FarmManager.Instance;
    public int availableFood => FarmManager.totalFood;
    public int maxFood => FarmManager.maxFood;
    public FarmerGOAP Assignee { get => null; set => value = null; }
    public float PriorityScore(FarmerGOAP farmer)
    {
        return farmer.IsHungry && availableFood>1 ? 80f : 0f; // High priority if hungry
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        if (availableFood > 1)
        {
            FarmManager.Instance.totalFood--;
            farmer.Eat();
            farmer.GetComponent<FarmerNeeds>().Eat();
            Debug.Log($"{farmer.name} ate food at {gameObject.name}");
        }
    }
}
