using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{
    public float durations;
    public float magnitudes;
    Vector3 posi;
    public bool slowFlag;

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {

        var pos = transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
            if (slowFlag == true)
            {
                break;
            }
        }

        transform.localPosition = new Vector3(pos.x, posi.y, posi.z);

    }
    private void Start()
    {
        posi = gameObject.transform.position;
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            Shake(durations, magnitudes);
        }

    }
}
