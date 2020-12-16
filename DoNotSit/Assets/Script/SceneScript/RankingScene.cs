using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingScene : MonoBehaviour
{
    public Text rankingText;
    public Text nameText;
    public Text scoreText;
    public Text myRankText;

    public GameObject stageSelect;
    public GameObject title;
    public static int sceneNum;
    private bool select;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    bool isPush;
    Fade fade;
    int rank;
    // Start is called before the first frame update
    void Start()
    {
        rank = 1;
        isPush = false;
        select = true;
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
        QuickRanking.Instance.FetchRanking();
        foreach (var list in QuickRanking.Instance.GetRanking())
        {
            if (list.stageNumber == StageDate.Instance.rankingSceneName)
            {
                if (list.name != null)
                {
                    if(list.objectid == QuickRanking.Instance.GetCurrentID())
                    {
                        nameText.text += "<color=red>" + "\t" + list.name + "</color> \n";
                    }
                    else
                    {
                        nameText.text += "\t" + list.name + "\n";
                    }
                   
                }
                else
                {
                    nameText.text += "\t"+"NoData"+"\n";
                }
                if (list.rankNum.ToString() != null)
                {
                    if(rank < 10)
                    {
                        if (list.objectid == QuickRanking.Instance.GetCurrentID())
                        {
                            rankingText.text += "<color=red>" + "\t" + rank + "</color> \n";
                        }
                        else
                        {
                            rankingText.text += "\t" + rank + "\n";
                        }
                        rank += 1;
                    }
                }
                else
                {
                    rankingText.text += "\t" + "NoData" + "\n";
                }
                if (list.score.ToString() != null)
                {
                    if (list.objectid == QuickRanking.Instance.GetCurrentID())
                    {
                        scoreText.text += "<color=red>" + "\t" + list.score.ToString() + "</color> \n";
                    }
                    else
                    {
                        scoreText.text += "\t" + list.score.ToString() + "\n";
                    }                   
                }
                else
                {
                    scoreText.text += "\t" + "NoData" + "\n";
                }
            }

            if (list.objectid == QuickRanking.Instance.GetCurrentID())
            {
               myRankText.text =  list.name  + ": \t" + list.score.ToString();
            }
          
        }
    }

    // Update is called once per frame
    void Update()
    {
        Select();
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            if (select)
            {
                if(sceneNum == 0)
                {
                    fade.StartFadeIn("StageSelect", true);
                }
                else
                {
                    fade.StartFadeIn("RankingSelect", true);
                }
                
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
}
