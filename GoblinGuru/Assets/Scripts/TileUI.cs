using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TileUI : MonoBehaviour {


    public UnityEngine.UI.Image image;
    public TextMeshProUGUI tileText;

    bool animating;

    public void UpdateUI(GameTile tile)
    {
        string text = Enum.GetName(typeof(TileTerrain), tile.tileTerrain);

        if (text == "Grass2")
        {
            text = "Grass";
        }
        if (text == "Mountain2")
        {
            text = "Mountain";
        }
        if (text == "Snow2")
        {
            text = "Snow";
        }

        if(tile.tileFeatures != 0)
        {
            text += ", " + Enum.GetName(typeof(TileFeatures), tile.tileFeatures);
        }

        tileText.text = text;
        StartCoroutine(AnimateFadeIn());
    }

    IEnumerator AnimateFadeIn()
    {
        if (!animating)
        {
            animating = true;
            Color oldImage = image.color;
            oldImage.a = 150/255f;
            Color oldText = tileText.color;
            oldText.a = 255;
            float speed = 1;

            float elapsedTime = 0;
            while (elapsedTime < 1)
            {
                image.color = Color.Lerp(image.color, oldImage, (elapsedTime / 1));
                tileText.color = Color.Lerp(tileText.color, oldText, (elapsedTime / 1));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            StartCoroutine(Hold());
        }
    }

    IEnumerator Hold()
    {
        float elapsedTime = 0;
        while (elapsedTime < 2)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(AnimateFadeOut());
    }

    IEnumerator AnimateFadeOut()
    {
        animating = true;
        Color oldImage = image.color;
        oldImage.a = 0;
        Color oldText = tileText.color;
        oldText.a = 0;
        float speed = 1;

        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            image.color = Color.Lerp(image.color, oldImage, (elapsedTime / 1));
            tileText.color = Color.Lerp(tileText.color, oldText, (elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        animating = false;
        
    }
}
