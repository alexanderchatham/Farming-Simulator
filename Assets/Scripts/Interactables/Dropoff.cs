using System.Threading.Tasks;
using UnityEngine;

public class Dropoff : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;
    public FarmerGOAP Assignee { get => null; set => value = null; }
    public float PriorityScore(FarmerGOAP farmer)
    {
        if(farmer.food >= 5)
            return 110f; // High priority if hungry
        if (farmer.food > 2)
            return 40f;
        if (farmer.food > 1)
            return 20f;
        if (farmer.food == 1)
            return 10f;
        return 0f;
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        farmer.DepositFood();
        Debug.Log($"{farmer.name} dropped off food at {gameObject.name}");
    }
}
