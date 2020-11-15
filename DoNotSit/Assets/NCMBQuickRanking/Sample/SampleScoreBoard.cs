using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SampleScoreBoard : MonoBehaviour
{
    public Text scoreBoardText;
    public Text playerCounter;

    private void Start()
    {
        scoreBoardText.text = QuickRanking.Instance.GetRankingByText();

        QuickRanking.Instance.FetchPlayerCount(SetPlayerCounter);
    }

    void SetPlayerCounter()
    {
        playerCounter.text = "Total Player Count: "+ QuickRanking.Instance.PlayerCount.ToString();
    }

    //Button//
    public void BackToMainGame()
    {
        SceneManager.LoadScene("SampleGame");
    }
}