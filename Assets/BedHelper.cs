using UnityEngine;

public class BedHelper : MonoBehaviour
{
    public int activeBeds = 1;
    public GameObject[] beds;
    public GameObject bedUI;
    public void BuyBed()
    {
        if (FarmManager.Instance.BuyBed() && activeBeds<beds.Length)
        {
            beds[activeBeds].SetActive(true);
            activeBeds++;
            if(activeBeds == beds.Length)
            {
                bedUI.SetActive(false);
            }
        }
    }
}
