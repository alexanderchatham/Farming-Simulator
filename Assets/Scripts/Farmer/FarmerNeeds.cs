using UnityEngine;

// Farmer Needs System
public class FarmerNeeds : MonoBehaviour
{
    public float hunger = 100f;
    public float fatigue = 100f;
    public float motivation = 100f;

    public float hungerDecay = 0.5f;
    public float fatigueDecay = 0.3f;
    public float motivationDecay = 0.2f;

    public bool IsHungry => hunger < 20f;
    public bool IsTired => fatigue < 20f;
    public bool IsUnmotivated => motivation < 20f;

    void Update()
    {
        hunger -= Time.deltaTime * hungerDecay;
        fatigue -= Time.deltaTime * fatigueDecay;
        motivation -= Time.deltaTime * motivationDecay;
    }

    public void Eat() { hunger = 100f; }
    public void Sleep() { fatigue = 100f; }
    public void Rest() { motivation = 100f; }
}
