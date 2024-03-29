using UnityEngine;

public class IdleChecker : MonoBehaviour
{
    private float timer, idleTime;
    public static bool slingshotActive = false;

    void Start()
    {
        timer = 0;
        idleTime = 55.0f;
    }

    void Update()
    {
        if (slingshotActive && gameObject.name.Contains("Sling")) return;
        
        timer += Time.deltaTime;
        if (timer >= idleTime) {
            CircleTransition.circleTransition.ClosingScreen("MainMenuScene");
        }
    }
}
