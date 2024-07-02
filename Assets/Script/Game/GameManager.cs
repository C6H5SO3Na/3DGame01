using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public enum Phase
    {
        FadeIn, Ready, Game, Clear, Dead, FadeOut
    }

    UIManager UIManager;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject player;
    [SerializeField] GameObject BGMPlayer;
    [SerializeField] Light directionalLight;
    [SerializeField] int maxStage;
    [SerializeField] Image fade;

    public static int stage = 1;//ステージ1からスタート
    public static int score = 0;
    public static int preScore = 0;//コース開始時のスコア
    public static Phase phase;//ゲームの段階

    float minCnt = 0.0f;//秒数

    // Start is called before the first frame update
    void Start()
    {
        //ステージ1、すなわちゲーム開始時にBGMを再生
        if (stage == 1)
        {
            Instantiate(BGMPlayer);
        }
        phase = Phase.FadeIn;
        UIManager = ui.GetComponent<UIManager>();
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

        //段階ごとの処理
        switch (phase)
        {
            case Phase.FadeIn:
                FadeIn();
                break;

            case Phase.Ready:
                Ready();
                break;

            case Phase.Game:
                Game();
                break;

            case Phase.Clear:
            case Phase.Dead://共通の処理
                StageEnd();
                break;

            case Phase.FadeOut:
                FadeOut();
                break;
        }
    }

    /// <summary>
    /// フェードイン時の処理
    /// </summary>
    void FadeIn()
    {
        //ステージ数に応じて光(太陽)の向きを変える
        LightAngleOperation();
        fade.GetComponent<Fade>().Fadein();
        if (!fade.GetComponent<Fade>().isFade)
        {
            ++phase;
        }
    }

    /// <summary>
    /// 「Ready」が表示されている際の処理
    /// </summary>
    void Ready()
    {
        minCnt += Time.deltaTime;
        if (minCnt > 1.0f)
        {
            UIManager.HideReadyImage();
            ++phase;
            minCnt = 0.0f;
        }
    }

    /// <summary>
    /// ゲーム本編の処理
    /// </summary>
    void Game()
    {
        //硬いオブジェクトが全部なくなったらゲームクリア
        if (GetWoodBoxNum() == 0)
        {
            PlayerController.playerState = PlayerController.State.Clear;
            UIManager.ShowClearImage();
            ++phase;
        }
    }

    /// <summary>
    ///ステージ終了時(クリア・死亡共通)の処理
    /// </summary>
    void StageEnd()
    {
        minCnt += Time.deltaTime;
        if (minCnt > 3.0f)
        {
            phase = Phase.FadeOut;
            minCnt = 0.0f;
        }
    }

    /// <summary>
    /// フェードアウト時の処理
    /// </summary>
    void FadeOut()
    {
        fade.GetComponent<Fade>().Fadeout();
        if (!fade.GetComponent<Fade>().isFade)
        {
            ToNextStage();
        }
    }

    /// <summary>
    /// 次のステージへ遷移
    /// </summary>
    void ToNextStage()
    {
        if (PlayerController.playerState == PlayerController.State.AfterClear)
        {
            ++stage;
            preScore = score;//スコアを保存
            if (stage > maxStage)
            {
                SceneManager.LoadScene("EndingScene");
                return;
            }
        }
        else
        {
            score = preScore;//やられたときは前のスコアに戻す
        }
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 太陽風の光をステージ毎に変える
    /// </summary>
    void LightAngleOperation()
    {
        Vector3 lightAngle = directionalLight.transform.rotation.eulerAngles;
        lightAngle.x = 5 * (stage - 1);
        directionalLight.transform.rotation = Quaternion.Euler(lightAngle);
    }

    /// <summary>
    /// スコアを加算
    /// </summary>
    /// <param name="n">加えたいスコア</param>
    public void AddScore(int n)
    {
        score += n;
    }

    /// <summary>
    /// 木箱の残り数を取得
    /// </summary>
    /// <returns>木箱の残り数</returns>
    public int GetWoodBoxNum()
    {
        return GameObject.FindGameObjectsWithTag("NormalObject").Length;
    }
}
