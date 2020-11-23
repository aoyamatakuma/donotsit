using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    public Image stageSelect;
    public Image title;
    Image titleFont;
    private bool select;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    bool isPush;
    Fade fade;
    Vector2 titleBaseSize;
    Vector2 stageSelectBaseSize;
    Vector2 titleFontBasesize;
    // Start is called before the first frame update
    void Start()
    {
        titleFont = title.transform.GetChild(0).GetComponent<Image>();
        isPush = false;
        select = true;
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
        titleBaseSize = title.rectTransform.sizeDelta;
        stageSelectBaseSize = stageSelect.rectTransform.sizeDelta;
        titleFontBasesize = titleFont.rectTransform.sizeDelta;
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
            stageSelect.rectTransform.sizeDelta = new Vector2(stageSelectBaseSize.x * 1.2f,stageSelectBaseSize.y * 1.2f);
            title.rectTransform.sizeDelta = titleBaseSize;
            titleFont.rectTransform.sizeDelta = titleFontBasesize;
            //stageSelect.GetComponent<Outline>().enabled = true;
            //title.GetComponent<Outline>().enabled = false;
        }
        else
        {
            stageSelect.rectTransform.sizeDelta = stageSelectBaseSize;
            title.rectTransform.sizeDelta = new Vector2(titleBaseSize.x * 1.2f,titleBaseSize.y * 1.2f);
            titleFont.rectTransform.sizeDelta = new Vector2(titleFontBasesize.x * 1.2f, titleFontBasesize.y * 1.2f);
            //stageSelect.GetComponent<Outline>().enabled = false;
            //title.GetComponent<Outline>().enabled = true;
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

