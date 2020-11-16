using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public List<Image> selectObjects;
    private int selectNum;
    private bool isSelect;
    private bool isMove;
    public Color selectColor;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
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

        SelectMove();
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            // SceneManager.LoadScene("Stage" + selectNum + 1);
            if (selectNum == 0)
            {
                fade.StartFadeIn("StageEasy", true);
            }
            else if(selectNum == 1)
            {
               fade.StartFadeIn("StageNormal", true);
            }
            else if (selectNum == 2)
            {
                fade.StartFadeIn("StageHard", true);
            }
            else 
            {
                fade.StartFadeIn("StageExtra", true);
            }
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
