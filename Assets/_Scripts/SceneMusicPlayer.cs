using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    [Header("Background Music")]
    [Tooltip("The music track you want to loop in this specific scene.")]
    public AudioClip sceneMusic;

    private void Start()
    {
        // Wait exactly 0.1 seconds to guarantee the SoundManager is fully built and ready
        Invoke("PlayMusic", 0.1f);
    }

    private void PlayMusic()
    {
        if (sceneMusic == null) return;

        // Find the SoundManager object directly in the scene by its name (used by the
        // Main Menu's Bloodlines UI sound system).
        GameObject managerObject = GameObject.Find("SoundManager");
        
        if (managerObject != null)
        {
            // Bypass the namespace wall and trigger the PlayBGM function directly
            managerObject.SendMessage("PlayBGM", sceneMusic, SendMessageOptions.DontRequireReceiver);
        }
        // --- UPDATED: Gameplay levels don't have a "SoundManager" object, so fall back
        // to the game's own AudioManager instead of silently failing.
        else if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM(sceneMusic);
        }
        else
        {
            Debug.LogWarning("SceneMusicPlayer could not find a SoundManager or AudioManager in the scene!");
        }
    }
}