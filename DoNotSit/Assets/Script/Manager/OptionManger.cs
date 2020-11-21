using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManger : MonoBehaviour
{
    Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.Escape) || Input.GetButtonDown("Attack")))
        {
            fade.StartFadeIn("Title", true);
        }
    }
}
