using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManger : MonoBehaviour
{
    Fade fade;
    bool isPush;
    public Slider bgm;
    public Slider se;
    float BGM;
    float SE;
    public AudioMixer mixer;
    AudioSource audio;
    public AudioClip moveSE;
    public List<Image> selectAudio;
    private int selectNum;
    private bool isSelect;
    private bool isMove;
    public float wait;
    // Start is called before the first frame update
    void Start()
    {
        BGM = bgm.value;
        SE = se.value;
        fade = GetComponent<Fade>();
        bgm.value = StageDate.GetAudio("BGM");
        se.value = StageDate.GetAudio("SE");
        audio = GetComponent<AudioSource>();
        selectNum = 0;
        Select();
    }

    // Update is called once per frame
    void Update()
    {
        SelectMove();
        BGM = bgm.value;
        SE = se.value;
        mixer.SetFloat("SE", SE);
        mixer.SetFloat("BGM", BGM);
        if (selectNum == 0)//BGM
        {

            float hol = Input.GetAxis("SelectMove");
            if (hol < -0.5f && !isMove)
            {
                bgm.value--;
                StartCoroutine(ChangeCoroutine2());
            }

            if (hol > 0.5f && !isMove)
            {
                bgm.value++;
                StartCoroutine(ChangeCoroutine2());
            }
            
        }
        else if (selectNum == 1)//SE
        {
            float hol = Input.GetAxis("SelectMove");
            if (hol < -0.5f && !isMove)
            {
                se.value--;
                StartCoroutine(ChangeCoroutine2());
            }

            if (hol > 0.5f && !isMove)
            {
                se.value++;
                StartCoroutine(ChangeCoroutine2());
            }
        }
        if ((Input.GetKey(KeyCode.Escape) || Input.GetButtonDown("Attack")) && !isPush)
        {
            StageDate.SetAudio(BGM, SE);
            fade.StartFadeIn("Title", true);
            isPush = true;
        }
    }
  public  void Select()
    {
        if (isSelect)
        {
            return;
        }
        for (int i = 0; i < selectAudio.Count; i++)
        {
            selectAudio[i].SetNativeSize();
        }
        selectAudio[selectNum].rectTransform.sizeDelta *= 1.2f;
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
            if (selectNum > selectAudio.Count - 1)
            {
                selectNum = selectAudio.Count - 1;
            }
            isSelect = false;
        }

        Select();
    }
    
    IEnumerator ChangeCoroutine()
    {
        isMove = true;
        if (selectNum >= 0 && selectNum < selectAudio.Count)
        {
               audio.PlayOneShot(moveSE);
            yield return new WaitForSeconds(0.4f);

        }
        isMove = false;
    }
    IEnumerator ChangeCoroutine2()
    {
        isMove = true;
        if (selectNum >= 0 && selectNum < selectAudio.Count)
        {
             // audio.PlayOneShot(moveSE);
            yield return new WaitForSeconds(wait);

        }
        isMove = false;
    }

}
