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

    //�ǂ̊p�x
    Vector3[] wallRot = new Vector3[]
    {
        new Vector3( 0.0f, 0.0f, 0.0f ),
        new Vector3( 0.0f, 0.0f, 0.0f ),
        new Vector3( 0.0f, 90.0f, 0.0f ),
        new Vector3( 0.0f, 90.0f, 0.0f )
    };

    //�ǂ̈ʒu
    Vector3[] wallPos = new Vector3[]
    {
        new Vector3( 0.0f, 5.0f, -25.05f ),
        new Vector3( 0.0f, 5.0f, 25.05f ),
        new Vector3(-25.05f, 5.0f, 0.0f ),
        new Vector3( 25.05f, 5.0f, 0.0f )
    };

    [SerializeField] GameObject floorPrefab;


    //���̈ʒu
    Vector3 floorPos = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField] GameObject normalObjectPrefab;
    GameObject[] normalObjects = new GameObject[100];

    [SerializeField] GameObject hardObjectPrefab;
    GameObject[] hardObjects = new GameObject[100];

    [SerializeField] GameObject enemyPrefab;
    GameObject[] enemies = new GameObject[3];

    //�v���C���̈ʒu
    [SerializeField] GameObject playerPrefab;
    Vector3 playerPos = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        //�ǃI�u�W�F�N�g���擾
        for (int i = 0; i < walls.Length; ++i)
        {
            walls[i] = Instantiate(wallPrefab);
            walls[i].transform.eulerAngles = wallRot[i];
            walls[i].transform.position = wallPos[i];
        }

        //���I�u�W�F�N�g���擾
        floorPrefab = Instantiate(floorPrefab);
        floorPrefab.transform.position = floorPos;

        //�ʏ�I�u�W�F�N�g���擾
        for (int i = 0; i < normalObjects.Length; ++i)
        {
            normalObjects[i] = Instantiate(normalObjectPrefab);
            normalObjects[i].transform.eulerAngles = new Vector3();
            normalObjects[i].transform.position = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
        }

        //�d���I�u�W�F�N�g���擾
        for (int i = 0; i < hardObjects.Length; ++i)
        {
            hardObjects[i] = Instantiate(hardObjectPrefab);
            hardObjects[i].transform.eulerAngles = new Vector3();
            hardObjects[i].transform.position = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
        }

        //�d���I�u�W�F�N�g���擾
        for (int i = 0; i < enemies.Length; ++i)
        {
            enemies[i] = Instantiate(enemyPrefab);
            enemies[i].transform.eulerAngles = new Vector3();
            enemies[i].transform.position = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
        }

        playerPrefab = Instantiate(playerPrefab);
        playerPrefab.transform.position = playerPos;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
