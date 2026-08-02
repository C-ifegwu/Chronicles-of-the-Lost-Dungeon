using UnityEngine;
using UnityEngine.UI;

namespace BloodlinesUI
{
    /// <summary>
    /// Sound manager for controlling audio settings with separated Master, BGM, and SFX channels.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Instance = null;
        }

        [Header("Audio Components")]
        [SerializeField] private AudioSource audioSource; // Used for UI SFX
        [SerializeField] private AudioSource bgmSource;   // Used for Background Music
        
        [Header("UI Controls")]
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        [Header("Default Values")]
        [SerializeField] private float defaultMasterVolume = 1.0f;
        [SerializeField] private float defaultBgmVolume = 0.8f;
        [SerializeField] private float defaultSfxVolume = 0.8f;
        [SerializeField] private bool defaultSoundEnabled = true;
        [SerializeField] private float defaultHoverScale = 0.5f;
        [SerializeField] private float defaultClickScale = 0.7f;
        
        private bool isSoundEnabled = true;
        private float masterVolume = 1.0f;
        private float bgmVolume = 0.8f;
        private float sfxVolume = 0.8f;
        private float hoverScale = 0.5f;
        private float clickScale = 0.7f;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if (transform.parent == null)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            InitializeAudio();
            SetupUIControls();
            SetupAudioSource();
        }
        
        private void InitializeAudio()
        {
            isSoundEnabled = PlayerPrefs.GetInt("SoundEnabled", defaultSoundEnabled ? 1 : 0) == 1;
            
            // Load individual volumes from memory
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume", defaultBgmVolume);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", defaultSfxVolume);
            
            hoverScale = PlayerPrefs.GetFloat("HoverScale", defaultHoverScale);
            clickScale = PlayerPrefs.GetFloat("ClickScale", defaultClickScale);
            
            ApplyAudioSettings();
        }
        
        private void SetupUIControls()
        {
            if (soundToggle != null)
            {
                soundToggle.isOn = isSoundEnabled;
                soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
            }
            
            // Hook up the 3 UI Sliders
            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = masterVolume;
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            }
            
            if (bgmVolumeSlider != null)
            {
                bgmVolumeSlider.value = bgmVolume;
                bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
            }
            
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = sfxVolume;
                sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            }
        }
        
        private void SetupAudioSource()
        {
            // Setup SFX Source
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;

            // Setup BGM Source
            if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
            bgmSource.loop = true; 
        }
        
        public void OnSoundToggleChanged(bool isEnabled)
        {
            isSoundEnabled = isEnabled;
            PlayerPrefs.SetInt("SoundEnabled", isEnabled ? 1 : 0);
            PlayerPrefs.Save();
            
            ApplyAudioSettings();
        }
        
        // --- Independent Volume Slider Handlers ---
        public void OnMasterVolumeChanged(float volume)
        {
            masterVolume = volume;
            PlayerPrefs.SetFloat("MasterVolume", volume);
            PlayerPrefs.Save();
            ApplyAudioSettings();
        }

        public void OnBgmVolumeChanged(float volume)
        {
            bgmVolume = volume;
            PlayerPrefs.SetFloat("BGMVolume", volume);
            PlayerPrefs.Save();
            ApplyAudioSettings();
        }

        public void OnSfxVolumeChanged(float volume)
        {
            sfxVolume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
            PlayerPrefs.Save();
            ApplyAudioSettings();
        }
        
        private void ApplyAudioSettings()
        {
            // SFX is a combination of Master Volume and SFX Volume
            if (audioSource != null)
            {
                audioSource.volume = isSoundEnabled ? (masterVolume * sfxVolume) : 0f;
            }
            
            // BGM is a combination of Master Volume and BGM Volume
            if (bgmSource != null)
            {
                bgmSource.volume = isSoundEnabled ? (masterVolume * bgmVolume) : 0f;
            }
        }

        public void PlayBGM(AudioClip musicClip)
        {
            if (bgmSource != null && musicClip != null)
            {
                if (bgmSource.clip == musicClip && bgmSource.isPlaying) return; 
                
                bgmSource.clip = musicClip;
                bgmSource.Play();
            }
        }
        
        public void PlaySound(AudioClip clip, float volume = 1f)
        {
            if (audioSource != null && clip != null && isSoundEnabled)
            {
                audioSource.PlayOneShot(clip, volume * masterVolume * sfxVolume);
            }
        }
        
        public void PlayHoverSound(AudioClip clip)
        {
            if (audioSource != null && clip != null && isSoundEnabled)
            {
                audioSource.PlayOneShot(clip, masterVolume * sfxVolume * hoverScale);
            }
        }
        
        public void PlayClickSound(AudioClip clip)
        {
            if (audioSource != null && clip != null && isSoundEnabled)
            {
                audioSource.PlayOneShot(clip, masterVolume * sfxVolume * clickScale);
            }
        }
        
        public void SetSoundEnabled(bool enabled)
        {
            if (soundToggle != null) soundToggle.isOn = enabled;
            else OnSoundToggleChanged(enabled);
        }
        
        public void SetHoverScale(float scale)
        {
            hoverScale = Mathf.Clamp01(scale);
            PlayerPrefs.SetFloat("HoverScale", hoverScale);
            PlayerPrefs.Save();
        }
        
        public void SetClickScale(float scale)
        {
            clickScale = Mathf.Clamp01(scale);
            PlayerPrefs.SetFloat("ClickScale", clickScale);
            PlayerPrefs.Save();
        }
        
        public bool IsSoundEnabled() { return isSoundEnabled; }
        public float GetMasterVolume() { return masterVolume; }
        public float GetBgmVolume() { return bgmVolume; }
        public float GetSfxVolume() { return sfxVolume; }
        public float GetHoverScale() { return hoverScale; }
        public float GetClickScale() { return clickScale; }
        
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
            if (soundToggle != null) soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
            if (masterVolumeSlider != null) masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            if (bgmVolumeSlider != null) bgmVolumeSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);
            if (sfxVolumeSlider != null) sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        }
    }
}