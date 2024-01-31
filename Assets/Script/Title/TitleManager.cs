using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] AudioClip titleVoice;
    [SerializeField] AudioClip buttonSE;
    bool isPressed = false;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();

        //クリア時にリセット
        GameDirector.stage = 1;
        GameDirector.score = 0;
        GameDirector.preScore = 0;//コース開始時のスコア
        BGMPlayer.isCreated = false;

        PlayerController.isRapidFire = false;
        PlayerController.isAbleMultiShot = false;
        //アイテムの取得数初期化
        for (int i = 0; i < PlayerController.getItemNum.Length; ++i)
        {
            PlayerController.getItemNum[i] = 0;
        }
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
        else if (Input.GetButtonDown("Fire1") && !isPressed)
        {
            aud.PlayOneShot(titleVoice);
            aud.PlayOneShot(buttonSE);
            Invoke("ToGame", 2.0f);
            isPressed = true;
        }
    }

    void ToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
