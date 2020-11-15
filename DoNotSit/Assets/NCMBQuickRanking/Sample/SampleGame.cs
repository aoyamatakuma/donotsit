using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SampleGame : MonoBehaviour
{
    public InputField playerName;
    public Text scoreText;
    private int score = 0;
    public Button saveScoreButton;
    public Button showRankingButton;

    public Text message;

    private void Start()
    {

        //NCMB SDK はWebGLに対応していないため、WebGLビルドしたときにはランキング機能をオフにしてください//
#if UNITY_WEBGL
        showRankingButton.interactable = false;
        saveScoreButton.interactable = false;
#else
        showRankingButton.interactable = false;
        saveScoreButton.interactable = true;
#endif

    }

    public void Tap()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void ShowRanking()
    {
        showRankingButton.interactable = false;//連打防止//
        SceneManager.LoadScene("SampleScoreBoard");
    }

    public void SavePlayerScore()
    {
        saveScoreButton.interactable = false;//連打防止//
        QuickRanking.Instance.SaveRanking(playerName.text, score, SetEnableSHowRankingButton);
    }

    void SetEnableSHowRankingButton()
    {
        //自分のスコアを保存すると、ShowRankingButtonが押せるようになる//
        showRankingButton.interactable = true;
        message.text = "Your score saved!";
    }
}