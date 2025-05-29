using UnityEngine;
using UnityEngine.Audio; // Provides access to Unity's audio system and mixer functionality
using UnityEngine.UI;    // Provides access to Unity's UI system (for Sliders)

public class VolumeManager : MonoBehaviour
{
    // Public references to be assigned in the Unity Inspector
    public AudioMixer mixer;           // Reference to the AudioMixer that controls audio routing
    public Slider musicSlider;         // UI Slider for controlling music volume
    public Slider environmentSlider;   // UI Slider for controlling sound effects/environment volume

    private void Awake()
    {
        // Add listeners to call volume control methods whenever sliders are moved
        musicSlider.onValueChanged.AddListener(ControlMusicVolume);
        environmentSlider.onValueChanged.AddListener(ControlEnvironmentVolume);
    }

    private void Start()
    {
        // Load saved volume settings when the game starts
        Load();
    }

    /// <summary>
    /// Controls the music volume based on slider value
    /// </summary>
    /// <param name="valor">Slider value between 0 and 1</param>
    private void ControlMusicVolume(float valor)
    {
        // Convert linear slider value to logarithmic decibel scale (audio perception is logarithmic)
        // Mathf.Log10(valor) * 20 converts 0-1 range to -80dB to 0dB (silence to full volume)
        // "MusicVolume" must match the exposed parameter name in the AudioMixer
        mixer.SetFloat("MusicVolume", Mathf.Log10(valor) * 20);

        // Save the current slider value to PlayerPrefs for persistence
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    /// <summary>
    /// Controls the environment/sound effects volume based on slider value
    /// </summary>
    /// <param name="valor">Slider value between 0 and 1</param>
    private void ControlEnvironmentVolume(float valor)
    {
        // Same conversion as music volume but for environment sounds
        mixer.SetFloat("EnvironmentVolume", Mathf.Log10(valor) * 20);

        // Save the current slider value to PlayerPrefs
        PlayerPrefs.SetFloat("EnvironmentVolume", environmentSlider.value);
    }

    /// <summary>
    /// Loads saved volume settings or uses defaults if none exist
    /// </summary>
    private void Load()
    {
        // Get saved values from PlayerPrefs or use 0.75 (75% volume) as default if no value exists
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        environmentSlider.value = PlayerPrefs.GetFloat("EnvironmentVolume", 0.75f);

        // Apply the loaded values to the AudioMixer
        ControlMusicVolume(musicSlider.value);
        ControlEnvironmentVolume(environmentSlider.value);
    }
}