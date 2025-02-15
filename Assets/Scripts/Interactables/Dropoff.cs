using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Dropoff : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;
    public TextMeshProUGUI text;
    public TextMeshProUGUI buttonText;
    private int maxFood => FarmManager.Instance.maxFood;
    private int upgradeCost => FarmManager.Instance.upgradeCostFood;

    public FarmerGOAP Assignee { get => null; set => value = null; }
    private async void Awake()
    {
        await Task.Delay(500);
        UpdateUI();
    }
    public float PriorityScore(FarmerGOAP farmer)
    {
        var foodPercent = (float)farmer.food / (float)maxFood;
        if ( foodPercent>= 1)
            return 110f; // High priority if hungry
        if (foodPercent > .5f)
            return 3f;
        if (foodPercent > .25f)
            return 2f;
        return 0f;
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        farmer.DepositFood();
        Debug.Log($"{farmer.name} dropped off food at {gameObject.name}");
    }

    public void UpgradeCarryCapacity()
    {
        if (FarmManager.Instance.UpgradeFood())
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        text.text = "Carrying Capacity: \n" + maxFood;
        buttonText.text = "Upgrade " + upgradeCost;
    }
}
