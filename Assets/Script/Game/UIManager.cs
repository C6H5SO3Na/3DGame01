using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject manager;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI woodBoxesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI operationText;
    [SerializeField] TextMeshProUGUI operationStickText;
    [SerializeField] TextMeshProUGUI ready;
    [SerializeField] Image imageClear;//Game Clear!!!
    // Start is called before the first frame update
    void Start()
    {
        gameManager = manager.GetComponent<GameManager>();
        imageClear.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        weaponText.text = $"x {PlayerController.getItemNum[3]}";
        woodBoxesText.text = $"x {gameManager.GetWoodBoxNum()}";
        scoreText.text = $"XRA {GameManager.score}";
        switch (GameManager.phase)
        {
            case GameManager.Phase.FadeIn:
                operationText.text = operationStickText.text = "";
                break;
            case GameManager.Phase.Game:
                operationStickText.text = "LStick:Ú®@RStick:_ÏX";

                //e­ËÂ\ÈÌÝe­Ëìð\¦
                if (PlayerController.getItemNum[3] > 0)
                {
                    operationText.text = "A:e­Ë@X:e­Ë";
                }
                else
                {
                    operationText.text = "A:e­Ë";
                }
                break;
        }
    }
    /// <summary>
    /// uStage ClearvÆ\¦
    /// </summary>
    public void ShowClearImage()
    {
        imageClear.gameObject.SetActive(true);
    }

    /// <summary>
    /// uReadyvðñ\¦É·é
    /// </summary>
    public void HideReadyImage()
    {
        ready.gameObject.SetActive(false);
    }
}
