using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer {
    public float duration = 1.0f;
    private float curTime = 0.0f;
    private bool active = false;

    public void init()
    {
        setActive(true);
    }

	public void update () {
		if(active)
        {
            curTime += Time.deltaTime;
            if (curTime >= duration)
                toggleActive();
        }
	}

    public bool isPassed()
    {
        return curTime >= duration;
    }

    public float timePassed()
    {
        return curTime < duration ? curTime : duration;
    }

    public float percentPassed()
    {
        if (duration != 0.0f)
            return timePassed() / duration;
        else
            return 0.0f;
    }

    public float timeRemaining()
    {
        return (duration - curTime) > 0.0f ? duration - curTime : 0.0f;
    }

    public float percentRemaining()
    {
        if (duration != 0.0f)
            return timeRemaining() / duration;
        else
            return 0.0f;
    }

    public void resetTimer()
    {
        curTime = 0.0f;
    }

    public bool isActive()
    {
        return active;
    }
    public void toggleActive()
    {
        active = !active;
    }

    public void setActive(bool activity)
    {
        active = activity;
    }

}
