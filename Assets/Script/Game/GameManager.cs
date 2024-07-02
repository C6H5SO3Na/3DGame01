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

    public static int stage = 1;//�X�e�[�W1����X�^�[�g
    public static int score = 0;
    public static int preScore = 0;//�R�[�X�J�n���̃X�R�A
    public static Phase phase;//�Q�[���̒i�K

    float minCnt = 0.0f;//�b��

    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W1�A���Ȃ킿�Q�[���J�n����BGM���Đ�
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

        //�i�K���Ƃ̏���
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
            case Phase.Dead://���ʂ̏���
                StageEnd();
                break;

            case Phase.FadeOut:
                FadeOut();
                break;
        }
    }

    /// <summary>
    /// �t�F�[�h�C�����̏���
    /// </summary>
    void FadeIn()
    {
        //�X�e�[�W���ɉ����Č�(���z)�̌�����ς���
        LightAngleOperation();
        fade.GetComponent<Fade>().Fadein();
        if (!fade.GetComponent<Fade>().isFade)
        {
            ++phase;
        }
    }

    /// <summary>
    /// �uReady�v���\������Ă���ۂ̏���
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
    /// �Q�[���{�҂̏���
    /// </summary>
    void Game()
    {
        //�d���I�u�W�F�N�g���S���Ȃ��Ȃ�����Q�[���N���A
        if (GetWoodBoxNum() == 0)
        {
            PlayerController.playerState = PlayerController.State.Clear;
            UIManager.ShowClearImage();
            ++phase;
        }
    }

    /// <summary>
    ///�X�e�[�W�I����(�N���A�E���S����)�̏���
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
    /// �t�F�[�h�A�E�g���̏���
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
    /// ���̃X�e�[�W�֑J��
    /// </summary>
    void ToNextStage()
    {
        if (PlayerController.playerState == PlayerController.State.AfterClear)
        {
            ++stage;
            preScore = score;//�X�R�A��ۑ�
            if (stage > maxStage)
            {
                SceneManager.LoadScene("EndingScene");
                return;
            }
        }
        else
        {
            score = preScore;//���ꂽ�Ƃ��͑O�̃X�R�A�ɖ߂�
        }
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// ���z���̌����X�e�[�W���ɕς���
    /// </summary>
    void LightAngleOperation()
    {
        Vector3 lightAngle = directionalLight.transform.rotation.eulerAngles;
        lightAngle.x = 5 * (stage - 1);
        directionalLight.transform.rotation = Quaternion.Euler(lightAngle);
    }

    /// <summary>
    /// �X�R�A�����Z
    /// </summary>
    /// <param name="n">���������X�R�A</param>
    public void AddScore(int n)
    {
        score += n;
    }

    /// <summary>
    /// �ؔ��̎c�萔���擾
    /// </summary>
    /// <returns>�ؔ��̎c�萔</returns>
    public int GetWoodBoxNum()
    {
        return GameObject.FindGameObjectsWithTag("NormalObject").Length;
    }
}
