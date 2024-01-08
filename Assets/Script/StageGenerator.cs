using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] GameObject wall;
    GameObject[] wallPrefab = new GameObject[4];

    //壁の角度
    Vector3[] wallRot = new Vector3[]
    {
        new Vector3( 0.0f, 0.0f, 0.0f ),
        new Vector3( 0.0f, 0.0f, 0.0f ),
        new Vector3( 0.0f, 90.0f, 0.0f ),
        new Vector3( 0.0f, 90.0f, 0.0f )
    };

    //壁の位置
    Vector3[] wallPos = new Vector3[]
    {
        new Vector3( 0.0f, 5.0f, -25.05f ),
        new Vector3( 0.0f, 5.0f, 25.05f ),
        new Vector3(-25.05f, 5.0f, 0.0f ),
        new Vector3( 25.05f, 5.0f, 0.0f )
    };
    // Start is called before the first frame update
    void Start()
    {
        //壁オブジェクトを取得
        for (int i = 0; i < wallPrefab.Length; ++i)
        {
            wallPrefab[i] = Instantiate(wall);
            wallPrefab[i].transform.eulerAngles = wallRot[i];
            wallPrefab[i].transform.position = wallPos[i];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
