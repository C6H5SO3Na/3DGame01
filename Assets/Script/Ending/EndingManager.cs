using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    bool isPressed = false;
    [SerializeField] AudioClip endingVoice;
    [SerializeField] AudioClip buttonSE;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.PlayOneShot(endingVoice);
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
            aud.PlayOneShot(buttonSE);
            Invoke("ToTitle", 2.0f);
            isPressed = true;
        }
    }

    void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
