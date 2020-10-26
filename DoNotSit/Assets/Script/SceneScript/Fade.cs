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
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartFadeIn(string SceneName)
    {
        StartCoroutine(End(SceneName));
    }

     IEnumerator End(string SceneName)
    {
        if (fadeInInstance == null)
        {
            yield return new WaitForSeconds(0.5f);
            audio.PlayOneShot(fadeSE);
            fadeInInstance = GameObject.Instantiate(fadeInPrefab) as GameObject;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneName);
        }
    }
}
