using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnClickHelper : MonoBehaviour
{
    public UnityEvent OnClickEvent;
    public UnityEvent mouseEnterEvent;
    public UnityEvent mouseExitEvent;
    public bool UpCancelable;
    private bool cancel;
    public enum triggerType
    {
        OnDown,
        OnUp,
        Held,
        DownAndUp,
        DownAndHeld,
        UpAndHeld,
        DoubleClick
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
        if (type == triggerType.DoubleClick)
            doubleClick();
        
    }
    float doubleClickTimeLimit = 0.25f;
    float lastClickTime = 0;
    private void doubleClick()
    {
        if(lastClickTime + doubleClickTimeLimit > Time.time)
        {
            triggerEvents();
            lastClickTime = 0;
        }
        else
        {
            lastClickTime = Time.time;
        }
    }

    private void OnMouseUp()
    {
        if (type == triggerType.OnUp&&!cancel)
            triggerEvents();
        if (type == triggerType.Held)
            endTimer();
        cancel = false;
    }

    private void OnMouseExit()
    {
        if (type == triggerType.Held && timerRunning)
            endTimer();
        mouseExitEvent.Invoke();
        if(UpCancelable)
            cancel = true;
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


    public void triggerEvents()
    {
        OnClickEvent.Invoke();
        print($"{type} Triggered");
    }
}
