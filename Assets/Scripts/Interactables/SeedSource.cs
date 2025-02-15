using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SeedSource : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;

    public TextMeshProUGUI text;
    public TextMeshProUGUI buttonText;
    int maxSeeds => FarmManager.Instance.maxSeeds;
    int upgradeCost => FarmManager.Instance.upgradeCostSeeds;
    public FarmerGOAP Assignee { get => null; set => value = null; }

    private async void Awake()
    {
        await Task.Delay(500);
        UpdateUI();
    }
    public float PriorityScore(FarmerGOAP farmer)
    {
        return farmer.HasSeeds ? -1f : 30f; // Medium priority if low on seeds
    }

    public async Task Interact(FarmerGOAP farmer)
    {
        farmer.CollectSeeds();
        Debug.Log($"{farmer.name} collected seeds at {gameObject.name}");
    }
    public void UpgradeSeeds()
    {
        if(FarmManager.Instance.UpgradeSeeds())
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        text.text = "Max Seeds: \n" + maxSeeds;
        buttonText.text = "Upgrade " + upgradeCost;
    }
}
