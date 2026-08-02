using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class LeaderboardDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text leaderboardTextLabel;
    [SerializeField] private GameObject leaderboardPanel;

    [Header("Dreamlo API Codes")]
    [SerializeField] private string publicCode = "6a6d264b8f40bb1218917d30";
    [SerializeField] private string privateCode = "F7th_H81DEmOxbXqGzLd7AxCaMYo8bpE-O6ptQRgrLLw";
    
    private string webURL = "http://dreamlo.com/lb/";

    // Called when the player clicks the "Leaderboard" button in the Main Menu
    public void ToggleLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            bool isActive = leaderboardPanel.activeSelf;
            leaderboardPanel.SetActive(!isActive);

            if (!isActive)
            {
                FetchScores();
            }
        }
    }

    public void FetchScores()
    {
        if (leaderboardTextLabel != null)
        {
            leaderboardTextLabel.text = "Fetching scores from cloud...";
            StartCoroutine(DownloadScoresRoutine());
        }
    }

    private IEnumerator DownloadScoresRoutine()
    {
        string url = webURL + publicCode + "/pipe/";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                leaderboardTextLabel.text = "Error connecting to cloud leaderboard.";
                Debug.LogError("REST API GET Error: " + www.error);
            }
            else
            {
                string textResult = www.downloadHandler.text;
                leaderboardTextLabel.text = string.IsNullOrEmpty(textResult) ? "No scores recorded yet." : textResult;
            }
        }
    }

    // Optional utility method to post scores directly from anywhere
    public void SubmitScore(string playerName, int score)
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
                Debug.LogError("REST API POST Error: " + www.error);
            }
            else
            {
                Debug.Log("Score successfully posted to online leaderboard!");
            }
        }
    }
}