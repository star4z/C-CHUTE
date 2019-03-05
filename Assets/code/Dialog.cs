using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MiniEvent
{
    public string Text { get; set; }
    private GameObject dialogBox;

    private GameObject textBox;
    private GameObject nameBox;
    private GameObject imageBox;

    // public static Dictionary<string, Texture2D> profileMap;

    public Dialog(string name, string text)
    {
        this.Name = name;
        this.Text = text;

        dialogBox = GameObject.Find("dialog_box");
        textBox = GameObject.Find("text_box");
        nameBox = GameObject.Find("name_box");
        imageBox = GameObject.Find("image_box");
    }

    public override void Start()
    {
        dialogBox.transform.localScale = new Vector3(1, 1, 1);
        textBox.GetComponent<Text>().text = Text;
        nameBox.GetComponent<Text>().text = Name.ToUpper();

        // Texture2D texture2D = LoadPNG("profiles/" + Name + "_profile.png");
        Sprite newSprite = Resources.Load<Sprite>("profiles/" + Name + "_profile");
        imageBox.GetComponent<Image>().sprite = newSprite;
    }

    public override void Stop()
    {
        dialogBox.transform.localScale = new Vector3(0, 0, 0);
    }

}
