using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearScene : MonoBehaviour
{
    public GameObject stageSelect;
    public GameObject title;
    private bool select;
    private AudioSource audio;
    public AudioClip selectSE;
    public AudioClip moveSE;
    public Text goalTimeText;
    float goaltimer;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        select = true;
        goaltimer = PlayerControl.TimeScore();
        goalTimeText.text = "GT:"+ goaltimer.ToString("f0") + "秒";//ゴールタイム
    }

    // Update is called once per frame
    void Update()
    {
        Select();
        if (Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            audio.PlayOneShot(selectSE);
            if (select)
            {
               
                SceneManager.LoadScene("StageSelect");
            }
            else
            {
                SceneManager.LoadScene("Title");
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
