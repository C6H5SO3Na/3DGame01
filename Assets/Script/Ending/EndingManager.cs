using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    bool isPressed = false;
    [SerializeField] AudioClip endingVoice;
    [SerializeField] AudioClip buttonSE;
    [SerializeField] Image fade;
    [SerializeField] Image pressAButton;
    float mainCnt = 0.0f;
    bool hasFadein;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.PlayOneShot(endingVoice);
        mainCnt = 0.0f;
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
        //�t�F�[�h�C�����I����Ă��瑀��ł���
        if (!hasFadein)
        {
            fade.GetComponent<Fade>().Fadein();
            if (fade.GetComponent<Fade>().isFade) { return; }
        }

        hasFadein = true;
        if (Input.GetButtonDown("Fire1") && !isPressed)
        {
            aud.PlayOneShot(buttonSE);
            isPressed = true;
            //�{�^�����������Ɣ���
            pressAButton.GetComponent<PressAButtonController>().changeAmount = 8.0f;
        }

        if (isPressed)
        {
            ToTitleScene();
        }
    }

    /// <summary>
    /// �^�C�g����ʂ֐��ڂ��鏈��
    /// </summary>
    void ToTitleScene()
    {
        mainCnt += Time.deltaTime;
        if (mainCnt < 2.0f) { return; }
        fade.GetComponent<Fade>().Fadeout();
        if (!fade.GetComponent<Fade>().isFade)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
