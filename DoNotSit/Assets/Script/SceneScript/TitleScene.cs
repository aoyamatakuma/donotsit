using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public GameObject startImage;
    public GameObject exitImage;
    private bool select;
    // Start is called before the first frame update
    void Start()
    {
        select = true;
    }

    // Update is called once per frame
    void Update()
    {
        Select();
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Jump"))
        {
            if (select)
            {
                SceneManager.LoadScene("StageSelect");
            }
            else
            {
                Quit();
            }

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
        float ver = Input.GetAxis("Vertical");

        if(select)
        {
            startImage.GetComponent<Outline>().enabled = true;
            exitImage.GetComponent<Outline>().enabled = false;
        }
        else
        {
            startImage.GetComponent<Outline>().enabled = false;
            exitImage.GetComponent<Outline>().enabled = true;
        }

        if(ver > 0.5f)
        {
            select = true;
           
        }
        else if(ver < -0.5f)
        {
            select = false;
        }

    }

}
