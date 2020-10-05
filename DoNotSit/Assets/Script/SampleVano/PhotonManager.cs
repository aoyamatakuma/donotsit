using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    public string objectName;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Connect("1.0");
    }

    private void Connect(string gameVersion)
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /// <summary>
    /// ロビーに接続すると呼ばれるメソッド
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("ログイン成功");
        // ランダムでルームに入室する
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// ランダムで部屋に入室できなかった場合呼ばれるメソッド
    /// </summary>
    void OnPhotonRandomJoinFailed()
    {
        // ルームを作成、部屋名は今回はnullに設定
        PhotonNetwork.CreateRoom(null);
    }

    /// <summary>
    /// ルームに入室成功した場合呼ばれるメソッド
    /// </summary>
   public override void OnJoinedRoom()
    {
        Debug.Log("ああああああああああああああああああああああああああああ");
        GameObject cube = PhotonNetwork.Instantiate(objectName, Vector3.zero, Quaternion.identity, 0);
    }

    /// <summary>
    /// UnityのGameウィンドウに表示させる
    /// </summary>
    void OnGUI()
    {
        // Photonのステータスをラベルで表示させています
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
}
