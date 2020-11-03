using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockElement : MonoBehaviour
{
    public float elementNum;

    public void Draw()
    {
        gameObject.SetActive(true);
    }

    public void Delete()
    {
        gameObject.SetActive(false);
    }


}
