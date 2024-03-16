using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator anim;
    private bool shouldDisplay;

    void Start()
    {
        anim = GetComponent<Animator>();
        shouldDisplay = gameObject.activeInHierarchy;
    }

    void Update()
    {
        // Check if closing animation is playing
        if (IsAnimating("Base Layer.End")) {
            // set shouldDisplay to false. need to turn off SoundPanel after animation ends
            if (shouldDisplay)
                shouldDisplay = false;
        } else if (!shouldDisplay)
            // animation is done, so now we can turn off the SoundPanel
            gameObject.SetActive(false);
    }

    public void DisplayOptions() {
        shouldDisplay = true;
        gameObject.SetActive(true);
    }

    public void CloseOptions() {
        if (anim != null) {
            anim.Play("Base Layer.End", 0, 0);
        }
    }

    private bool IsAnimating() {
        // returns true if any animation playing
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }

    private bool IsAnimating(string animName) {
        // returns true if a specific animation (by name) is playing
        return IsAnimating() && anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
}
