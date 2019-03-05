using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DisplayScript : MonoBehaviour
{
    private Main gameManager;
    private GameObject dialogBox;

    private Conversation conversation;

    public int currentEventNo;

    private Fade fade;


    // Use this for initialization
    void Start()
    {
        dialogBox = GameObject.Find("dialog_box");
        gameManager = GameObject.Find("GameManager").GetComponent<Main>();
        fade = GameObject.Find("fade").GetComponent<Fade>();
    }

    public void StartConvo(Conversation conversation)
    {
        this.conversation = conversation;

        currentEventNo = -1;
        // this.events = conversation.Events;
        StartNextEvent();
    }

    public void StartNextEvent()
    {
        // Debug.Log(currentEventNo + ", " + conversation.Events.Count);
        if (currentEventNo != -1)
        {
            conversation.Events[currentEventNo].Stop();
        }
        currentEventNo++;
        if (currentEventNo < conversation.Events.Count)
        {
            StartEvent(conversation.Events[currentEventNo]);
        }
        else
        {

            gameManager.Resume();
        }
    }

    public void StartEvent(MyEvent myEvent)
    {
        myEvent.Start();
    }

    public void StartTransition()
    {
        fade = GameObject.Find("fade").GetComponent<Fade>();

        // Debug.Log("Fading out...");
        // fade.FadeOut();
    }

    public void EndTransition()
    {
        fade = GameObject.Find("fade").GetComponent<Fade>();

        // Debug.Log("Fading in...");
        // fade.FadeIn();
    }

}
