using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image fade;
    public bool isFade;
    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<Image>();
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void Fadein()
    {
        Color tmp = fade.color;
        tmp.a -= 1.0f * Time.deltaTime;
        tmp.a = Mathf.Clamp01(tmp.a);
        fade.color = tmp;
        isFade = fade.color.a > 0.0f;
    }
    /// <summary>
    /// フェードアウト
    /// </summary>
    public void Fadeout()
    {
        Color tmp = fade.color;
        tmp.a += 1.0f * Time.deltaTime;
        fade.color = tmp;
        isFade = fade.color.a < 1.0f;
    }
}
