using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    public Sound[] music, effects;
    public AudioSource musicSource, effectsSource;

    void Awake() {
        if (soundManager == null) {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    void Start() {
        PlayMusic("Spongebob Guitar");
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
        effectsSource.PlayOneShot(sound.clip);
    }

    public void ToggleMusic() {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleEffects() {
        effectsSource.mute = !effectsSource.mute;
    }

    public void MusicVolume(float volume) {
        musicSource.volume = volume;
    }

    public void EffectsVolume(float volume) {
        effectsSource.volume = volume;
    }
}
