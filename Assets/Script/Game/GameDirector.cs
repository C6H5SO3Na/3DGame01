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
    [SerializeField] GameObject player;
    [SerializeField] Light directionalLight;
    [SerializeField] int maxStage;
    [SerializeField] Image image;

    public AudioClip damageSE;
    public AudioClip shootSE;
    public AudioClip explosionSE;
    public AudioClip itemGetSE;
    public AudioSource aud;

    public static int stage = 1;
    public static int score = 0;
    public static int preScore;//�R�[�X�J�n���̃X�R�A
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        preScore = 0;
        aud = gameObject.GetComponent<AudioSource>();
        image.gameObject.SetActive(false);
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
        //�d���I�u�W�F�N�g���S���Ȃ��Ȃ�����Q�[���N���A
        if (hardObjects.Length == 0)
        {
            if (PlayerController.playerState != PlayerController.State.Clear)
            {
                PlayerController.playerState = PlayerController.State.Clear;
                Invoke("ToNextStage", 2.0f);
                image.gameObject.SetActive(true);
            }
        }

        //�v���C�������ꂽ��A�����R�[�X����蒼��
        if (PlayerController.playerState == PlayerController.State.Dead)
        {
            Invoke("ToNextStage", 2.0f);
        }
        weaponText.text = $"x {PlayerController.getItemNum[3]}";
        hardObjectsText.text = $"x {hardObjects.Length}";
        scoreText.text = $"�X�R�A {GetScore()}";

        //�X�e�[�W���ɉ����Č�(���z)�̌�����ς���
        Vector3 lightAngle = directionalLight.transform.rotation.eulerAngles;
        lightAngle.x = 5 * (stage - 1);
        directionalLight.transform.rotation = Quaternion.Euler(lightAngle);
    }

    void ToNextStage()
    {
        if (PlayerController.playerState == PlayerController.State.Clear)
        {
            ++stage;
            preScore = score;//�X�R�A��ۑ�
            if (stage > maxStage)
            {
                SceneManager.LoadScene("EndingScene");
            }
        }
        else
        {
            score = preScore;//���ꂽ�Ƃ��͑O�̃X�R�A�ɖ߂�
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
