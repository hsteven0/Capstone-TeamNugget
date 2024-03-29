using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    public static CircleTransition circleTransition;
    private Canvas canvas;
    private Image blackScreen;
    private Material mat;
    private float transitionTime;

    private static readonly int RADIUS = Shader.PropertyToID("_Radius");

    void Awake() {
        if (circleTransition == null) {
            circleTransition = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
        blackScreen = GetComponentInChildren<Image>();

        mat = blackScreen.material;
        mat.SetFloat(RADIUS, 1.0f);

        transitionTime = 1.0f;
        DrawBlackScreen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpeningScreen();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            ClosingScreen();
        }
    }

    public void OpeningScreen()
    {
        DrawBlackScreen();
        StartCoroutine(Transition(transitionTime, 0, 1));
    }

    public void ClosingScreen()
    {
        DrawBlackScreen();
        StartCoroutine(Transition(transitionTime, 1, 0));
    }

    public void ClosingScreen(string sceneName)
    {
        DrawBlackScreen();
        StartCoroutine(Transition(transitionTime, 1, 0,sceneName));
    }

    private void DrawBlackScreen()
    {
        var canvasRect = canvas.GetComponent<RectTransform>().rect;
        var canvasWidth = canvasRect.width;

        blackScreen.rectTransform.sizeDelta = new Vector2(canvasWidth, canvasWidth);
    }

    private IEnumerator Transition(float duration, float beginRadius, float endRadius)
    {
        var time = 0f;
        while (time <= duration)
        {
            time += Time.deltaTime;
            var t = time / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            mat.SetFloat(RADIUS, radius);

            yield return null;
        }
    }

    private IEnumerator Transition(float duration, float beginRadius, float endRadius, string sceneName)
    {
        var time = 0f;
        while (time <= duration)
        {
            time += Time.deltaTime;
            var t = time / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            mat.SetFloat(RADIUS, radius);

            yield return null;
        }
        SceneManager.LoadScene(sceneName);
        OpeningScreen();
    }
}
