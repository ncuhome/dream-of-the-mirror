using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHint : MonoBehaviour
{
    public static TextHint instance;
    public TMP_Text hintT;

    private IEnumerator preEnumerator;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("The scene has another TextHint");
        }    
        instance = this;
    }

    public void ShowHint(string hint, Color color)
    {
        if (preEnumerator != null)
        {
            StopCoroutine(preEnumerator);
        }
        hintT.text = hint;
        hintT.color = color;
        preEnumerator = FadeOut();
        StartCoroutine(preEnumerator);
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        Color tColor = hintT.color;
        float fadeOutDuration = 0.5f;
        float fadeOutDone = Time.time + fadeOutDuration;
        while (Time.time < fadeOutDone)
        {
            tColor.a = (fadeOutDone - Time.time) / fadeOutDone;
            hintT.color = tColor;
            yield return null;
        }
        tColor.a = 0;
        hintT.color = tColor;
    }
}
