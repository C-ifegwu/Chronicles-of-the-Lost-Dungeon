using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    [Header("Destination")]
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entering Portal... Loading " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}