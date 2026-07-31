using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject loadingScreen; 
    public RectTransform loadingIcon; // The runic motif to rotate
    public float rotationSpeed = -200f; // Negative spins clockwise

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // This is the magic line that stops the Canvas from being destroyed during a scene change
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    private void Update()
    {
        // Rotate the runic icon every frame while the screen is active
        if (loadingScreen != null && loadingScreen.activeSelf && loadingIcon != null)
        {
            loadingIcon.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    // Call this from your portals or menu buttons!
    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);

        // Load the scene asynchronously in the background so the game doesn't freeze
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until Unity completely finishes loading the next level
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Keep the screen black for just a tiny moment longer to ensure textures load
        yield return new WaitForSeconds(0.5f);

        if (loadingScreen != null) loadingScreen.SetActive(false);
    }
}