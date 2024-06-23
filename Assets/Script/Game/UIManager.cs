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
        scoreText.text = $"�X�R�A {GameManager.score}";
        switch (GameManager.phase)
        {
            case GameManager.Phase.FadeIn:
                operationText.text = operationStickText.text = "";
                break;
            case GameManager.Phase.Game:
                operationStickText.text = "LStick:�ړ��@RStick:���_�ύX";

                //���e���ˉ\�Ȏ��̂ݔ��e���ˑ����\��
                if (PlayerController.getItemNum[3] > 0)
                {
                    operationText.text = "A:�e���ˁ@X:���e����";
                }
                else
                {
                    operationText.text = "A:�e����";
                }
                break;
        }
    }
    /// <summary>
    /// �uStage Clear�v�ƕ\��
    /// </summary>
    public void ShowClearImage()
    {
        imageClear.gameObject.SetActive(true);
    }

    /// <summary>
    /// �uReady�v���\���ɂ���
    /// </summary>
    public void HideReadyImage()
    {
        ready.gameObject.SetActive(false);
    }
}
