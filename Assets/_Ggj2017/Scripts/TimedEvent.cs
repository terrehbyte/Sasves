using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvent : MonoBehaviour
{
    private Timer timer = new Timer();

    public float secondsToFire = 1.0f;

    public UnityEvent onTimerExpired;

    void Start()
    {
        timer.duration = secondsToFire;
        timer.setActive(true);
    }

    void Update()
    {
        timer.update();

        if(timer.isPassed())
        {
            onTimerExpired.Invoke();
        }
    }
}
