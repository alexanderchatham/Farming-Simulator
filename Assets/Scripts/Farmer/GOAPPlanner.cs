using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    public List<IGOAPAction> Plan(List<IGOAPAction> availableActions, Dictionary<string, bool> worldState, string goal)
    {
        List<IGOAPAction> validActions = new List<IGOAPAction>();

        foreach (IGOAPAction action in availableActions)
        {
            if (action.ArePreconditionsMet(worldState))
            {
                validActions.Add(action);
            }
        }

        validActions.Sort((a, b) => a.Cost.CompareTo(b.Cost)); // Prioritize cheaper actions
        return validActions;
    }
}