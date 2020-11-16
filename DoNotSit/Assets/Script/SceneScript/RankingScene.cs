using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScene : MonoBehaviour
{
    public Text rankingText;

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
        rankingText.text = QuickRanking.Instance.GetRankingByText();
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
