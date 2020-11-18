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
    private AudioSource audio;
    private bool select,push;
    public AudioClip selectSE;
    public AudioClip moveSE;
    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<Fade>();
        audio = GetComponent<AudioSource>();
        select = false;
        push = false;
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
                endClick();
            }
            else
            {
                SceneManager.LoadScene("Title");
                Time.timeScale = 1;
               // fade.StartFadeIn("Title", true);
            }
        }
    }
    void Select()
    {
        float ver = Input.GetAxis("Vertical");

        if (!select)
        {
            end.GetComponent<Outline>().enabled = false;
            title.GetComponent<Outline>().enabled = true;
        }
        else
        {
            end.GetComponent<Outline>().enabled = true;
            title.GetComponent<Outline>().enabled = false;
        }
        if(!push)
        {
            if (ver > 0.5f || ver < -0.5f)
            {
                push = true;
                audio.PlayOneShot(moveSE);
                if (select)
                {
                    select = false;
                }
                else
                {
                    select = true;
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
