using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Icon : MonoBehaviour
{
    public float limitX;
    public float limitY;
    public float speed;
    private RectTransform icon;
    private string keyText;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        IconMove();
    }

    void IconMove()
    {
        // 画面左下のワールド座標をビューポートから取得
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // 画面右上のワールド座標をビューポートから取得
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        float ver = Input.GetAxis("Vertical");
        float hol = Input.GetAxis("Horizontal");
        Vector2 pos = icon.localPosition;
        pos += new Vector2(hol * speed, ver * speed);
        if (pos.x > limitX)
        {
            pos.x = limitX;
        }
        if (pos.x < -limitX)
        {
            pos.x = -limitX;
        }

        if (pos.y > limitY)
        {
            pos.y = limitY;
        }
        if (pos.y < -limitY)
        {
            pos.y = -limitY;
        }
        icon.localPosition = pos;

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Key")
        {
            keyText = col.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }
    }

    public string GetText()
    {
        return keyText;
    }
}
