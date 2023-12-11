using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    int mainCnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        player = GameObject.Find("Player");
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        if (mainCnt >= 10.0f * fps)
        {
            Destroy(gameObject);
        }
        ++mainCnt;
    }

    //’e‚ð”­ŽË
    public void Shoot(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction);
    }
}
