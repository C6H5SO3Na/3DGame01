using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushAButtonController : MonoBehaviour
{
    Image image;
    float alpha;
    float changeAmount;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        alpha = 1.0f;
        changeAmount = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        alpha += changeAmount * Time.deltaTime;
        if (alpha < 0.0f)
        {
            changeAmount = 1.0f;
        }
        else if (alpha > 1.0f)
        {
            changeAmount = -1.0f;
        }
        image.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }
}
