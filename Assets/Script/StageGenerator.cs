using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StageGenerator : MonoBehaviour
{
    //struct PrefabInfo
    //{
    //    Vector3 Rot;
    //    Vector3 Pos;
    //}

    //struct Prefab
    //{
    //    public GameObject ovject;//object
    //    PrefabInfo info;
    //}
    //Prefab[] walls = new Prefab[4];

    [SerializeField] GameObject wallPrefab;
    GameObject[] walls = new GameObject[4];

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

    [SerializeField] GameObject normalObjectPrefab;
    GameObject[] normalObjects = new GameObject[100];

    //壁の角度
    //Vector3[] normalRot = new Vector3[5];

    //壁の位置
    //Vector3[] normalPos = new Vector3[5];
    [SerializeField] GameObject hardObjectPrefab;
    GameObject[] hardObjects = new GameObject[100];
    // Start is called before the first frame update
    void Start()
    {
        //壁オブジェクトを取得
        for (int i = 0; i < walls.Length; ++i)
        {
            walls[i] = Instantiate(wallPrefab);
            walls[i].transform.eulerAngles = wallRot[i];
            walls[i].transform.position = wallPos[i];
        }

        //通常オブジェクトを取得
        for (int i = 0; i < normalObjects.Length; ++i)
        {
            normalObjects[i] = Instantiate(normalObjectPrefab);
            normalObjects[i].transform.eulerAngles = new Vector3();
            normalObjects[i].transform.position = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
        }

        //硬いオブジェクトを取得
        for (int i = 0; i < hardObjects.Length; ++i)
        {
            hardObjects[i] = Instantiate(hardObjectPrefab);
            hardObjects[i].transform.eulerAngles = new Vector3();
            hardObjects[i].transform.position = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
