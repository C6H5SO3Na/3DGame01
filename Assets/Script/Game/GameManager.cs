using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    GameObject[] hardObjects;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI hardObjectsText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI operationText;
    [SerializeField] TextMeshProUGUI operationStickText;
    [SerializeField] GameObject player;
    [SerializeField] GameObject BGMPlayer;
    [SerializeField] Light directionalLight;
    [SerializeField] int maxStage;
    [SerializeField] Image imageClear;//Game Clear!!!
    [SerializeField] Image fade;
    [SerializeField] TextMeshProUGUI ready;

    public AudioClip damageSE;
    public AudioClip shootSE;
    public AudioClip explosionSE;
    public AudioClip itemGetSE;
    public AudioClip hitSE;
    public AudioSource aud;

    public static int stage = 1;
    public static int score = 0;
    public static int preScore = 0;//コース開始時のスコア
    public static int phase;//ゲームの段階

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        aud = gameObject.GetComponent<AudioSource>();
        imageClear.gameObject.SetActive(false);
        //ステージ1、すなわちゲーム開始時にBGMを再生
        if (stage == 1)
        {
            Instantiate(BGMPlayer);
        }
        phase = 0;
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
        hardObjects = GameObject.FindGameObjectsWithTag("HardObject");

        //段階ごとの処理
        switch (phase)
        {
            case 0://フェードイン
                operationText.text = operationStickText.text = "";
                fade.GetComponent<Fade>().Fadein();
                if (!fade.GetComponent<Fade>().isFade)
                {
                    ++phase;
                }
                break;
            case 1://Ready
                if (Input.GetButtonDown("Start"))
                {
                    ready.gameObject.SetActive(false);
                    ++phase;
                }
                break;

            case 2:
                //硬いオブジェクトが全部なくなったらゲームクリア
                if (hardObjects.Length == 0)
                {
                    PlayerController.playerState = PlayerController.State.Clear;
                    imageClear.gameObject.SetActive(true);
                    ++phase;
                }

                operationStickText.text = "LStick:移動　RStick:視点変更";
                //爆弾発射
                if (PlayerController.getItemNum[3] > 0)
                {
                    operationText.text = "A:弾発射　X:爆弾発射";
                }
                else
                {
                    operationText.text = "A:弾発射";
                }
                break;

            case 3:
                if (Input.GetButtonDown("Start"))
                {
                    ++phase;
                }
                break;

            case 4:
                fade.GetComponent<Fade>().Fadeout();
                if (!fade.GetComponent<Fade>().isFade)
                {
                    ToNextStage();
                }
                break;
        }

        weaponText.text = $"x {PlayerController.getItemNum[3]}";
        hardObjectsText.text = $"x {hardObjects.Length}";
        scoreText.text = $"スコア {GetScore()}";

        //ステージ数に応じて光(太陽)の向きを変える
        Vector3 lightAngle = directionalLight.transform.rotation.eulerAngles;
        lightAngle.x = 5 * (stage - 1);
        directionalLight.transform.rotation = Quaternion.Euler(lightAngle);
    }


    /// <summary>
    /// 次のステージへ遷移
    /// </summary>
    void ToNextStage()
    {
        if (PlayerController.playerState == PlayerController.State.Clear)
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
    /// スコアを加算
    /// </summary>
    /// <param name="n">加えたいスコア</param>
    public void AddScore(int n)
    {
        score += n;
    }

    /// <summary>
    /// 現在のスコアを取得
    /// </summary>
    /// <returns>現在のスコア</returns>
    public int GetScore()
    {
        return score;
    }
}
