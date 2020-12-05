using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingSelect : MonoBehaviour
{
    public List<Image> selectObjects;
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
  

    // Start is called before the first frame update
    void Start()
    {
        isPush = false;
        selectNum = 0;
        Select();
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        //  SetBool();
        // FreezImage();
        SelectMove();
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush)
        {
            isPush = true;

            // SceneManager.LoadScene("Stage" + selectNum + 1);
            if (selectNum == 0)
            {
                audio.PlayOneShot(selectSE);
                StageDate.Instance.SetSceneName("StageEasy");
                fade.StartFadeIn("RankingEasy", true);
            }
            else if (selectNum == 1)
            {
                audio.PlayOneShot(selectSE);
                StageDate.Instance.SetSceneName("StageNormal");
                fade.StartFadeIn("RankingNormal", true);
            }
            else if (selectNum == 2)
            {
                audio.PlayOneShot(selectSE);
                StageDate.Instance.SetSceneName("StageHard");
                fade.StartFadeIn("RankingHard", true);
            }
            else
            {
                audio.PlayOneShot(selectSE);
                StageDate.Instance.SetSceneName("StageExtra");
                fade.StartFadeIn("RankingExtra", true);
            }
        }

        if ((Input.GetKey(KeyCode.Escape) || Input.GetButtonDown("Attack")) && !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            fade.StartFadeIn("Title", true);
        }
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

    //void FreezImage()
    //{
    //    if (StageDate.GetBool(StageDate.easyKey, true))
    //    {
    //        selectObjects[0].color = new Color(1, 1, 1);
    //    }
    //    else
    //    {
    //        selectObjects[0].color = new Color(0, 0, 0);
    //    }

    //    if (StageDate.GetBool(StageDate.normalKey, true))
    //    {
    //        selectObjects[1].color = new Color(1, 1, 1);
    //    }
    //    else
    //    {
    //        selectObjects[1].color = new Color(0, 0, 0);
    //    }

    //    if (StageDate.GetBool(StageDate.hardKey, true))
    //    {
    //        selectObjects[2].color = new Color(1, 1, 1);
    //    }
    //    else
    //    {
    //        selectObjects[2].color = new Color(0, 0, 0);
    //    }

    //    if (StageDate.GetBool(StageDate.extraKey, true))
    //    {
    //        selectObjects[3].color = new Color(1, 1, 1);
    //    }
    //    else
    //    {
    //        selectObjects[3].color = new Color(0, 0, 0);
    //    }
    //}
}
