using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    public Sound[] music, effects;
    public AudioSource musicSource, effectsSource, pullingEffectsSource;
    // separate AudioSource (pullingSrc) to cancel one specific sound ("ElasticPulling")

    void Awake() {
        if (soundManager == null) {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    void Start() {
        PlayMusic("Thrash");
    }

    public void PlayMusic(string name) {
        Sound sound = Array.Find(music, x => x.name == name);
        if (sound == null) {
            Debug.LogFormat("Sound (Music) {} could not be found", sound);
            return;
        }
        musicSource.clip = sound.clip;
        musicSource.Play();
    }

    public void PlayEffect(string name) {
        Sound sound = Array.Find(effects, x => x.name == name);
        if (sound == null) {
            Debug.LogFormat("Sound (Effect) {} could not be found", sound);
            return;
        }
        if (name.Equals("ElasticPulling")) {
            pullingEffectsSource.PlayOneShot(sound.clip);
        } else {
            effectsSource.PlayOneShot(sound.clip);
        }
    }

    public void ToggleMusic() {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleEffects() {
        effectsSource.mute = !effectsSource.mute;
        pullingEffectsSource.mute = !pullingEffectsSource.mute;
    }

    public void MusicVolume(float volume) {
        musicSource.volume = volume;
    }

    public void EffectsVolume(float volume) {
        effectsSource.volume = volume;
        pullingEffectsSource.volume = volume;
    }
}
