using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace RGSK
{
    public class AudioSettings : MonoBehaviour
    {
        public Slider masterVolumeSlider;
        public Slider sfxVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider engineVolumeSlider;
        private AudioMixer mixer;

        void Start()
        {
            mixer = GlobalSettings.Instance.gameAudioMixer;

            // Устанавливаем начальные значения
            UpdateUIToMatchSettings();

            // Сразу применяем сохранённые настройки
            ApplySavedVolumes();

            AddListeners();
        }

        void AddListeners()
        {
            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.onValueChanged.AddListener(delegate
                {
                    SetMasterVolume(masterVolumeSlider.value);
                });
            }

            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.onValueChanged.AddListener(delegate
                {
                    SetSFXVolume(sfxVolumeSlider.value);
                });
            }

            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.onValueChanged.AddListener(delegate
                {
                    SetMusicVolume(musicVolumeSlider.value);
                });
            }

            if (engineVolumeSlider != null)
            {
                engineVolumeSlider.onValueChanged.AddListener(delegate
                {
                    SetEngineVolume(engineVolumeSlider.value);
                });
            }
        }

        void UpdateUIToMatchSettings()
        {
            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
            }

            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            }

            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            }

            if (engineVolumeSlider != null)
            {
                engineVolumeSlider.value = PlayerPrefs.GetFloat("EngineVolume", 1);
            }
        }

        void ApplySavedVolumes()
        {
            SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.75f));
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
            SetEngineVolume(PlayerPrefs.GetFloat("EngineVolume", 1));
        }

        public void SetMasterVolume(float volume)
        {
            if (mixer != null)
            {
                mixer.SetFloat("Master_Volume", Helper.LinearToDecibel(volume));
            }

            PlayerPrefs.SetFloat("MasterVolume", volume);
        }

        public void SetSFXVolume(float volume)
        {
            if (mixer != null)
            {
                mixer.SetFloat("SFX_Volume", Helper.LinearToDecibel(volume));
            }

            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            if (mixer != null)
            {
                mixer.SetFloat("Music_Volume", Helper.LinearToDecibel(volume));
            }

            PlayerPrefs.SetFloat("MusicVolume", volume);

            // Если у вас есть `MusicPlayer`, обновляем громкость напрямую
            MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
            if (musicPlayer != null)
            {
                musicPlayer.SetVolume(volume);
            }
        }

        public void SetEngineVolume(float volume)
        {
            PlayerPrefs.SetFloat("EngineVolume", volume);

            // Если используется RealisticEngineSound_mobile, применяем громкость
            RealisticEngineSound_mobile engineSound = FindObjectOfType<RealisticEngineSound_mobile>();
            if (engineSound != null)
            {
                engineSound.masterVolume = volume;
            }
        }
    }
}
