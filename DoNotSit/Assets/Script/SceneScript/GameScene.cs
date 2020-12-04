using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
        StageDate.Instance.SetData(SceneManager.GetActiveScene().name);
    }
}
