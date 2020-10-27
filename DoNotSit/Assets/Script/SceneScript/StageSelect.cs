using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public List<GameObject> selectObjects;
    public GameObject rightObj;
    public GameObject leftObj;
    private int selectNum;
    private bool isSelect;
    private bool isMove;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        selectNum = 0;
        Select();
        audio = GetComponent<AudioSource>();
        fade = GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {

        SelectMove();
        if (Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            audio.PlayOneShot(selectSE);
            // SceneManager.LoadScene("Stage" + selectNum + 1);
            if (selectNum == 0)
            {
                fade.StartFadeIn("StageTest",true);
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
            selectObjects[i].SetActive(false);
        }
        selectObjects[selectNum].SetActive(true);
        isSelect = true;
    }

    void SelectMove()
    {
        float hol = Input.GetAxis("SelectMove");
        if (hol < -0.5f && !isMove)
        {
            selectNum--;
            StartCoroutine(ChangeCoroutine(false));
            if (selectNum < 0)
            {
                selectNum = 0;
            }
            isSelect = false;
        }

        if (hol > 0.5f && !isMove)
        {
            selectNum++;
            StartCoroutine(ChangeCoroutine(true));
            if (selectNum > selectObjects.Count - 1)
            {
                selectNum = selectObjects.Count - 1;
            }
            isSelect = false;
        }

        Select();
    }

    IEnumerator ChangeCoroutine(bool isRight)
    {
        isMove = true;
        if (selectNum >= 0 && selectNum < selectObjects.Count)
        {
            audio.PlayOneShot(moveSE);


            if (isRight)
            {
                rightObj.GetComponent<Outline>().enabled = true;
            }
            else
            {
                leftObj.GetComponent<Outline>().enabled = true;
            }
            yield return new WaitForSeconds(0.4f);
            if (isRight)
            {
                rightObj.GetComponent<Outline>().enabled = false;
            }
            else
            {
                leftObj.GetComponent<Outline>().enabled = false;
            }
        }
        isMove = false;
    }
}
