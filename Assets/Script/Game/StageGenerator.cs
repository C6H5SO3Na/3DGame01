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

    //�ʏ�I�u�W�F�N�g(��)
    [SerializeField] GameObject normalObjectPrefab;
    List<GameObject> normalObjects = new List<GameObject>();
    //���W�f�[�^
    List<Vector3> normalObjectData = new List<Vector3>();
    //���ɓ����Ă���A�C�e���̃f�[�^
    List<int> normalObjectItemData = new List<int>();

    //�d���I�u�W�F�N�g(��)
    [SerializeField] GameObject hardObjectPrefab;
    List<GameObject> hardObjects = new List<GameObject>();
    //���W�f�[�^
    List<Vector3> hardObjectData = new List<Vector3>();

    //�G
    [SerializeField] GameObject enemyPrefab;
    List<GameObject> enemies = new List<GameObject>();
    List<Vector3> enemyData = new List<Vector3>();

    //�v���C��
    [SerializeField] GameObject playerPrefab;
    Vector3 playerPos = new Vector3(0, 3, 0);

    // Start is called before the first frame update
    void Start()
    {
        SetPrefabInfo();
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
            for (int i = 0; i < normalObjectData.Count; ++i)
            {
                normalObjects.Add(normalObjectPrefab);
            }
            Vector3[] Rot = new Vector3[normalObjects.Count];
            Vector3[] Pos = new Vector3[normalObjects.Count];
            for (int i = 0; i < normalObjects.Count; ++i)
            {
                Rot[i] = Vector3.zero;
                if (GameDirector.stage >= 4)
                {
                    Pos[i] = new Vector3(Random.Range(-25, 25), 1.0f, Random.Range(-25, 25));
                }
                else
                {
                    Pos[i] = normalObjectData[i];
                }
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
            for (int i = 0; i < hardObjectData.Count; ++i)
            {
                hardObjects.Add(hardObjectPrefab);
            }
            Vector3[] Rot = new Vector3[hardObjects.Count];
            Vector3[] Pos = new Vector3[hardObjects.Count];
            for (int i = 0; i < hardObjects.Count; ++i)
            {
                Rot[i] = Vector3.zero;
                Pos[i] = hardObjectData[i];
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
            for (int i = 0; i < enemyData.Count; ++i)
            {
                enemies.Add(enemyPrefab);
            }
            Vector3[] Rot = new Vector3[enemies.Count];
            Vector3[] Pos = new Vector3[enemies.Count];
            for (int i = 0; i < enemies.Count; ++i)
            {
                Rot[i] = Vector3.zero;
                Pos[i] = enemyData[i];
            }
            SetPrefab(enemies, Rot, Pos);
        }

        //�v���C���̔z�u
        playerPrefab = Instantiate(playerPrefab, playerPos, Quaternion.identity);

    }

    //�v���t�@�u��z�u
    void SetPrefab(List<GameObject> ovjects, Vector3[] Rot, Vector3[] Pos)
    {
        for (int i = 0; i < ovjects.Count; ++i)
        {
            ovjects[i] = Instantiate(ovjects[i], Pos[i], Quaternion.Euler(Rot[i]));
            if (ovjects[i].CompareTag("NormalObject"))
            {
                ovjects[i].GetComponent<ObjectController>().hasItemNum = normalObjectItemData[i];
            }
        }
    }

    //�f�[�^�̒�`
    void SetPrefabInfo()
    {
        switch (GameDirector.stage)
        {
            case 1:
                normalObjectData = new List<Vector3>
                {
                    new Vector3(10.0f ,1.0f,10.0f),
                    new Vector3(0.0f  ,1.0f,10.0f),
                    new Vector3(-10.0f,1.0f,10.0f),
                };

                normalObjectItemData = new List<int>
                {
                    0,0,0
                };

                hardObjectData = new List<Vector3>
                {
                    new Vector3( 0.0f ,1.0f,20.0f),
               };
                break;

            case 2:
                normalObjectData = new List<Vector3>
                {
                    new Vector3(10.0f ,1.0f,10.0f),
                    new Vector3(20.0f ,1.0f,0.0f ),
                    new Vector3(10.0f ,1.0f,-10.0f),
                    new Vector3(-10.0f ,1.0f,10.0f),
                    new Vector3(-20.0f ,1.0f,0.0f ),
                    new Vector3(-10.0f ,1.0f,-10.0f),
                };

                normalObjectItemData = new List<int>
                {
                    1,1,2,1,1,1,
                };

                hardObjectData = new List<Vector3>
                {
                    new Vector3( 0.0f ,1.0f,20.0f),
                    new Vector3( 0.0f ,1.0f,-20.0f),
                };
                break;

            case 3:
                normalObjectData = new List<Vector3>
                {
                    new Vector3(-20.0f ,1.0f,20.0f),
                    new Vector3(-10.0f ,1.0f,20.0f),
                    new Vector3( 10.0f ,1.0f,20.0f),
                    new Vector3( 20.0f ,1.0f,20.0f),

                    new Vector3(-20.0f ,1.0f,10.0f),
                    new Vector3(-10.0f ,1.0f,10.0f),
                    new Vector3(  0.0f ,1.0f,10.0f),
                    new Vector3( 10.0f ,1.0f,10.0f),
                    new Vector3( 20.0f ,1.0f,10.0f),

                    new Vector3(-10.0f ,1.0f, 0.0f),
                    new Vector3( 10.0f ,1.0f, 0.0f),

                    new Vector3(-20.0f ,1.0f,-10.0f),
                    new Vector3(-10.0f ,1.0f,-10.0f),
                    new Vector3(  0.0f ,1.0f,-10.0f),
                    new Vector3( 10.0f ,1.0f,-10.0f),
                    new Vector3( 20.0f ,1.0f,-10.0f),

                    new Vector3(-20.0f ,1.0f,-20.0f),
                    new Vector3(-10.0f ,1.0f,-20.0f),
                    new Vector3( 10.0f ,1.0f,-20.0f),
                };

                normalObjectItemData = new List<int>
                {
                    0,0,  0,1,
                    0,0,0,0,0,
                      1,  1,  
                    0,0,1,0,0,
                    0,0,  0,
                };


                hardObjectData = new List<Vector3>
                {
                    new Vector3( 0.0f ,1.0f,20.0f),

                    new Vector3(-20.0f ,1.0f, 0.0f),
                    new Vector3( 20.0f ,1.0f, 0.0f),

                    new Vector3( 0.0f ,1.0f,-20.0f),
                };
                enemyData = new List<Vector3>
                {
                    new Vector3( 20.0f ,1.0f,-20.0f),
               };
                break;

            case 4:
                for (int i = 0; i < 10; ++i)
                {
                    normalObjectData.Add(new Vector3(Random.Range(-23, 23), 1.0f, Random.Range(-23, 23)));
                }

                normalObjectItemData = new List<int>
                {
                    0,0,1,0,0,0,0,0,0,0,
                };

                for (int i = 0; i < 10; ++i)
                {
                    hardObjectData.Add(new Vector3(Random.Range(-23, 23), 1.0f, Random.Range(-23, 23)));
                }

                for (int i = 0; i < 3; ++i)
                {
                    enemyData.Add(new Vector3(Random.Range(-23, 23), 1.0f, Random.Range(-23, 23)));
                }
                break;

            case 5:
                for (int i = 0; i < 20; ++i)
                {
                    normalObjectData.Add(new Vector3(Random.Range(-23, 23), 1.0f, Random.Range(-23, 23)));
                }

                normalObjectItemData = new List<int>
                {
                    0,0,1,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                };

                for (int i = 0; i < 20; ++i)
                {
                    hardObjectData.Add(new Vector3(Random.Range(-23, 23), 1.0f, Random.Range(-23, 23)));
                }

                for (int i = 0; i < 5; ++i)
                {
                    enemyData.Add(new Vector3(Random.Range(-23, 23), 1.0f, Random.Range(-23, 23)));
                }
                break;
        }
    }
}
