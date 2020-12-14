using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class Goal : MonoBehaviour
{
    Fade fade;
    public PlayableDirector playableDirector;
    private GameObject player;
    private GameObject chaseEnemy;
    public List<GameObject> ui;
    private bool isEvent;
    private GameObject camera;
    private Renderer renderer;

    void Start()
    {
        fade = GetComponent<Fade>();
        renderer = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        chaseEnemy = GameObject.FindGameObjectWithTag("ChaseEnemy");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        if (isEvent)
        {
            ExitMovie();
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {    
            StageDate.Instance.SetData(SceneManager.GetActiveScene().name);
            StartMovie();
        }
    }

    void StartMovie()
    {
        player.SetActive(false);
        chaseEnemy.SetActive(false);
        camera.SetActive(false);
        renderer.enabled = false;
        for(int i = 0; i < ui.Count; i++)
        {
            ui[i].SetActive(false);
        }
        playableDirector.Play();
        isEvent = true;
    }

    void ExitMovie()
    {
        if (playableDirector.state != PlayState.Playing)
        {
            playableDirector.Stop();
            fade.StartFadeIn("GameClear", false);
        }
    }
}
