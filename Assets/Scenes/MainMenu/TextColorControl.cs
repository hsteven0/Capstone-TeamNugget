using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
This class controls the color of all TextMeshProUI's in the Canvas GameObject
*/
public class TextColorControl : MonoBehaviour
{
    private float timer, fadingHue = 1F;
    private bool hueDarkening;
    private TextMeshProUGUI[] textComponents;

    void Start() {
        textComponents = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        timer = 0.0f;
        StartCoroutine(ColorFade(1.4f));
    }

    void Update()
    {
        foreach (TextMeshProUGUI text in textComponents) {
            if (text.GetComponentInParent<Button>() || text.name.Equals("Ignore")) continue;
            text.color = FadingBlue();
        }
    }

    private Color FadingBlue() {
        return Color.HSVToRGB(0.6F, fadingHue, 0.82f);
    }

    // better implementation of smoothly updating values over time
    private IEnumerator ColorFade(float duration)
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (timer <= duration) {
                if (!hueDarkening) {
                    if (fadingHue > 0.6) {
                        fadingHue -= 0.005F;
                    } else {
                        hueDarkening = true;
                    }
                } else {
                    if (fadingHue < 1) {
                        fadingHue += 0.005F;
                    } else {
                        hueDarkening = false;
                    }
                }
                timer -= duration;
            }
            yield return null;
        }
    }
}
