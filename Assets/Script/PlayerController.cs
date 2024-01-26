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
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject shotPrefab;
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] float gravity;
    [SerializeField] ParticleSystem ps;

    float speed;
    public static int[] getItemNum = new int[4];
    Vector3 moveDirection;
    Vector2 playerDirection;
    Quaternion defaultCameraDirection;
    Vector3 defaultCameraOffset;
    bool isRapidFire = false;
    bool isAbleMultiShot = false;
    int mainCnt = 0;

    public enum State
    {
        Normal, Clear, Dead
    }
    public static State playerState;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = 3.0f;

        //�A�C�e���̎擾��������
        for (int i = 0; i < getItemNum.Length; ++i)
        {
            getItemNum[i] = 0;
        }
        getItemNum[3] = 4;
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
            //�v���C���̏㉺���E�ړ�
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
                moveDirection.x = moveDirection.z = 0;//�������͂���Ă��Ȃ��Ƃ���xz�̈ړ��ʂ�0�ɂ���
            }

            //angle += lotateAngle;

            //�v���C���̎��_�ύX
            //Debug.Log(Camera.main.transform.localEulerAngles);

            playerDirection.x += Input.GetAxis("Horizontal_R");
            playerDirection.y -= Input.GetAxis("Vertical_R");

            //�J�����̏㉺�𐧌�
            playerDirection.y = Mathf.Clamp(playerDirection.y, -10.0f, 45.0f);

            Camera.main.transform.rotation = Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraDirection;
            Camera.main.transform.position = transform.position + Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraOffset;

            //�J��������]
            //Camera.main.transform.RotateAround(transform.position, transform.up, lotateAngle.x);
            //Camera.main.transform.RotateAround(transform.position, Camera.main.transform.right, lotateAngle.y);

            //���n����
            if (controller.isGrounded)
            {
                //�W�����v
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = 5.0f;
                }
            }
            //�e����
            if (Input.GetButtonDown("Fire1") || RapidFireOperation())
            {
                int shotNum = 1;
                if (isAbleMultiShot)
                {
                    shotNum = 3;
                }
                //����
                Vector3[] difference = {
                    Vector3.zero,
                    Camera.main.transform.right * 0.5f,
                    Camera.main.transform.right * -0.5f
                };
                //�}���`�V���b�g���L���ɂȂ��Ă���ꍇ�A3�̒e�𔭎�
                for (int i = 0; i < shotNum; ++i)
                {
                    GameObject shot = Instantiate(shotPrefab);
                    Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, (float)Screen.height / 2f));//�^�񒆂�������߂����Ĕ���
                    Vector3 worldDirection = ray.direction;
                    shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 800.0f);

                    //�ړ������ʒu����e����
                    shot.transform.position = Camera.main.transform.position
                        + Camera.main.transform.forward * 6.0f + difference[i];
                }
            }
            //�}�E�X����
            //if (Input.GetMouseButtonDown(0))
            //{
            //    GameObject shot = Instantiate(shotPrefab);
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    Vector3 worldDirection = ray.direction;
            //    shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

            //    //�ړ������ʒu����e����
            //    shot.transform.position = Camera.main.transform.position
            //        + Camera.main.transform.forward * 5.5f;
            //}

            //���͒e����
            if (Input.GetButtonDown("Fire2") && getItemNum[3] != 0)
            {
                ++getItemNum[3];
                GameObject shot = Instantiate(weaponPrefab);
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, (float)Screen.height / 1.2f, 1));//�^�񒆂�������߂����Ĕ���
                Vector3 worldDirection = ray.direction;
                shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

                //�ړ������ʒu����e����
                shot.transform.position = Camera.main.transform.position
                    + Camera.main.transform.forward * 6.0f;
            }
        }
        else
        {
            moveDirection.x = moveDirection.z = 0;//�v���C���Ă��Ȃ��Ƃ���xz�̈ړ��ʂ�0�ɂ���
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
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                ++getItemNum[0];
                GameDirector.score += 1;
                speed *= 1.05f;
                break;
            case "RapidFireItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                ++getItemNum[1];
                GameDirector.score += 2;
                isRapidFire = true;
                break;
            case "MultiShotItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                ++getItemNum[2];
                GameDirector.score += 2;
                isAbleMultiShot = true;
                break;
            case "WeaponItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                ++getItemNum[3];
                weapon.GetComponent<WeaponNumManager>().SetSprite();
                GameDirector.score += 5;
                break;
        }
    }
    void OnTriggerEnter(Collider hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Enemy":
                Instantiate(ps, this.transform.position, Quaternion.identity);
                playerState = State.Dead;
                Destroy(gameObject);
                break;
        }
    }


    bool RapidFireOperation()
    {
        return isRapidFire && Input.GetButton("Fire1") && mainCnt % 8 == 0;
    }
}
