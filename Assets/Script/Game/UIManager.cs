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
        scoreText.text = $"スコア {GameManager.score}";
        switch (GameManager.phase)
        {
            case GameManager.Phase.FadeIn:
                operationText.text = operationStickText.text = "";
                break;
            case GameManager.Phase.Game:
                operationStickText.text = "LStick:移動　RStick:視点変更";

                //爆弾発射可能な時のみ爆弾発射操作を表示
                if (PlayerController.getItemNum[3] > 0)
                {
                    operationText.text = "A:弾発射　X:爆弾発射";
                }
                else
                {
                    operationText.text = "A:弾発射";
                }
                break;
        }
    }
    /// <summary>
    /// 「Stage Clear」と表示
    /// </summary>
    public void ShowClearImage()
    {
        imageClear.gameObject.SetActive(true);
    }

    /// <summary>
    /// 「Ready」を非表示にする
    /// </summary>
    public void HideReadyImage()
    {
        ready.gameObject.SetActive(false);
    }
}
