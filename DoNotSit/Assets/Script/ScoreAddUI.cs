using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAddUI : MonoBehaviour
{
    public Text scoreText;
    public Text comboText;
    public float fadeOutSpeed = 1f;
    public float moveSpeed=0.4f;
    
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        scoreText.color = Color.Lerp(scoreText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

        if (scoreText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetScore(float score)
    {
        scoreText.text = score.ToString();
    }

    public void SetCombo(float combo)
    {
        comboText.text = "Combo:" + combo;
    }
}
