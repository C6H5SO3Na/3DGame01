using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDirector : MonoBehaviour
{
    GameObject[] hardObjects;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI hardObjectsText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI operationText;
    [SerializeField] GameObject player;
    [SerializeField] GameObject BGMPlayer;
    [SerializeField] Light directionalLight;
    [SerializeField] int maxStage;
    [SerializeField] Image imageClear;//Game Clear!!!

    PlayerController playerController;

    public AudioClip damageSE;
    public AudioClip shootSE;
    public AudioClip explosionSE;
    public AudioClip itemGetSE;
    public AudioClip hitSE;
    public AudioSource aud;

    public static int stage = 1;
    public static int score = 0;
    public static int preScore = 0;//コース開始時のスコア
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        aud = gameObject.GetComponent<AudioSource>();
        playerController = player.GetComponent<PlayerController>();
        imageClear.gameObject.SetActive(false);
        //ステージ1、すなわちゲーム開始時にBGMを再生
        if(stage == 1)
        {
            Instantiate(BGMPlayer);
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
        hardObjects = GameObject.FindGameObjectsWithTag("HardObject");

        //硬いオブジェクトが全部なくなったらゲームクリア
        if (hardObjects.Length == 0)
        {
            if (playerController.playerState != PlayerController.State.Clear)
            {
                playerController.playerState = PlayerController.State.Clear;
                Invoke("ToNextStage", 2.0f);
                imageClear.gameObject.SetActive(true);
            }
        }

        weaponText.text = $"x {PlayerController.getItemNum[3]}";
        hardObjectsText.text = $"x {hardObjects.Length}";
        scoreText.text = $"スコア {GetScore()}";
        //爆弾発射
        if(PlayerController.getItemNum[3] > 0)
        {
            operationText.text = "A:弾発射　B:ジャンプ　X:爆弾発射";
        }
        else
        {
            operationText.text = "A:弾発射　B:ジャンプ";
        }
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
        if (playerController.playerState == PlayerController.State.Clear)
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
