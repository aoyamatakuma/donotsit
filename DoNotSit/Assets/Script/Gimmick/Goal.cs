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
            StageDate.Instance.SetData(SceneManager.GetActiveScene().name);
            fade.StartFadeIn("GameClear",false);
        }
    }
}
