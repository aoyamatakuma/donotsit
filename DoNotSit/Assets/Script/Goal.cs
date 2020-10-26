using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    Fade fade;

    void Start()
    {
        fade = GetComponent<Fade>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            fade.StartFadeIn("GameClear",false);
        }
    }
}
