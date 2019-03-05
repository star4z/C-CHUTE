using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Main : MonoBehaviour
{
    private bool isPaused = false;
    private string path = "Assets/text.txt";

    public int cameraSlack;

    public float leftEnd = 15f;
    public float rightEnd = 88f;

    public bool buttonEnabled = true;

    private HashSet<string> leftys = new HashSet<string>() { "windham", "porter", "mullen" };

    private HashSet<string> allCharacters = new HashSet<string>() { "windham", "stuart", "leblanc", "mullen", "porter", "polyorketes" };

    int convoIndex = -1;
    List<Conversation> allConvos;
    DisplayScript displayScript;

    private List<string> playerOrder;

    private List<string> endMessages;

    GameObject player;
    GameObject cameraObject;
    Rigidbody2D playerRigidBody;
    bool buttonPressed;
    float move;
    public float playerSpeed = 2.0f;

    public Character other;

    AudioSource audioSource;

    public AudioClip[] audioClips = new AudioClip[8];

    public AudioClip doorSound;

    void Start()
    {
        Application.targetFrameRate = 30;

        displayScript = GameObject.Find("Canvas").GetComponent<DisplayScript>();

        allConvos = new List<Conversation>();
        playerOrder = new List<string>();

        readAllConvos();
        printAllConvosToConsole();

        //Hide dialog box

        GameObject.Find("dialog_box").transform.localScale = new Vector3(0, 0, 0);


        cameraObject = GameObject.Find("Camera");

        audioSource = GameObject.Find("steps").GetComponent<AudioSource>();

        Resume();
    }

    int numberBetweenSteps = 0;
    void Update()
    {
        move = Input.GetAxis("Horizontal");
        buttonPressed = Input.GetKeyDown("space") || Input.GetKeyUp("return");

        // other = GameObject.Find(allConvos[nextDialog].Trigger).GetComponent<Character>();

        if (!isPaused)
        {
            playerRigidBody.velocity = new Vector2(move * playerSpeed, 0);

            if (move > 0)
            {
                player.GetComponent<SpriteRenderer>().flipX = true;
            }
            if (move < 0)
            {
                player.GetComponent<SpriteRenderer>().flipX = false;
            }

            // if (move != 0)
            // {

            //     audioSource.transform.position = player.transform.position;
            //     System.Random rnd = new System.Random();
            //     if (numberBetweenSteps == 0)
            //     {
            //         int clipNo = rnd.Next(0, 8);
            //         audioSource.clip = audioClips[clipNo];
            //         audioSource.Play();
            //         numberBetweenSteps = 480;
            //     }
            //     else
            //     {
            //         numberBetweenSteps--;
            //     }
            // }

            updateCameraPos();

            wanderAI();

            if (other.CollisionWith != null && other.EnteredTrigger && other.CollisionWith.name == player.name)
            {
                // GameObject otherCharacter = GameObject.Find(other.name);

                other.GetComponent<SpriteRenderer>().flipX = (other.transform.position.x - player.transform.position.x) < 0;

                displayScript.StartConvo(allConvos[convoIndex]);
                other.OnTriggerHandled();
            }
        }
        else
        {
            playerRigidBody.velocity = new Vector2(0, 0);
            cameraObject.transform.position = new Vector3(player.transform.position.x, 0, -1);

            if (buttonPressed && buttonEnabled)
            {
                displayScript.StartNextEvent();
            }
        }
    }

    /** when called, all AI move. Each has their own list of movements,  a random combination of left, right, and stay,
    for a random length of time (greater than 1/2 seconds)
     */
    private void wanderAI()
    {

    }

    private void updateCameraPos()
    {
        Vector3 cameraPos = cameraObject.transform.position;
        float offset = cameraPos.x - player.transform.position.x;

        if (Math.Abs(offset) > cameraSlack)
        {
            cameraObject.transform.position = new Vector3(player.transform.position.x + (offset * cameraSlack / Math.Abs(offset)), 0, -1);
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        Debug.Log("resumed");
        if (convoIndex < allConvos.Count - 1)
        {
            convoIndex++;

            displayScript.StartTransition();

            if (player != null)
            {
                player.GetComponent<BoxCollider2D>().isTrigger = true;
            }

            player = GameObject.Find(playerOrder[convoIndex]);
            playerRigidBody = player.GetComponent<Rigidbody2D>();
            player.GetComponent<BoxCollider2D>().isTrigger = false;
            player.GetComponent<SpriteRenderer>().sortingLayerName = "Player";

            resetCharactersForScene();

            other = GameObject.Find(allConvos[convoIndex].Trigger).GetComponent<Character>();

            float startTime = Time.time * 1000;

            while ((Time.time * 1000) - startTime > 2000) ;

            displayScript.EndTransition();
        }
        else
        {
            //TODO: exit animation?
            displayScript.StartTransition();
            Debug.Log("Game Over");
            Application.Quit(); //only works with built project
        }

        //Empties triggers for all characters passed
        foreach (string character in allCharacters)
        {
            GameObject.Find(character).GetComponent<Character>().OnTriggerHandled();
        }
        isPaused = false;
    }

    /** Set protagonist y to -4.5. Set protagonist x to either left or right end, depending on who it is.
Distribute other characters between those two points, and give them small, random y-differences from -4.5 */
    void resetCharactersForScene()
    {
        Vector3 currentPos = player.transform.position;
        float position = leftys.Contains(player.name) ? leftEnd : rightEnd;
        player.transform.position = new Vector3(position, -4.5f, 0.0f);
        foreach (string character in allCharacters)
        {
            if (character != player.name)
            {
                GameObject otherCharacter = GameObject.Find(character);

                float y = -4.5f + UnityEngine.Random.Range(0.5f, -0.5f);

                if (leftys.Contains(otherCharacter.name))
                {
                    otherCharacter.transform.position = new Vector3(
                        leftEnd + UnityEngine.Random.Range(5, 55),
                        y,
                        0
                    );
                }
                else
                {
                    otherCharacter.transform.position = new Vector3(
                        rightEnd - UnityEngine.Random.Range(5, 55),
                        y,
                        0
                    );
                }
                if (y < -4.5)
                {
                    otherCharacter.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

                }
                else
                {
                    otherCharacter.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                }
            }
        }

        if (allConvos[convoIndex].Trigger == "kloro")
        {
            GameObject.Find("kloro").transform.position = new Vector3(3, -4.5f, 0);
            // audioSource.clip = doorSound;
            // audioSource.Play();
        }
        else
        {
            // Transform transform = GameObject.Find("kloro").transform;
            GameObject.Find("kloro").transform.position = new Vector3(-12, -4.5f, 0);
            // if (!transform.position.Equals(new Vector3(-12, -4.5f, 0)))
            // {
            //     audioSource.clip = doorSound;
            //     audioSource.Play();
            // }

        }
    }


    public bool IsPaused()
    {
        return isPaused;
    }

    void readAllConvos()
    {
        StreamReader reader = new StreamReader(path);

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (!(string.IsNullOrEmpty(line) || line[0] == '/'))
            {
                switch (line[0])
                {
                    case '!':
                        //New conversation, with trigger name
                        allConvos.Add(new Conversation(line.Substring(1)));

                        //If there's a convo with the same protag as the previous one, add the protag to the protag order again
                        if (playerOrder.Count < allConvos.Count)
                        {
                            playerOrder.Add(playerOrder[playerOrder.Count - 1]);
                        }
                        break;
                    case '*':
                        playerOrder.Add(line.Substring(1));
                        break;
                    case '?':
                        Debug.Log(line.Substring(1, 4));
                        switch (line.Substring(1, 4))
                        {
                            case "aud:":
                                break;
                            case "anm:":
                                break;
                            case "dlg:":
                                string[] splits = line.Substring(5).Split(new char[] { '%' });
                                Dialog d = new Dialog(splits[0], splits[1]);
                                addDialog(d);
                                break;
                        }
                        break;
                    case '#':
                        bool fade = line[1] == '1';
                        allConvos.Add(new ImmediateConvo(line.Substring(2), fade));
                        break;
                    default:
                        string[] substrings = line.Split(new char[] { '%' });
                        Dialog dialog = new Dialog(substrings[0], substrings[1]);
                        addDialog(dialog);
                        break;
                }
            }
        }

        reader.Close();
    }

    private void addDialog(Dialog dialog)
    {
        List<MyEvent> events = allConvos[allConvos.Count - 1].Events;
        if (events.Count > 0)
        {
            MyEvent prevEvent = events[events.Count - 1];


            //If the event already contains a dialog, add a new event, else add the dialog to the event
            if (prevEvent.contains(typeof(Dialog)))
            {
                MyEvent mEvent = new MyEvent();
                mEvent.miniEvents.Add(dialog);
                allConvos[allConvos.Count - 1].Events.Add(mEvent);
            }
            else
            {
                prevEvent.miniEvents.Add(dialog);
            }
        }
        else
        {
            MyEvent mEvent = new MyEvent();
            mEvent.miniEvents.Add(dialog);
            allConvos[allConvos.Count - 1].Events.Add(mEvent);
        }
    }

    void printAllConvosToConsole()
    {
        foreach (Conversation conversation in allConvos)
        {
            string nextConversation = conversation.Trigger + "{";
            foreach (MyEvent Event in conversation.Events)
            {
                nextConversation += "[";
                foreach (MiniEvent miniEvent in Event.miniEvents)
                {
                    nextConversation += miniEvent.Name.ToUpper();
                    if (miniEvent is Dialog)
                    {
                        nextConversation += " = '" + (miniEvent as Dialog).Text + "', ";
                    }
                }
                nextConversation = nextConversation.Substring(0, nextConversation.Length - 2);
                nextConversation += "],";
            }
            nextConversation = nextConversation.Substring(0, nextConversation.Length - 1);
            nextConversation += "}";
            Debug.Log(nextConversation);
        }
    }
}
