using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider musicSlider, effectsSlider;

    public void ToggleMusic() {
        SoundManager.soundManager.ToggleMusic();
    }

    public void ToggleEffects() {
        SoundManager.soundManager.ToggleEffects();
    }

    public void MusicVolume() {
        SoundManager.soundManager.MusicVolume(musicSlider.value);
    }

    public void EffectsVolume() {
        SoundManager.soundManager.EffectsVolume(effectsSlider.value);
    }
}
