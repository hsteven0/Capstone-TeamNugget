using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void DisplayOptions() {
        if (anim != null) {
            anim.SetTrigger("OptionsPressed");
        }
    }

    public void CloseOptions() {
        if (anim != null) {
            anim.Play("Base Layer.Return", 0, 0);
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
