using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //アイテムを回転させる
        transform.Rotate(0, 1, 0);
    }
}
