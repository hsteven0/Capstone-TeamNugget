using UnityEngine;

public class IdleChecker : MonoBehaviour
{
    private float timer, idleTime;
    private bool isTransitioning;
    public static bool slingshotActive = false;

    void Start()
    {
        isTransitioning = false;
        timer = 0;
        idleTime = 55.0f;
    }

    void Update()
    {
        if (slingshotActive && gameObject.name.Contains("Sling")) return;
        
        timer += Time.deltaTime;
        if (timer >= idleTime && !isTransitioning) {
            isTransitioning = true;
            CircleTransition.circleTransition.ClosingScreen("MainMenuScene");
        }
    }
}
