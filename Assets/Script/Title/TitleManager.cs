using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] AudioClip titleVoice;
    [SerializeField] AudioClip buttonSE;
    [SerializeField] Image fade;
    [SerializeField] Image pressAButton;
    public bool isPressed = false;
    bool hasFadein;
    float mainCnt = 0.0f;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();

        //クリア時にリセット
        GameManager.stage = 1;
        GameManager.score = 0;
        GameManager.preScore = 0;//コース開始時のスコア
        BGMPlayer.isCreated = false;

        Application.targetFrameRate = 60;

        PlayerController.isRapidFire = false;
        PlayerController.isAbleMultiShot = false;
        PlayerController.speed = 4.0f;
        //アイテムの取得数初期化
        for (int i = 0; i < PlayerController.getItemNum.Length; ++i)
        {
            PlayerController.getItemNum[i] = 0;
        }
        mainCnt = 0.0f;
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
        //フェードインが終わってから操作できる
        if (!hasFadein)
        {
            fade.GetComponent<Fade>().Fadein();
            if (fade.GetComponent<Fade>().isFade) { return; }
        }

        hasFadein = true;
        if (Input.GetButtonDown("Fire1") && !isPressed)
        {
            aud.PlayOneShot(titleVoice);
            aud.PlayOneShot(buttonSE);
            isPressed = true;
            //ボタンが押されると反応
            pressAButton.GetComponent<PressAButtonController>().changeAmount = 8.0f;
        }

        if (isPressed)
        {
            ToGameScene();
        }
    }

    /// <summary>
    /// ゲームシーンへ推移するときの処理
    /// </summary>
    void ToGameScene()
    {
        mainCnt += Time.deltaTime;
        if (mainCnt < 2.0f) { return; }
        fade.GetComponent<Fade>().Fadeout();
        if (!fade.GetComponent<Fade>().isFade)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
