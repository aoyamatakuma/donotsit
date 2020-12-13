using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearScene : MonoBehaviour
{
    public GameObject stageSelect;
    public GameObject title;
    public GameObject easy;
    public GameObject normal;
    public GameObject hard;
    public GameObject ex;
    public GameObject easyBack;
    public GameObject normalBack;
    public GameObject hardBack;
    public GameObject exBack;
    public InputField inputField;
    public Text nameText;
    private bool select;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    public Text goalTimeText;
    public Text goalScoreText;
    public Text rankText;
    public Text stageText;
    public float maxValue;
    float goaltimer;
    float goalscore;
    string stageName;
    string rankingName;
    public string rank;
    bool isPush;
    float preValue;
    Fade fade;

    // Start is called before the first frame update
    void Start()
    {
        isPush = false;
        audio = GetComponent<AudioSource>();
        select = true;
        goaltimer = PlayerControl.TimeScore();
        goalscore = PlayerControl.ClearScore();
        rank = RankManger.ClearRank();
        //goalTimeText.text = "Time:"+ goaltimer.ToString("f0") + "秒";//ゴールタイム
        goalScoreText.text = "Score:" + goalscore.ToString();//ゴールスコア
        fade = GetComponent<Fade>();
        stageName = StageDate.Instance.referer;
       // stageText.text =   stageName.ToString();
       
        // Type == Number の場合

    }

    // Update is called once per frame
    void Update()
    {
        float value;
        if(float.TryParse(inputField.text,out value))
        {
            if(value > maxValue)
            {
                inputField.text = preValue + "";
            }
            else
            {
                preValue = value;
            }
        }
        Name();
        Select();
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            if (select)
            {
                StageDate.SetBool(StageDate.clearKey, true);
                StageClearBool();
                QuickRanking.Instance.SaveRanking(nameText.text, stageName, (int)goalscore);
                fade.StartFadeIn(rankingName, true);
            }
            else
            {
                StageDate.SetBool(StageDate.clearKey, true);
                StageClearBool();
                QuickRanking.Instance.SaveRanking(nameText.text, stageName, (int)goalscore);
                fade.StartFadeIn("Title", true);
            }
        }
    }

    void Select()
    {
        float ver = Input.GetAxis("Vertical");

        if (select)
        {

            stageSelect.GetComponent<Outline>().enabled = true;
            title.GetComponent<Outline>().enabled = false;
        }
        else
        {

            stageSelect.GetComponent<Outline>().enabled = false;
            title.GetComponent<Outline>().enabled = true;
        }

        if (ver > 0.5f)
        {
            if (!select)
            {
                audio.PlayOneShot(moveSE);
            }
            select = true;

        }
        else if (ver < -0.5f)
        {
            if (select)
            {
                audio.PlayOneShot(moveSE);
            }
            select = false;
        }

    }
    void Name()
    {
        if (stageName == "StageEasy")
        {
            easy.SetActive(true);
            easyBack.SetActive(true);
        }
        else if (stageName == "StageNormal")
        {
            normal.SetActive(true);
            normalBack.SetActive(true);
        }
        else if (stageName == "StageHard")
        {
            hard.SetActive(true);
            hardBack.SetActive(true);
        }
        else
        {
            ex.SetActive(true);
            exBack.SetActive(true);
        }
    }
    void StageClearBool()
    {
        StageDate.Instance.SetSceneName(stageName);
        if (stageName == "StageEasy")
        {
            StageDate.SetBool(StageDate.easyKey, true);
            rankingName = "RankingEasy";
            
        }
        else if(stageName == "StageNormal")
        {
            StageDate.SetBool(StageDate.normalKey, true);
            rankingName = "RankingNormal";
        }
        else if(stageName == "StageHard")
        {
            StageDate.SetBool(StageDate.hardKey, true);
            rankingName = "RankingHard";
        }
        else
        {
            StageDate.SetBool(StageDate.extraKey, true);
            rankingName = "RankingExtra";
        }

      
    }





}
