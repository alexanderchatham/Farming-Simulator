using System;
using UnityEngine;
using UnityEngine.UI;
public class FarmerUI : MonoBehaviour
{
    public float offset;
    public Transform farmer;
    private FarmerNeeds needs => farmer.GetComponent<FarmerNeeds>();
    private FarmerGOAP FarmerGOAP => farmer.GetComponent<FarmerGOAP>();
    public Image waterSlider;
    public Image seedSlider;
    public Image hungerSlider;
    public Image fatigueSlider;
    public CanvasGroup cg;
    public void SetFarmer(Transform farmer)
    {
        this.farmer = farmer;
    }

    private void OnDisable()
    {
        cg.alpha = 0;
    }

    public void UpdateUI(float water,float seed,float hunger,float fatigue)
    {
        waterSlider.fillAmount = water;
        seedSlider.fillAmount = seed;
        hungerSlider.fillAmount = hunger;
        fatigueSlider.fillAmount = fatigue;
    }
    public void UpdateUI()
    {
        var waterSliderValue = (float)FarmerGOAP.water / (float)FarmerGOAP.maxWater;
        var seedSliderValue = (float)FarmerGOAP.seeds / (float)FarmerGOAP.maxSeeds;
        var hungerSliderValue = needs.hunger / 100f;
        var fatigueSliderValue = needs.fatigue / 100f;
        UpdateUI(waterSliderValue, seedSliderValue, hungerSliderValue, fatigueSliderValue);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(farmer == null) return;
        transform.position = Camera.main.WorldToScreenPoint(farmer.position ) + Vector3.up * offset;
        cg.alpha = 1;
        UpdateUI();
    }
}
