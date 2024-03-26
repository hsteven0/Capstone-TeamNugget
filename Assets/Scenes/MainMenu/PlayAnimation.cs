using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator anim;
    private bool isOpen;

    void Start()
    {
        isOpen = false;
        anim = GetComponent<Animator>();
    }

    public void DisplayOptions() {
        if (anim != null && !isOpen) {
            SoundManager.soundManager.PlayEffect("ButtonClick");
            anim.SetTrigger("OptionsPressed");
            isOpen = true;
        }
    }

    public void CloseOptions() {
        if (anim != null && isOpen) {
            SoundManager.soundManager.PlayEffect("ButtonClick");
            anim.Play("Base Layer.Return", 0, 0);
            isOpen = false;
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
