using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StageGenerator : MonoBehaviour
{
    //struct Prefab
    //{
    //    public GameObject ovject;//object
    //    List<GameObject> ovjects;
    //}
    //Prefab[] walls = new Prefab[4];

    //��
    [SerializeField] GameObject wallPrefab;
    List<GameObject> walls = new List<GameObject>();

    //��
    [SerializeField] GameObject floorPrefab;
    Vector3 floorPos = Vector3.zero;

    //�ʏ�I�u�W�F�N�g
    [SerializeField] GameObject normalObjectPrefab;
    int[] normalObjectNum = { 3, 4, 19, 20, 50 };
    List<GameObject> normalObjects = new List<GameObject>();

    Dictionary<int, string> take;
    //List<List<Vector3>> normalObjectData = new Vector3[,]
    //{
    //    //{
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //},
    //    //{
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //},
    //    //{
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //},
    //    //{
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //    new Vector3(0, 0, 0),
    //    //},
    //};

    //�d���I�u�W�F�N�g
    [SerializeField] GameObject hardObjectPrefab;
    int[] hardObjectNum = { 1, 2, 4, 20, 50 };
    List<GameObject> hardObjects = new List<GameObject>();

    //�G
    [SerializeField] GameObject enemyPrefab;
    int[] enemyNum = { 1, 0, 1, 2, 3 };
    List<GameObject> enemies = new List<GameObject>();

    //�v���C��
    [SerializeField] GameObject playerPrefab;
    Vector3 playerPos = new Vector3(0, 3, 0);

    // Start is called before the first frame update
    void Start()
    {
        //�ǃI�u�W�F�N�g�̐ݒ�
        {
            for (int i = 0; i < 4; ++i)
            {
                walls.Add(wallPrefab);
            }
            Vector3[] Rot = new Vector3[]{
                new Vector3( 0.0f, 0.0f, 0.0f ),
                new Vector3( 0.0f, 0.0f, 0.0f ),
                new Vector3( 0.0f, 90.0f, 0.0f ),
                new Vector3( 0.0f, 90.0f, 0.0f )
            };

            Vector3[] Pos = new Vector3[]{
                new Vector3( 0.0f, 5.0f, -25.05f ),
                new Vector3( 0.0f, 5.0f, 25.05f ),
                new Vector3(-25.05f, 5.0f, 0.0f ),
                new Vector3( 25.05f, 5.0f, 0.0f )
            };
            SetPrefab(walls, Rot, Pos);
        }

        //���I�u�W�F�N�g�̐ݒ�
        floorPrefab = Instantiate(floorPrefab);
        floorPrefab.transform.position = floorPos;

        //�ʏ�I�u�W�F�N�g�̐ݒ�
        {
            for (int i = 0; i < normalObjectNum[GameDirector.stage - 1]; ++i)
            {
                normalObjects.Add(normalObjectPrefab);
            }
            Vector3[] Rot = new Vector3[normalObjects.Count];
            Vector3[] Pos = new Vector3[normalObjects.Count];
            for (int i = 0; i < normalObjects.Count; ++i)
            {
                Rot[i] = Vector3.zero;
                Pos[i] = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
            }
            SetPrefab(normalObjects, Rot, Pos);
        }
        //for (int i = 0; i < normalObjects.Length; ++i)
        //{
        //    normalObjects[i] = Instantiate(normalObjectPrefab);
        //    normalObjects[i].transform.eulerAngles = new Vector3();
        //    normalObjects[i].transform.position = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
        //    normalObjects[i].GetComponent<ObjectController>().hasItem = (i % 3 == 0);
        //}

        //�d���I�u�W�F�N�g�̐ݒ�
        {
            for (int i = 0; i < hardObjectNum[GameDirector.stage - 1]; ++i)
            {
                hardObjects.Add(hardObjectPrefab);
            }
            Vector3[] Rot = new Vector3[hardObjects.Count];
            Vector3[] Pos = new Vector3[hardObjects.Count];
            for (int i = 0; i < hardObjects.Count; ++i)
            {
                Rot[i] = Vector3.zero;
                Pos[i] = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
            }
            SetPrefab(hardObjects, Rot, Pos);
        }
        //for (int i = 0; i < hardObjects.Length; ++i)
        //{
        //    hardObjects[i] = Instantiate(hardObjectPrefab);
        //    hardObjects[i].transform.eulerAngles = new Vector3();
        //    hardObjects[i].transform.position = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
        //}

        //�G�̐ݒ�
        {
            for (int i = 0; i < enemyNum[GameDirector.stage - 1]; ++i)
            {
                enemies.Add(enemyPrefab);
            }
            Vector3[] Rot = new Vector3[enemies.Count];
            Vector3[] Pos = new Vector3[enemies.Count];
            for (int i = 0; i < enemies.Count; ++i)
            {
                Rot[i] = Vector3.zero;
                Pos[i] = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
            }
            SetPrefab(enemies, Rot, Pos);
        }

        //�v���C���̐ݒ�
        playerPrefab = Instantiate(playerPrefab);
        playerPrefab.transform.position = playerPos;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetPrefab(List<GameObject> ovjects, Vector3[] Rot, Vector3[] Pos)
    {
        for (int i = 0; i < ovjects.Count; ++i)
        {
            ovjects[i] = Instantiate(ovjects[i], Pos[i], Quaternion.Euler(Rot[i]));
            if (ovjects[i].tag.Contains("Object"))
            {
                ovjects[i].GetComponent<ObjectController>().hasItemNum = 2;
            }
        }
    }
}
