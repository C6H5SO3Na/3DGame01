using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlayer : MonoBehaviour
{
    public static bool isCreated = false;
    // Start is called before the first frame update
    void Awake()
    {
        //1つしか生成されないようにする
        if (!isCreated)
        {
            DontDestroyOnLoad(gameObject);
            isCreated = true;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        //エンディング画面に推移時に削除
        if (SceneManager.GetActiveScene().name == "EndingScene")
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            Destroy(gameObject);
        }
    }
}
