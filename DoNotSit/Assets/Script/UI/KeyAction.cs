using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyAction : MonoBehaviour
{
    public Color pushColor;
    private Image image;
    private Color baseColor;

    void Start()
    {
        image = GetComponent<Image>();
        baseColor = image.color;
    }

    public void Push()
    {
        StartCoroutine(PushCoroutine());
    }
   
    IEnumerator PushCoroutine()
    {
        image.color = pushColor;
        yield return new WaitForSeconds(0.5f);
        image.color = baseColor;
    }

    public void Reset()
    {
        image.color = baseColor;
    }
}
