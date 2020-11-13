using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearScene : MonoBehaviour
{
    public GameObject stageSelect;
    public GameObject title;
    public GameObject rankS;
    public GameObject rankA;
    public GameObject rankB;
    public GameObject rankC;
    private bool select;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    public Text goalTimeText;
    public Text goalScoreText;
    public Text rankText;
    float goaltimer;
    float goalscore;
    public float SA;
    public float B;
    public float C;
    public  string rank;
    bool isPush;
    Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        isPush = false;
        audio = GetComponent<AudioSource>();
        select = true;
        goaltimer = PlayerControl.TimeScore();
        goalscore = PlayerControl.ClearScore();
        //goalTimeText.text = "Time:"+ goaltimer.ToString("f0") + "秒";//ゴールタイム
        goalScoreText.text = "Score:" + goalscore.ToString();//ゴールスコア
        fade = GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        Rank();
        Select();
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            if (select)
            {

                fade.StartFadeIn("StageSelect", true);
            }
            else
            {
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
    void Rank()
    {
        if(goalscore> SA)//Sランク
        {
            rank = "S";
            if (rank == "S")
            {
                rankS.SetActive(true);
                rankA.SetActive(false);
                rankB.SetActive(false);
                rankC.SetActive(false);
            }
        }
        if (goalscore <= SA)//Aランク
        {
            rank = "A";
            if (rank == "A")
            {
                rankS.SetActive(false);
                rankA.SetActive(true);
                rankB.SetActive(false);
                rankC.SetActive(false);
            }
        }
        if (goalscore <= B)//Bランク
        {
            rank = "B";
            if (rank == "B")
            {
                rankS.SetActive(false);
                rankA.SetActive(false);
                rankB.SetActive(true);
                rankC.SetActive(false);
            }
        }
        if (goalscore <= C)//Cランク
        {
            rank = "C";
            if (rank == "C")
            {
                rankS.SetActive(false);
                rankA.SetActive(false);
                rankB.SetActive(false);
                rankC.SetActive(true);
            }
        }
    }

}
