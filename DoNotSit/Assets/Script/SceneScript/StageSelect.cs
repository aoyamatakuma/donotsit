using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public List<Image> selectObjects;
    public Image hardImage;
    public Image exImage;
    public Image hardLockImage;
    public Image exLockImage;
    private int selectNum;
    private bool isSelect;
    private bool isMove;
    public Color selectColor;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    public AudioClip lockPushSE;
    bool isPush;
    Fade fade;
    bool isEasy;
    bool isNormal;
    bool isHard;
    bool isExtra;
    // Start is called before the first frame update
    void Start()
    {
        SetBool();
        DrawChange();
        isPush = false;
        selectNum = 0;
        Select();
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
        if (isNormal)
        {
            selectObjects[2] = hardImage;
        }

        if (isHard)
        {
            selectObjects[3] = exImage;
        }
    }

    // Update is called once per frame
    void Update()
    {

        SelectMove();
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush)
        {
            isPush = true;
          
            // SceneManager.LoadScene("Stage" + selectNum + 1);
            if (selectNum == 0)
            {
                audio.PlayOneShot(selectSE);
                fade.StartFadeIn("StageEasy", true);
            }
            else if(selectNum == 1)
            {
                audio.PlayOneShot(selectSE);
                fade.StartFadeIn("StageNormal", true);
            }
            else if (selectNum == 2)
            {
                if (isNormal)
                {
                    audio.PlayOneShot(selectSE);
                    fade.StartFadeIn("StageHard", true);
                }
                else
                {
                    audio.PlayOneShot(lockPushSE);
                    isPush = false;
                }
            }
            else 
            {
                if (isHard)
                {
                    audio.PlayOneShot(selectSE);
                    fade.StartFadeIn("StageExtra", true);
                }
                else
                {
                    audio.PlayOneShot(lockPushSE);
                    isPush = false;
                }
              
            }
        }

        if((Input.GetKey(KeyCode.Escape)||Input.GetButtonDown("Attack")) && !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            fade.StartFadeIn("Title", true);
        }
    }

    void DrawChange()
    {
        if (isNormal)
        {
            hardImage.gameObject.SetActive(true);
            hardLockImage.gameObject.SetActive(false);
        }
        else
        {
            hardImage.gameObject.SetActive(false);
            hardLockImage.gameObject.SetActive(true);
        }

        if (isHard)
        {
            exImage.gameObject.SetActive(true);
            exLockImage.gameObject.SetActive(false);
        }
        else
        {
            exImage.gameObject.SetActive(false);
            exLockImage.gameObject.SetActive(true);
        }
    }

    void SetBool()
    {
        isEasy = StageDate.GetBool(StageDate.easyKey, false);
        isNormal = StageDate.GetBool(StageDate.normalKey, false);
        isHard = StageDate.GetBool(StageDate.hardKey, false);
        isExtra = StageDate.GetBool(StageDate.extraKey, false);
    }

    void Select()
    {
        if (isSelect)
        {
            return;
        }
        for (int i = 0; i < selectObjects.Count; i++)
        {
            selectObjects[i].color = selectColor;
        }
        selectObjects[selectNum].color = new Color(1f, 1f, 1f);
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
            if (selectNum > selectObjects.Count - 1)
            {
                selectNum = selectObjects.Count - 1;
            }
            isSelect = false;
        }

        Select();
    }

    IEnumerator ChangeCoroutine()
    {
        isMove = true;
        if (selectNum >= 0 && selectNum < selectObjects.Count)
        {
            audio.PlayOneShot(moveSE);     
            yield return new WaitForSeconds(0.4f);
           
        }
        isMove = false;
    }
}
