using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDirector : MonoBehaviour
{
    GameObject[] items;
    GameObject[] hardObjects;
    [SerializeField] TextMeshProUGUI clearText;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI hardObjectsText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject player;
    PlayerController pc;

    public static int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        Application.targetFrameRate = 60;
        pc = player.GetComponent<PlayerController>();
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
            pc.SetState(PlayerController.State.Clear);
            Invoke("AfterGameClear", 2.0f);
            clearText.text = "Game Clear!!!";
        }
        weaponText.text = $"Item:{PlayerController.getItemNum[0]}";
        hardObjectsText.text = $"Hard Objects:{hardObjects.Length}";
        scoreText.text = $"Score:{GetScore()}";
    }

    void AfterGameClear()
    {
        SceneManager.LoadScene("ResultScene");
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
