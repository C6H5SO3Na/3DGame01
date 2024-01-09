using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDirector : MonoBehaviour
{
    GameObject[] items;
    [SerializeField] TextMeshProUGUI clearText;
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
        items = GameObject.FindGameObjectsWithTag("HardObject");
        GameObject[] item = GameObject.FindGameObjectsWithTag("Item");
        Debug.Log(item.Length);
        if (items.Length == 0)
        {
            pc.state = PlayerController.State.Clear;
            Invoke("AfterGameClear", 2.0f);
            clearText.text = "Game Clear!!!";
        }
    }

    void AfterGameClear()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
