using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator anim;
    private bool optionsOpen, creditsOpen;
    private float timer, closeTimer;

    void Start()
    {
        // timer will keep incrementing until we get to 45s right when it fully opens.
        // so keep it relatively high so user has time to change sound options, or read the credited names
        closeTimer = 45.0f;
        timer = 0f;

        optionsOpen = false;
        creditsOpen = false;
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (optionsOpen || creditsOpen) {
            timer += Time.deltaTime;
            if (timer >= closeTimer) {
                if (optionsOpen) CloseOptions();
                else if (creditsOpen) CloseCredits();
            }
        } else {
            timer = 0f;
        }
    }

    public void DisplayOptions() {
        if (creditsOpen) CloseCredits();
        if (anim != null && !optionsOpen) {
            SoundManager.soundManager.PlayEffect("ButtonClick");
            anim.SetTrigger("OptionsPressed");
            optionsOpen = true;
        }
    }

    public void CloseOptions() {
        if (anim != null && optionsOpen) {
            SoundManager.soundManager.PlayEffect("ButtonClick");
            anim.Play("Base Layer.Return", 0, 0);
            optionsOpen = false;
        }
    }

    public void DisplayCredits() {
        if (optionsOpen) CloseOptions();
        if (anim != null && !creditsOpen) {
            SoundManager.soundManager.PlayEffect("ButtonClick");
            anim.SetTrigger("CreditsPressed");
            creditsOpen = true;
        }
    }

    public void CloseCredits() {
        if (anim != null && creditsOpen) {
            SoundManager.soundManager.PlayEffect("ButtonClick");
            anim.Play("Base Layer.Credits Return", 0, 0);
            creditsOpen = false;
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
