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
    public static int preScore = 0;//�R�[�X�J�n���̃X�R�A
    public static int phase;//�Q�[���̒i�K

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        aud = gameObject.GetComponent<AudioSource>();
        imageClear.gameObject.SetActive(false);
        //�X�e�[�W1�A���Ȃ킿�Q�[���J�n����BGM���Đ�
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

        //�i�K���Ƃ̏���
        switch (phase)
        {
            case 0://�t�F�[�h�C��
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
                //�d���I�u�W�F�N�g���S���Ȃ��Ȃ�����Q�[���N���A
                if (hardObjects.Length == 0)
                {
                    PlayerController.playerState = PlayerController.State.Clear;
                    imageClear.gameObject.SetActive(true);
                    ++phase;
                }

                operationStickText.text = "LStick:�ړ��@RStick:���_�ύX";
                //���e����
                if (PlayerController.getItemNum[3] > 0)
                {
                    operationText.text = "A:�e���ˁ@X:���e����";
                }
                else
                {
                    operationText.text = "A:�e����";
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
        scoreText.text = $"�X�R�A {GetScore()}";

        //�X�e�[�W���ɉ����Č�(���z)�̌�����ς���
        Vector3 lightAngle = directionalLight.transform.rotation.eulerAngles;
        lightAngle.x = 5 * (stage - 1);
        directionalLight.transform.rotation = Quaternion.Euler(lightAngle);
    }


    /// <summary>
    /// ���̃X�e�[�W�֑J��
    /// </summary>
    void ToNextStage()
    {
        if (PlayerController.playerState == PlayerController.State.Clear)
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
    /// �X�R�A�����Z
    /// </summary>
    /// <param name="n">���������X�R�A</param>
    public void AddScore(int n)
    {
        score += n;
    }

    /// <summary>
    /// ���݂̃X�R�A���擾
    /// </summary>
    /// <returns>���݂̃X�R�A</returns>
    public int GetScore()
    {
        return score;
    }
}