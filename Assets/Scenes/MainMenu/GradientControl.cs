using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
This class controls the gradient of all TextMeshProUI's in the Canvas GameObject
using Lerp(). The colors fade up and down, giving a "ping pong" effect to it.
*/
public class GradientControl : MonoBehaviour
{
    private Color colorTop;
    private Color colorBottom;
    private float fadeDuration = 1.2f, fadeStart = 0.0f;
    private bool reachedBottom;
    private float timer, hueTimer = 0.1F, fadingHue = 1F;
    private bool hueDarkening;
    private TextMeshProUGUI[] textComponents;

    void Start() {
        textComponents = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        colorTop = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        colorBottom = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        reachedBottom = false;
        timer = 0.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        // slowly reduce values over time
        // if (fadeStart < fadeDuration) {
        //     fadeStart += Time.deltaTime / fadeDuration;
        //     foreach (TextMeshProUGUI text in textComponents) {
        //         if (text.GetComponentInParent<Button>()) continue;
        //         // color calculations
        //         var colTop = Color.Lerp(colorTop, colorBottom, fadeStart);
        //         var colBot = Color.Lerp(colorBottom, colorTop, fadeStart);
                
        //         if (!reachedBottom) {
        //             text.colorGradient = new VertexGradient(colTop, colTop, colBot, colBot);
        //         } else {
        //             text.colorGradient = new VertexGradient(colBot, colBot, colTop, colTop);
        //         }
        //     }
        // } else {
        //     reachedBottom = !reachedBottom;
        //     fadeStart = 0;
        // }
        if (timer >= hueTimer) {
            if (!hueDarkening) {
                if (fadingHue > 0.6) {
                    fadingHue -= 0.1F;
                } else {
                    hueDarkening = true;
                }
            } else {
                if (fadingHue < 1) {
                    fadingHue += 0.1F;
                } else {
                    hueDarkening = false;
                }
            }
            timer -= hueTimer;
        }
        foreach (TextMeshProUGUI text in textComponents) {
            if (text.GetComponentInParent<Button>()) continue;
            text.faceColor = FadingBlue();
        }
    }

    private Color FadingBlue() {
        return Color.HSVToRGB(0.6F, fadingHue, 0.82f);
    }
}
