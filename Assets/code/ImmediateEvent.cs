using System;
using System.Collections.Generic;
using UnityEngine;

class ImmediateConvo : Conversation
{

    private Main GameManager;

    private bool Fade;

    public ImmediateConvo(string trigger, bool fade) : base(trigger)
    {
        this.Trigger = trigger; //NOT really using this as trigger in this case;
        Fade = fade;

        GameManager = GameObject.Find("GameManager").GetComponent<Main>();

        Start();
    }

    void Start()
    {
        if (Fade)
        {
            
        }
    }
}