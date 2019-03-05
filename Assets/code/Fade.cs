using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Image image = GetComponent<Image>();

        image.color = Color.clear;
    }

    public void FadeOut()
    {
		transform.localScale = new Vector3(1, 1, 1);

        // Image image = GetComponent<Image>();
        // image.color = Color.black;

        // StartCoroutine(DoFadeOut());
    }

    public void FadeIn()
    {
		transform.localScale = new Vector3(0, 0, 0);
        // Image image = GetComponent<Image>();
		// image.color = Color.clear;
        // image.color = Color.black;

        // StartCoroutine(DoFadeIn());
    }

    // volatile bool fadeAccessable = true;

    IEnumerator DoFadeOut()
    {

        // while (!fadeAccessable) ;
		// fadeAccessable = false;

        Image image = GetComponent<Image>();
        // image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        while (image.color.a < 1)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + Time.deltaTime / 2);
            yield return null;
        }

        Main main = GameObject.Find("GameManager").GetComponent<Main>();
        main.buttonEnabled = false;

        // fadeAccessable = true;

        yield return null;
    }

    IEnumerator DoFadeIn()
    {
        // while (!fadeAccessable) ;
        // fadeAccessable = false;

        Image image = GetComponent<Image>();
        // image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.deltaTime / 2);
            yield return null;
        }

        Main main = GameObject.Find("GameManager").GetComponent<Main>();
        main.buttonEnabled = true;

        // fadeAccessable = true;

        yield return null;
    }
}
