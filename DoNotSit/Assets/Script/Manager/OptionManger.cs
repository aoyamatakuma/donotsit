using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManger : MonoBehaviour
{
    Fade fade;
    bool isPush;
    public Slider bgm;
    public Slider se;
    float BGM;
    float SE;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        BGM = bgm.value;
        SE = se.value;
        fade = GetComponent<Fade>();
        bgm.value = StageDate.GetAudio("BGM");
        se.value = StageDate.GetAudio("SE");
    }

    // Update is called once per frame
    void Update()
    {
        BGM = bgm.value;
        SE = se.value;
        mixer.SetFloat("SE", SE);
        mixer.SetFloat("BGM", BGM);
        if ((Input.GetKey(KeyCode.Escape) || Input.GetButtonDown("Attack")) && !isPush)
        {
            StageDate.SetAudio(BGM, SE);
            fade.StartFadeIn("Title", true);
            isPush = true;
        }
    }
}
