using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    void Start() {
        colorTop = new Color(0.3f, 0.3f, 1.0f, 1.0f);
        colorBottom = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        reachedBottom = false;
    }

    void Update()
    {
        var textComponents = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        // slowly reduce values over time
        if (fadeStart < fadeDuration) {
            fadeStart += Time.deltaTime / fadeDuration;
            foreach (TextMeshProUGUI text in textComponents) {
                if (text.name == "PlayText") continue;
                // color calculations
                var colTop = Color.Lerp(colorTop, colorBottom, fadeStart);
                var colBot = Color.Lerp(colorBottom, colorTop, fadeStart);
                
                if (!reachedBottom) {
                    text.colorGradient = new VertexGradient(colTop, colTop, colBot, colBot);
                } else {
                    text.colorGradient = new VertexGradient(colBot, colBot, colTop, colTop);
                }
            }
        } else {
            reachedBottom = !reachedBottom;
            fadeStart = 0;
        }
    }
}
