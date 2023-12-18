using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraContoroller : MonoBehaviour
{
    GameObject player;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        direction = this.transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + direction;
    }
}
