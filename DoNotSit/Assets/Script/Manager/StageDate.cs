using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDate:SingletonMonoBehaviour<StageDate>
{
    public readonly static StageDate Instance = new StageDate();
    

    public string referer = string.Empty;
    public string rankingSceneName = string.Empty;

    public static string clearKey ="clear";
    public static string easyKey = "easy";
    public static string normalKey = "normal";
    public static string hardKey = "hard";
    public static string extraKey = "extra";

  

    public void SetData(string stageName)
    {
        referer = stageName;
    }

    public void SetSceneName(string rankingName)
    {
        rankingSceneName = rankingName;
    }

    public static bool GetBool(string key,bool defaultValue)
    {
        bool isKey;
        var value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        if(value == 0)
        {
            isKey = false;
        }
        else
        {
            isKey = true;
        }
        return isKey;
    }

    public static void SetBool(string key,bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static void SetAudio(float BGM,float SE)
    {
        PlayerPrefs.SetFloat("BGM", BGM);
        PlayerPrefs.SetFloat("SE", SE);
    }

    public static float GetAudio(string key)
    {
        var value = PlayerPrefs.GetFloat(key);

        return value;
    }
 

}
