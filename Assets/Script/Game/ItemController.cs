using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //アイテムを回転させる
        transform.Rotate(0.0f, 1.0f, 0.0f);
    }
}
