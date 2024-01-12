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
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] GameObject player;
    PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        hardObjects = GameObject.FindGameObjectsWithTag("HardObject");
        items = GameObject.FindGameObjectsWithTag("Item");
        //硬いオブジェクトが全部なくなったらゲームクリア
        if (hardObjects.Length == 0)
        {
            pc.state = PlayerController.State.Clear;
            Invoke("AfterGameClear", 2.0f);
            clearText.text = "Game Clear!!!";
        }

        itemText.text = $"Item:{PlayerController.getItemNum} {items.Length}";
    }

    void AfterGameClear()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
