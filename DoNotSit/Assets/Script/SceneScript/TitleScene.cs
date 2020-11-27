using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TitleScene : MonoBehaviour
{
    public List<Image> images;
    public AudioClip moveSE;
    public AudioClip selectSE;
    AudioSource audio;
    bool isPush;
    private int selectNum;
    private bool isSelect;
    private bool isMove;

    bool status;
    Fade fade;
    float BGM;
    float SE;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        selectNum = images.Count -1;
        Select();
        isPush = false;
        Time.timeScale = 1.0f;
        BGM= StageDate.GetAudio("BGM");
        SE = StageDate.GetAudio("SE");
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
       
    }

    // Update is called once per frame
    void Update()
    {
        status = StageDate.GetBool(StageDate.clearKey, false);
        FreezImage();
        SelectMove();
        mixer.SetFloat("SE", SE);
        mixer.SetFloat("BGM", BGM);
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump") )&& !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            if (selectNum == 0)
            {
                Quit();
               
            }
            else if(selectNum ==1)
            {
                fade.StartFadeIn("Option", true);
              
            }
            else if (selectNum == 2)
            {
                if (status)
                {
                    fade.StartFadeIn("RankingSelect", true);
                }
                else
                {
                    isPush = false;
                }
              
            }
            else 
            {
                fade.StartFadeIn("StageSelect", true);
            }

        }
    }

    void FreezImage()
    {
        if (!status)
        {
            images[2].color = new Color(0,0,0);
        }
        else
        {
            images[2].color = new Color(1, 1, 1);
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }

    void Select()
    {
        if (isSelect)
        {
            return;
        }
        for (int i = 0; i < images.Count; i++)
        {
            images[i].SetNativeSize();
        }
        images[selectNum].rectTransform.sizeDelta *= 1.1f;
        isSelect = true;

    }

    void SelectMove()
    {
        if (isPush)
        {
            return;
        }
        float hol = Input.GetAxis("SelectMove");
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
