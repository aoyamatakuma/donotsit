using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PouseButtonScript : MonoBehaviour
{
    Fade fade;
    public GameObject end;
    public GameObject title;
    public GameObject retry;
    private AudioSource audio;
    private bool select,push;
    public AudioClip selectSE;
    public AudioClip moveSE;
    private int selectNum;
    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<Fade>();
        audio = GetComponent<AudioSource>();
        select = false;
        push = false;
        selectNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Select();
        if (Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            audio.PlayOneShot(selectSE);
            if (selectNum == 0)
            {
                SceneManager.LoadScene("Title");
                Time.timeScale = 1;
                // fade.StartFadeIn("Title", true);
            }
            else if(selectNum == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1;
            }
            else if (selectNum == 2)
            {
                endClick(); 
            }
        }
    }
    void Select()
    {
        float ver = Input.GetAxis("Vertical");

        if(selectNum == 0)
        {
            end.GetComponent<Outline>().enabled = false;
            title.GetComponent<Outline>().enabled = true;
            retry.GetComponent<Outline>().enabled = false;
        }
        else if (selectNum == 1)
        {
            end.GetComponent<Outline>().enabled = false;
            title.GetComponent<Outline>().enabled = false;
            retry.GetComponent<Outline>().enabled = true;
        }
        else if (selectNum == 2)
        {
            end.GetComponent<Outline>().enabled = true;
            title.GetComponent<Outline>().enabled = false;
            retry.GetComponent<Outline>().enabled = false;
        }
        if (!push)
        {
            if ( ver < -0.5f)
            {
                push = true;
                audio.PlayOneShot(moveSE);
                selectNum++;
                if (selectNum >= 3)
                {
                    selectNum = 2;
                }
            }
            if (ver > 0.5f )
            {
                push = true;
                audio.PlayOneShot(moveSE);
                selectNum--;
                if (selectNum < 0)
                {
                    selectNum = 0;
                }
            }
        }


        if(ver == 0)
        {
            push = false;
        }

    }

    public void endClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }
}
