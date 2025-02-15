using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class WaterSource : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;
    public TextMeshProUGUI text;
    public TextMeshProUGUI buttonText;
    int maxWater => FarmManager.Instance.maxWater;
    int upgradeCost => FarmManager.Instance.upgradeCostWater;

    public FarmerGOAP Assignee { get => null; set => value = null; }

    private async void Awake()
    {
        await Task.Delay(500);
        UpdateUI();
    }
    public float PriorityScore(FarmerGOAP farmer)
    {
        return farmer.HasWater ? -1f : 30f; // Medium priority if low on water
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        farmer.CollectWater();
        Debug.Log($"{farmer.name} collected water at {gameObject.name}");
    }
    public void UpgradeWater()
    {
        if(FarmManager.Instance.UpgradeWater())
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        text.text = "Max Water: \n" + maxWater;
        buttonText.text = "Upgrade " + upgradeCost;
    }
}
