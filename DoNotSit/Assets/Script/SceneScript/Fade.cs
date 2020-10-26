using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField]
    private GameObject fadeOutPrefab;
    private GameObject fadeOutInstance;

    [SerializeField]
    private GameObject fadeInPrefab;
    private GameObject fadeInInstance;

    AudioSource audio;
    public AudioClip fadeSE;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(fadeOutInstance);
        Destroy(fadeInInstance);
        audio = GetComponent<AudioSource>();
        if (fadeOutInstance == null && fadeOutPrefab != null)
        {
            fadeOutInstance = GameObject.Instantiate(fadeOutPrefab) as GameObject;
        }
    }

    public void StartFadeIn(string SceneName,bool isWait)
    {
        StartCoroutine(End(SceneName,isWait));
    }

     IEnumerator End(string SceneName,bool isWait)
    {
        if (fadeInInstance == null)
        {
            if (isWait)
            {
                yield return new WaitForSeconds(0.5f);
            }
            audio.PlayOneShot(fadeSE);
            fadeInInstance = GameObject.Instantiate(fadeInPrefab) as GameObject;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneName);
        }
    }
}
