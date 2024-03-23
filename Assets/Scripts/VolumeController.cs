using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider musicSlider, effectsSlider;
    public Button musicButton, effectsButton; // a reference to the toggle buttons
    public Sprite disabledMusic, disabledEffects, enabledMusic, enabledEffects;

    void Awake() {
        UpdateButtonSprites();
    }

    private void UpdateButtonSprites() {
        musicButton.image.sprite = SoundManager.soundManager.musicSource.mute ? disabledMusic : enabledMusic;
        effectsButton.image.sprite = SoundManager.soundManager.effectsSource.mute ? disabledEffects : enabledEffects;
    }

    public void ToggleMusic() {
        SoundManager.soundManager.ToggleMusic();
        UpdateButtonSprites();
    }

    public void ToggleEffects() {
        SoundManager.soundManager.ToggleEffects();
        UpdateButtonSprites();
    }

    public void MusicVolume() {
        SoundManager.soundManager.MusicVolume(musicSlider.value);
    }

    public void EffectsVolume() {
        SoundManager.soundManager.EffectsVolume(effectsSlider.value);
    }
}
