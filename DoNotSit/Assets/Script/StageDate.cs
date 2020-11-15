using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDate:SingletonMonoBehaviour<StageDate>
{
    public readonly static StageDate Instance = new StageDate();

    public string referer = string.Empty;

    public void SetData(string stageName)
    {
        referer = stageName;
    }
}
