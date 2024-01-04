using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThinkingBubble : MonoBehaviour
{
    private GameObject thinkingBubble;
    private TMP_Text bubbleText;
    private float fadeInDuration = 1f;
    private float fadeOutDuration = 0.5f;
    private CanvasGroup canvasGroup;

    public void Awake()
    {
        thinkingBubble = GameObject.Find("ThinkingBubble");
        bubbleText = GameObject.Find("BubbleText").GetComponent<TMP_Text>();

        if(thinkingBubble != null)
        {
            canvasGroup = thinkingBubble.GetComponent<CanvasGroup>();

            if(canvasGroup == null)
            {
                canvasGroup = thinkingBubble.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
        }

        thinkingBubble.SetActive(false);
    }

    private System.Collections.IEnumerator FadeIn()
    {
        if(canvasGroup != null)
        {
            float elapsedTime = 0f;
            thinkingBubble.SetActive(true);
            while (elapsedTime < fadeInDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

    }

    private System.Collections.IEnumerator FadeOut()
    {
        if (canvasGroup != null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeOutDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            thinkingBubble.SetActive(false);
            canvasGroup.alpha = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerObject"))
        {
            StartCoroutine(FadeIn());
            bubbleText.text = "\nThese seem\nto be the baddies from that\ngame that Bruno is always playing.\nI think he calls them orcs and\n beholders.\n\nWe'll see who is the orc\n and beholder now!";
        }
        if (other.CompareTag("CorridorRoomTrigger"))
        {
            StartCoroutine(FadeIn());
            bubbleText.text = "\nThis looks like Lisas\nroom.\nI wonder why its in disarray...\n\nShes the only one who is nice to me...\n\nI sure hope nothing bad has happened\nto her";
        }
        if (other.CompareTag("ChestTrigger"))
        {
            StartCoroutine(FadeIn());
            bubbleText.text = "\nOooh! A shiny box!\nMom always gets mad at me when\nI try to eat the tasty stuff in these\n\nWell shes not here to\nstop me anymore!";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(FadeOut());
    }
}
