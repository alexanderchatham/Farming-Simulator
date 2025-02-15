using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FoodSource : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;
    public TextMeshProUGUI text;
    public TextMeshProUGUI buttonText;
    private FarmManager FarmManager => FarmManager.Instance;
    public int availableFood => FarmManager.totalFood;
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
            FarmManager.Instance.UpdateUI();
            Debug.Log($"{farmer.name} ate food at {gameObject.name}");
        }
    }
}
