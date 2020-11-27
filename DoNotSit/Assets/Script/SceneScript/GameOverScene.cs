﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    private bool isSelect;
    bool isMove;
    private int selectNum;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    bool isPush;
    Fade fade;
    public List<Image> images;
    string stageName;
    // Start is called before the first frame update
    void Start()
    {
        selectNum = 0;
        isPush = false;
        isSelect = true;
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
        stageName = StageDate.Instance.referer;
        images[selectNum].GetComponent<Outline>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        SelectMove();
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            if (selectNum == 0)
            {
                fade.StartCoroutine(stageName, true);
            }
            else if(selectNum ==1)
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
        if (isSelect)
        {
            return;
        }
        for (int i = 0; i < images.Count; i++)
        {
            images[i].GetComponent<Outline>().enabled = false;
        }
        images[selectNum].GetComponent<Outline>().enabled = true;
        isSelect = true;

    }



    void SelectMove()
    {
        if (isPush)
        {
            return;
        }
        float hol = Input.GetAxis("SelectVerMove");
        if (hol < -0.5f && !isMove)
        {
            selectNum--;
            StartCoroutine(ChangeCoroutine());
            if (selectNum < 0)
            {
                selectNum = 0;
            }
            isSelect = false;
        }

        if (hol > 0.5f && !isMove)
        {
            selectNum++;
            StartCoroutine(ChangeCoroutine());
            if (selectNum > images.Count - 1)
            {
                selectNum = images.Count - 1;
            }
            isSelect = false;
        }

        Select();
    }


    IEnumerator ChangeCoroutine()
    {
        isMove = true;
        if (selectNum >= 0 && selectNum < images.Count)
        {
            audio.PlayOneShot(moveSE);
            yield return new WaitForSeconds(0.4f);

        }
        isMove = false;
    }

}

