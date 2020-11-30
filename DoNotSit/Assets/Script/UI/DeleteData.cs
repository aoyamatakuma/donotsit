using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteData : MonoBehaviour
{
    bool isPush;
    float cnt;
    int pushCnt;
    
    void Update()
    {
        if (isPush)
        {
            cnt += Time.deltaTime; ;
            if(cnt > 2)
            {
                cnt = 0;
                pushCnt = 0;
                isPush = false;
            }
        }
    }
    public void Delete()
    {
        if (pushCnt ==0)
        {
            PlayerPrefs.DeleteAll();
        } 
        else if(pushCnt ==1)
        {
            StageDate.SetBool(StageDate.clearKey, true);
            StageDate.SetBool(StageDate.normalKey, true);
        }
        else if (pushCnt == 2)
        {
            StageDate.SetBool(StageDate.hardKey, true);
        }
        else
        {
            StageDate.SetBool(StageDate.extraKey, true);
        }

        pushCnt += 1;
    }
}
