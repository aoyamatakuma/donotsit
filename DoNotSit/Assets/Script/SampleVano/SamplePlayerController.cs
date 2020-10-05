using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class SamplePlayerController : MonoBehaviour
{
    private PhotonView photonView;

    void Start()
    {
        photonView = PhotonView.Get(this);//MonoBehaviourクラスを継承するクラスはこれでPhotonViewを参照
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            transform.Translate(x * 0.2f, 0, z * 0.2f);
        }
    }
}
