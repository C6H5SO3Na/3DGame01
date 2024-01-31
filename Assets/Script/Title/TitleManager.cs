using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] AudioClip titleVoice;
    [SerializeField] AudioClip buttonSE;
    bool isPressed = false;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();

        //�N���A���Ƀ��Z�b�g
        GameDirector.stage = 1;
        GameDirector.score = 0;
        GameDirector.preScore = 0;//�R�[�X�J�n���̃X�R�A
        BGMPlayer.isCreated = false;

        PlayerController.isRapidFire = false;
        PlayerController.isAbleMultiShot = false;
        //�A�C�e���̎擾��������
        for (int i = 0; i < PlayerController.getItemNum.Length; ++i)
        {
            PlayerController.getItemNum[i] = 0;
        }
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
        else if (Input.GetButtonDown("Fire1") && !isPressed)
        {
            aud.PlayOneShot(titleVoice);
            aud.PlayOneShot(buttonSE);
            Invoke("ToGame", 2.0f);
            isPressed = true;
        }
    }

    void ToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
