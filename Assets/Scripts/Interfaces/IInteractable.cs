using System.Threading.Tasks;
using UnityEngine;

public interface IInteractable
{
    Transform Transform { get; }
    float PriorityScore(FarmerGOAP farmer); // Determines importance of interaction
    public FarmerGOAP Assignee { get; set; }
    Task Interact(FarmerGOAP farmer); // Defines interaction behavior
}
