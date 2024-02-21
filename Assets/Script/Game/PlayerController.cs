using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.Assertions.Must;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] GameObject shotPrefab;
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] float gravity;
    [SerializeField] ParticleSystem ps;
    Animator animator;
    GameObject unityChan;

    public AudioSource aud;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip clear;
    [SerializeField] AudioClip weapon;

    GameDirector gameDirector;

    public static float speed = 4.0f;
    public static int[] getItemNum = new int[4]; //0:None 1:Speed 2:RapidFire 3:Weapon
    Vector3 moveDirection;
    Vector2 playerDirection;
    Quaternion defaultCameraDirection;
    Vector3 defaultCameraOffset;
    public static bool isRapidFire = false;
    public static bool isAbleMultiShot = false;
    int mainCnt = 0;
    bool isClear = false;//クリア時のボイスを1回しか流さないようにする

    public enum State
    {
        Normal, Clear, Dead
    }
    public static State playerState;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        unityChan = transform.GetChild(0).gameObject;
        animator = unityChan.GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        playerState = State.Normal;
        defaultCameraDirection = Camera.main.transform.rotation;
        defaultCameraOffset = Camera.main.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null) { return; }
        if (playerState == State.Normal)
        {
            animator.SetBool("Run", Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
            //プレイヤの上下左右移動
            if (animator.GetBool("Run"))
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
            //Debug.Log(Camera.main.transform.localEulerAngles);

            playerDirection.x += Input.GetAxis("Horizontal_R");
            playerDirection.y -= Input.GetAxis("Vertical_R");

            //カメラの上下を制限
            playerDirection.y = Mathf.Clamp(playerDirection.y, -10.0f, 45.0f);

            //カメラを回転
            Camera.main.transform.rotation = Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraDirection;
            Camera.main.transform.position = transform.position + Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraOffset;

            //Camera.main.transform.RotateAround(transform.position, transform.up, lotateAngle.x);
            //Camera.main.transform.RotateAround(transform.position, Camera.main.transform.right, lotateAngle.y);

            if (Input.GetKeyDown(KeyCode.Z)) { }
            //着地判定
            if (controller.isGrounded)
            {
                animator.SetBool("Jump", false);
                //ジャンプ
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = 5.0f;
                    animator.SetBool("Jump", true);
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

                gameDirector.aud.PlayOneShot(gameDirector.shootSE);//3つ発射しても1回しか鳴らさない(音量の都合)
                //マルチショットが有効になっている場合、3つの弾を発射
                for (int i = 0; i < shotNum; ++i)
                {
                    GameObject shot = Instantiate(shotPrefab);
                    float vec;
                    if(i == 2)
                    {
                        vec = -1 * 0.5f;
                    }
                    else
                    {
                        vec = i * 0.5f;
                    }
                    Ray ray = new Ray(transform.position + transform.forward * 0.5f,
                        transform.forward + transform.up * 0.15f +transform.right * vec);//真ん中よりやや上をめがけて発射
                    //Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, (float)Screen.height / 2f));//真ん中よりやや上をめがけて発射
                    Vector3 worldDirection = ray.direction;
                    shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 800.0f);

                    //移動した位置から弾発射
                    shot.transform.position = Camera.main.transform.position
                        + Camera.main.transform.forward * 7.0f + difference[i];

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
            if (Input.GetButtonDown("Fire2") && getItemNum[3] != 0)
            {
                aud.PlayOneShot(weapon);
                --getItemNum[3];
                GameObject shot = Instantiate(weaponPrefab);
                Ray ray = new Ray(transform.position + transform.forward * 0.5f,
                    transform.forward + transform.up * 0.15f);//真ん中よりやや上をめがけて発射
                //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, (float)Screen.height / 1.2f, 1));//真ん中よりやや上をめがけて発射
                Vector3 worldDirection = ray.direction;
                shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

                //移動した位置から弾発射
                shot.transform.position = Camera.main.transform.position
                    + Camera.main.transform.forward * 7.0f;
            }
        }
        else
        {
            moveDirection.x = moveDirection.z = 0;//プレイしていないときはxzの移動量を0にする
            if (playerState == State.Clear)
            {
                animator.SetTrigger("Clear");
                transform.rotation = Quaternion.Euler(0, playerDirection.x + 180.0f, 0);
                if (!isClear)
                {
                    aud.PlayOneShot(clear);
                    isClear = true;
                }
            }
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
                gameDirector.aud.PlayOneShot(gameDirector.itemGetSE);
                ++getItemNum[0];
                GameDirector.score += 1;
                speed *= 1.2f;
                break;
            case "RapidFireItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//もう一回この関数が呼び出されるためタグを変更して呼び出しを回避
                gameDirector.aud.PlayOneShot(gameDirector.itemGetSE);
                ++getItemNum[1];
                GameDirector.score += 2;
                isRapidFire = true;
                break;
            case "MultiShotItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//もう一回この関数が呼び出されるためタグを変更して呼び出しを回避
                gameDirector.aud.PlayOneShot(gameDirector.itemGetSE);
                ++getItemNum[2];
                GameDirector.score += 2;
                isAbleMultiShot = true;
                break;
            case "WeaponItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//もう一回この関数が呼び出されるためタグを変更して呼び出しを回避
                gameDirector.aud.PlayOneShot(gameDirector.itemGetSE);
                ++getItemNum[3];
                GameDirector.score += 5;
                break;
        }
    }
    void OnTriggerEnter(Collider hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Enemy":
                animator.SetTrigger("Dead");
                playerState = State.Dead;
                gameDirector.aud.PlayOneShot(gameDirector.damageSE);
                aud.PlayOneShot(dead);
                break;
        }
    }


    bool RapidFireOperation()
    {
        return isRapidFire && Input.GetButton("Fire1") && mainCnt % 8 == 0;
    }
}
