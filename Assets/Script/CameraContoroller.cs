using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoroller : MonoBehaviour
{
    int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Camera.main.transform.rotation = Quaternion.Euler(1, cnt, 1);
        cnt++;
    }
}
