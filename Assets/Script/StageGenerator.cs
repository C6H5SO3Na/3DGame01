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
    List<GameObject> walls = new List<GameObject>();

    [SerializeField] GameObject floorPrefab;

    //床の位置
    Vector3 floorPos = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField] GameObject normalObjectPrefab;
    [SerializeField] int normalObjectNum;
    List<GameObject> normalObjects = new List<GameObject>();

    [SerializeField] GameObject hardObjectPrefab;
    [SerializeField] int hardObjectNum;
    List<GameObject> hardObjects = new List<GameObject>();

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int enemyNum;
    List<GameObject> enemies = new List<GameObject>();

    //プレイヤの位置
    [SerializeField] GameObject playerPrefab;
    Vector3 playerPos = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        //壁オブジェクトの設定
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

        //床オブジェクトを取得
        floorPrefab = Instantiate(floorPrefab);
        floorPrefab.transform.position = floorPos;

        //通常オブジェクトの設定
        {
            for (int i = 0; i < normalObjectNum; ++i)
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

        //硬いオブジェクトの設定
        {
            for (int i = 0; i < hardObjectNum; ++i)
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

        //敵の設定
        {
            for (int i = 0; i < enemyNum; ++i)
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
            ovjects[i] = Instantiate(ovjects[i]);
            ovjects[i].transform.eulerAngles = Rot[i];
            ovjects[i].transform.position = Pos[i];
        }
    }
}
