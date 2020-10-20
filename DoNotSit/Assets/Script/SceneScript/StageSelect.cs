using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public List<GameObject> selectObjects;
    private int selectNum;
    private bool isSelect;
    // Start is called before the first frame update
    void Start()
    {
        selectNum = 0;
        Select();
    }

    // Update is called once per frame
    void Update()
    {
       
        SelectMove();
        //if (Input.GetKey(KeyCode.Space) || Input.GetButton("Jump"))
        //{
        //    SceneManager.LoadScene("Main");
        //}
    }

    void Select()
    {
        if (isSelect)
        {
            return;
        }
        for (int i = 0; i < selectObjects.Count; i++)
        {
            selectObjects[i].GetComponent<Outline>().enabled = false;
        }
        selectObjects[selectNum].GetComponent<Outline>().enabled = true;
        isSelect = true;
    }

    void SelectMove()
    {
        float hol = Input.GetAxis("SelectMove");
        if(hol < 0.5f)
        {
            selectNum --;
            if (selectNum < 0)
            {
                selectNum = 0;
            }
            isSelect = false;
        }

        if(hol > 0.5f)
        {
            selectNum ++;
            if (selectNum > selectObjects.Count -1)
            {
               selectNum = selectObjects.Count - 1;
            }
            isSelect = false;
        }

        Select();
    }
}
