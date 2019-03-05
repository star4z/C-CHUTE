using System;
using System.Collections.Generic;
using UnityEngine;

public class MyEvent
{
    public List<MiniEvent> miniEvents { get; set; }

    private bool isActive = false;

    private Main GameManager;

    public MyEvent()
    {
        this.miniEvents = new List<MiniEvent>();
        GameManager = GameObject.Find("GameManager").GetComponent<Main>();
    }
    public MyEvent(List<MiniEvent> miniEvents)
    {
        this.miniEvents = miniEvents;
        GameManager = GameObject.Find("GameManager").GetComponent<Main>();
    }

    public void Start()
    {
        GameManager.Pause();
        foreach (MiniEvent mini in miniEvents)
        {
            mini.Start();
        }

        isActive = true;
    }

    public void Stop()
    {
        if (isActive)
        {
            foreach (MiniEvent miniEvent in miniEvents)
            {
                miniEvent.Stop();
            }
        }

    }

    public virtual bool contains(Type Name)
    {
        foreach (MiniEvent mini in miniEvents){
            if (mini.GetType() == Name){
                return true;
            }
        }
        return false;
    }
}