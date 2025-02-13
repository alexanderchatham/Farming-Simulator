using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
   
    public Transform Transform => transform;
    private FarmerGOAP assignee;
    public FarmerGOAP Assignee { get => assignee; set => assignee = value; }

    public float PriorityScore(FarmerGOAP farmer)
    {
        if (assignee != farmer && assignee != null)
        {
            return -1f;
        }
        if(farmer.IsTired)
            return 100f;
        return -1f; // No action needed
    }

    public async Task Interact(FarmerGOAP farmer)
    {

        await farmer.Sleep();
        assignee = null;
    }

}
