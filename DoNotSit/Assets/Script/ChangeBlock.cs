using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBlock : MonoBehaviour
{
  
    public List<BlockElement> blocks;
    public int BPM = 120;
    private float totalTime;
    bool isChange;
    AudioSource audio;
    public AudioClip tempoSE;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        totalTime = 60f / (float)BPM;
        isChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChange)
        {
            StartCoroutine(ChangeCoroutine());
        }
    }

    IEnumerator ChangeCoroutine()
    {
        isChange = true;
        audio.PlayOneShot(tempoSE);
        for(int i = 0; i < blocks.Count; i++)
        {
            if(blocks[i].elementNum == 0)
            {
                blocks[i].Draw();
            }
            else
            {
                blocks[i].Delete();
            }
          
        }

        yield return new WaitForSeconds(totalTime);

        audio.PlayOneShot(tempoSE);
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].elementNum == 1)
            {
                blocks[i].Draw();
            }
            else
            {
                blocks[i].Delete();
            }

        }

        yield return new WaitForSeconds(totalTime);
        isChange = false;
    }

}
