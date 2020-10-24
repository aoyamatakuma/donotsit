using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleFadeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fadeOutPrefab;
    private GameObject fadeOutInstance;

    [SerializeField]
    private GameObject fadeInPrefab;
    private GameObject fadeInInstance;

    public GameObject startImage;
    public GameObject exitImage;
    private bool select;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(fadeOutInstance);
        Destroy(fadeInInstance);
        if (fadeOutInstance == null)
        {
            //fadeOutInstance = GameObject.Instantiate(fadeOutPrefab) as GameObject;
        }

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
                //SceneManager.LoadScene("StageSelect");
                StartCoroutine("End");

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

        if (select)
        {
            startImage.GetComponent<Outline>().enabled = true;
            exitImage.GetComponent<Outline>().enabled = false;
        }
        else
        {
            startImage.GetComponent<Outline>().enabled = false;
            exitImage.GetComponent<Outline>().enabled = true;
        }

        if (ver > 0.5f)
        {
            select = true;

        }
        else if (ver < -0.5f)
        {
            select = false;
        }

    }

    public IEnumerator End()
    {
        if (fadeInInstance == null)
        {
            
            fadeInInstance = GameObject.Instantiate(fadeInPrefab) as GameObject;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("StageSelect");
        }
    }

}



