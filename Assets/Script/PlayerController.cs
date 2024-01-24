using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] GameObject shotPrefab;
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] float gravity;
    [SerializeField] ParticleSystem ps;

    float speed;
    public static int[] getItemNum = new int[4];
    Vector2 angle;
    Vector3 moveDirection;
    Vector2 playerDirection;
    Quaternion defaultCameraDirection;
    Vector3 defaultCameraOffset;
    bool isRapidFire = false;
    bool isAbleMultiShot = false;
    int mainCnt = 0;

    //カメラに制限を掛けるにはどうすればよいか?

    public enum State
    {
        Normal, Clear,//プレイ中とクリア時
    }
    State state;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = 3.0f;

        //アイテムの取得数初期化
        for (int i = 0; i < getItemNum.Length; ++i)
        {
            getItemNum[i] = 0;
        }
        state = State.Normal;
        defaultCameraDirection = Camera.main.transform.rotation;
        defaultCameraOffset = Camera.main.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null) { return; }
        if (state == State.Normal)
        {
            //プレイヤの上下左右移動
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                if (inputVector.magnitude > 1.0f)
                {
                    inputVector = inputVector.normalized;
                }
                moveDirection.x = speed * inputVector.x;
                moveDirection.z = speed * inputVector.z;

                float Dir = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, playerDirection.x + Dir, 0);
            }
            else
            {
                moveDirection.x = moveDirection.z = 0;//何も入力されていないときはxzの移動量を0にする
            }

            //angle += lotateAngle;

            //プレイヤの視点変更
            Debug.Log(Camera.main.transform.localEulerAngles);

            playerDirection.x += Input.GetAxis("Horizontal_R");
            playerDirection.y -= Input.GetAxis("Vertical_R");

            Camera.main.transform.rotation = Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraDirection;
            Camera.main.transform.position = transform.position + Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraOffset;

            //カメラを回転
            //Camera.main.transform.RotateAround(transform.position, transform.up, lotateAngle.x);
            //Camera.main.transform.RotateAround(transform.position, Camera.main.transform.right, lotateAngle.y);

            //着地判定
            if (controller.isGrounded)
            {
                //ジャンプ
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = 5.0f;
                }
            }
            //弾発射
            if (Input.GetButtonDown("Fire1") || RapidFireOperation())
            {
                int shotNum = 1;
                if (isAbleMultiShot)
                {
                    shotNum = 3;
                }
                //距離
                Vector3[] difference = {
                    Vector3.zero,
                    Camera.main.transform.right * 0.5f,
                    Camera.main.transform.right * -0.5f
                };
                for (int i = 0; i < shotNum; ++i)
                {
                    GameObject shot = Instantiate(shotPrefab);
                    Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, (float)Screen.height / 2f));//真ん中よりやや上をめがけて発射
                    Vector3 worldDirection = ray.direction;
                    shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 500.0f);

                    //移動した位置から弾発射
                    shot.transform.position = Camera.main.transform.position
                        + Camera.main.transform.forward * 6.0f + difference[i];
                }
            }
            //マウス時代
            //if (Input.GetMouseButtonDown(0))
            //{
            //    GameObject shot = Instantiate(shotPrefab);
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    Vector3 worldDirection = ray.direction;
            //    shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

            //    //移動した位置から弾発射
            //    shot.transform.position = Camera.main.transform.position
            //        + Camera.main.transform.forward * 5.5f;
            //}

            //強力弾発射
            if (Input.GetButtonDown("Fire2") && getItemNum[0] != 0)
            {
                --getItemNum[0];
                GameObject shot = Instantiate(weaponPrefab);
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, (float)Screen.height / 1.2f, 1));//真ん中よりやや上をめがけて発射
                Vector3 worldDirection = ray.direction;
                shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

                //移動した位置から弾発射
                shot.transform.position = Camera.main.transform.position
                    + Camera.main.transform.forward * 6.0f;
            }
        }
        else
        {
            moveDirection.x = moveDirection.z = 0;//プレイしていないときはxzの移動量を0にする
        }

        moveDirection.y -= gravity * Time.deltaTime;

        Vector3 globalDirection = Quaternion.Euler(0, playerDirection.x, 0) * moveDirection;
        controller.Move(globalDirection * Time.deltaTime);
        //moveDirection = Vector3.zero;
        //charaCon.Move(moveDirection * Time.deltaTime);

        ++mainCnt;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "SpeedItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//もう一回この関数が呼び出されるためタグを変更して呼び出しを回避
                ++getItemNum[0];
                GameDirector.score += 1;
                speed *= 1.05f;
                break;
            case "RapidFireItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//もう一回この関数が呼び出されるためタグを変更して呼び出しを回避
                ++getItemNum[1];
                GameDirector.score += 2;
                isRapidFire = true;
                break;
            case "MultiShotItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//もう一回この関数が呼び出されるためタグを変更して呼び出しを回避
                ++getItemNum[2];
                GameDirector.score += 2;
                isAbleMultiShot = true;
                break;
            case "WeaponItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//もう一回この関数が呼び出されるためタグを変更して呼び出しを回避
                ++getItemNum[3];
                GameDirector.score += 5;
                break;
            case "Enemy":
                Instantiate(ps, this.transform.position, Quaternion.identity);
                break;
        }
    }

    public void SetState(State s_)
    {
        state = s_;
    }

    public State GetState()
    {
        return state;
    }

    bool RapidFireOperation()
    {
        return isRapidFire && Input.GetButton("Fire1") && mainCnt % 8 == 0;
    }
}
