using UnityEngine;

public class SwitchScenes : MonoBehaviour
{
    public void LoadScene(string sceneName) {
        SoundManager.soundManager.PlayEffect("ButtonClick");
        CircleTransition.circleTransition.ClosingScreen(sceneName);
    }
}
