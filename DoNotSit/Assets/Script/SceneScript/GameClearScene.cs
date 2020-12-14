using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearScene : MonoBehaviour
{
    public GameObject easy;
    public GameObject normal;
    public GameObject hard;
    public GameObject ex;
    public GameObject easyBack;
    public GameObject normalBack;
    public GameObject hardBack;
    public GameObject exBack;
    public InputField inputField;
    public Text nameText;
    private bool isInput;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    public Text goalTimeText;
    public Text goalScoreText;
    public Text rankText;
    public Text stageText;
    public float maxValue;
    float goaltimer;
    float goalscore;
    string stageName;
    string rankingName;
    public string rank;
    public float speed;
    bool isPush;
    float preValue;
    Fade fade;
    string inputName = "Name";
    private bool isSelect;
    bool isMove;
    private int selectNum;
    public List<GameObject> images;
    private TouchScreenKeyboard keyboard;
    public RectTransform icon;
    string keyText;
    public List<GameObject> keys;
    int keyCnt;

    // Start is called before the first frame update
    void Start()
    {
        isPush = false;
        audio = GetComponent<AudioSource>();
        goaltimer = PlayerControl.TimeScore();
        goalscore = PlayerControl.ClearScore();
        rank = RankManger.ClearRank();
        goalScoreText.text = "Score:" + goalscore.ToString();//ゴールスコア
        fade = GetComponent<Fade>();
        stageName = StageDate.Instance.referer;
        selectNum = 0;
        keyCnt = 0;
        for (int i = 0; i < keys.Count; i++)
        {
            keys[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float value;
        if (float.TryParse(inputField.text,out value))
        {
            if(value > maxValue)
            {
                inputField.text = preValue + "";
            }
            else
            {
                preValue = value;
            }
        }
  
        Name();
        SelectMove();
        if (isInput)
        {
            SelectKey();
        }
      
        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isPush && !isInput)
        {
            isPush = true;
            audio.PlayOneShot(selectSE);
            if (selectNum == 0)
            {
                icon.gameObject.SetActive(true);
                for(int i = 0; i < keys.Count; i++)
                {
                    keys[i].SetActive(true);
                }
                isInput = true;
            }
            else if (selectNum ==1)
            {
                StageDate.SetBool(StageDate.clearKey, true);
                StageClearBool();
                QuickRanking.Instance.SaveRanking(nameText.text, stageName, (int)goalscore);
                fade.StartFadeIn(rankingName, true);
            }
            else
            {
                StageDate.SetBool(StageDate.clearKey, true);
                StageClearBool();
                QuickRanking.Instance.SaveRanking(nameText.text, stageName, (int)goalscore);
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

    void Name()
    {
        if (stageName == "StageEasy")
        {
            easy.SetActive(true);
            easyBack.SetActive(true);
        }
        else if (stageName == "StageNormal")
        {
            normal.SetActive(true);
            normalBack.SetActive(true);
        }
        else if (stageName == "StageHard")
        {
            hard.SetActive(true);
            hardBack.SetActive(true);
        }
        else
        {
            ex.SetActive(true);
            exBack.SetActive(true);
        }
    }
    void StageClearBool()
    {
        StageDate.Instance.SetSceneName(stageName);
        if (stageName == "StageEasy")
        {
            StageDate.SetBool(StageDate.easyKey, true);
            rankingName = "RankingEasy";
            
        }
        else if(stageName == "StageNormal")
        {
            StageDate.SetBool(StageDate.normalKey, true);
            rankingName = "RankingNormal";
        }
        else if(stageName == "StageHard")
        {
            StageDate.SetBool(StageDate.hardKey, true);
            rankingName = "RankingHard";
        }
        else
        {
            StageDate.SetBool(StageDate.extraKey, true);
            rankingName = "RankingExtra";
        }

      
    }

    //void IconRay()
    //{
    //    RaycastHit hit;
    //    int layerMask = 1 << 8;
    //    if (Physics.Raycast(
    //                  icon.localPosition,
    //                  new Vector3(0, 0, 1),
    //                  out hit,
    //                  Mathf.Infinity, layerMask))
    //    {
    //        Debug.Log(gameObject.name);
    //    }

    //    Debug.DrawRay(icon.localPosition, new Vector3(0,0,1),new Color(0,0,255), Mathf.Infinity);
    //}

    void SelectKey()
    {

        if (( Input.GetButtonDown("Jump")))
        {
            
            if(icon.GetComponent<Icon>().GetText() != null)
            {
                keyText = icon.GetComponent<Icon>().GetText();
                if(keyText == "ALLDELETE")
                {
                    inputField.text = "";
                    keyCnt = 0;
                }
                else
                {
                    if(keyCnt <= maxValue)
                    {
                        inputField.text += keyText;
                    }                   
                }
                keyCnt++;
            }
        }

        if ( Input.GetButtonDown("Attack"))
        {
            keyCnt = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                keys[i].SetActive(false);
            }
            icon.gameObject.SetActive(false);
            selectNum = 0;
            isPush = false;
            isInput = false;
        }

    }

}
