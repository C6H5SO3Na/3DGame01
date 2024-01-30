using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //クリア時にリセット
        GameDirector.stage = 1;
        GameDirector.score = 0;
        GameDirector.preScore = 0;//コース開始時のスコア
        BGMPlayer.isCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
