using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [Header("Dreamlo API Codes")]
    [Tooltip("Paste your 16-character public code from dreamlo.com here")]
    [SerializeField] private string publicCode = "YOUR_PUBLIC_CODE";
    [Tooltip("Paste your 16-character private code from dreamlo.com here")]
    [SerializeField] private string privateCode = "YOUR_PRIVATE_CODE";

    private string webURL = "http://dreamlo.com/lb/";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// REST API POST: Sends a player's score to the online leaderboard.
    /// </summary>
    public void SendScore(string playerName, int score)
    {
        StartCoroutine(UploadScoreRoutine(playerName, score));
    }

    private IEnumerator UploadScoreRoutine(string playerName, int score)
    {
        string url = webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(playerName) + "/" + score;
        
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to upload score to REST API: " + www.error);
            }
            else
            {
                Debug.Log("Score successfully posted to online leaderboard!");
            }
        }
    }

    /// <summary>
    /// REST API GET: Fetches the top scores from the online server.
    /// </summary>
    public void GetLeaderboard(System.Action<string> onScoreLoaded)
    {
        StartCoroutine(DownloadScoresRoutine(onScoreLoaded));
    }

    private IEnumerator DownloadScoresRoutine(System.Action<string> onScoreLoaded)
    {
        string url = webURL + publicCode + "/pipe/"; // 'pipe' format makes it easy to read

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download leaderboard from REST API: " + www.error);
                onScoreLoaded?.Invoke("Error loading leaderboard.");
            }
            else
            {
                // Returns the raw text data from the web server
                string textResult = www.downloadHandler.text;
                onScoreLoaded?.Invoke(textResult);
            }
        }
    }
}