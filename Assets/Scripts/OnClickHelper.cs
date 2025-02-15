using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnClickHelper : MonoBehaviour
{
    public UnityEvent OnClickEvent;
    public UnityEvent mouseEnterEvent;
    public UnityEvent mouseExitEvent;
    public enum triggerType
    {
        OnDown,
        OnUp,
        Held
    }
    public triggerType type;
    public float holdTime;
    private bool timerRunning;
    private void OnMouseDown()
    {
        if(type == triggerType.OnDown)
            triggerEvents();
        if (type == triggerType.Held)
            startTimer();
    }

    private void OnMouseUp()
    {
        if (type == triggerType.OnUp)
            triggerEvents();
        if (type == triggerType.Held)
            endTimer();
    }

    private void OnMouseExit()
    {
        if (type == triggerType.Held && timerRunning)
            endTimer();
        mouseExitEvent.Invoke();
    }
    private void OnMouseEnter()
    {
        mouseEnterEvent.Invoke();   
    }
    private void startTimer()
    {
        StartCoroutine(RunTimer());
    }
    IEnumerator RunTimer()
    {
        float timer = 0;
        timerRunning = true;
        while(timer < holdTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;

        }
        triggerEvents();
        timerRunning = false;
    }
    private void endTimer()
    {
        StopAllCoroutines();
    }


    private void triggerEvents()
    {
        OnClickEvent.Invoke();
        print($"{type} Triggered");
    }
}
