using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScene : MonoBehaviour
{
    public Text rankingText;
    public Text nameText;
    public Text scoreText;

    public GameObject stageSelect;
    public GameObject title;

    private bool select;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    bool isPush;
    Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        isPush = false;
        select = true;
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
        QuickRanking.Instance.FetchRanking();
        foreach(var list in QuickRanking.Instance.GetRanking())
        {
            if(list.name != null)
            {
                nameText.text += "\t"+list.name + "\n";
            }
            else
            {
                nameText.text += "\tNoData\n";
            }
            if (list.rankNum.ToString() != null)
            {
                rankingText.text += "\t" + list.rankNum + "\n";
            }
            else
            {
                rankingText.text += "\tNoData\n";
            }
            if (list.score.ToString() != null)
            {
                scoreText.text += "\t" + list.score.ToString() + "\n";
            }
            else
            {
                scoreText.text += "\tNoData\n";
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
}
