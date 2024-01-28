using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDirector : MonoBehaviour
{
    GameObject[] hardObjects;
    [SerializeField] TextMeshProUGUI clearText;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI hardObjectsText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject player;
    [SerializeField] Light directionalLight;

    public static int stage = 1;
    public static int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
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
            PlayerController.playerState = PlayerController.State.Clear;
            Invoke("ToNextStage", 2.0f);
            clearText.text = "Game Clear!!!";
        }

        //プレイヤがやられたら、同じコースをやり直し
        if (PlayerController.playerState == PlayerController.State.Dead)
        {
            Invoke("ToNextStage", 2.0f);
        }
        weaponText.text = $"x {PlayerController.getItemNum[3]}";
        hardObjectsText.text = $"x {hardObjects.Length}";
        scoreText.text = $"スコア {GetScore()}";

        //ステージ数に応じて光(太陽)の向きを変える
        Vector3 lightAngle = directionalLight.transform.rotation.eulerAngles;
        lightAngle.x = 5 * (stage - 1);
        directionalLight.transform.rotation = Quaternion.Euler(lightAngle);
    }

    void ToNextStage()
    {
        if (PlayerController.playerState == PlayerController.State.Clear)
        {
            ++stage;
        }
        SceneManager.LoadScene("GameScene");
    }

    public void AddScore(int n)
    {
        score += n;
    }

    public int GetScore()
    {
        return score;
    }
}
