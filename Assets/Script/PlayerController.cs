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
    CharacterController charaCon;
    [SerializeField] GameObject shotPrefab;
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] float gravity;
    [SerializeField] ParticleSystem ps;

    float speed;
    public static int[] getItemNum = new int[2];
    Vector2 angle;
    Vector3 moveDirection;
    Vector2 playerDirection;
    Quaternion defaultCameraDirection;
    Vector3 defaultCameraOffset;

    //�J�����ɐ������|����ɂ͂ǂ�����΂悢��?

    public enum State
    {
        Normal, Clear,//�v���C���ƃN���A��
    }
    State state;
    // Start is called before the first frame update
    void Start()
    {
        charaCon = GetComponent<CharacterController>();
        speed = 3.0f;
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
        if (charaCon == null) { return; }
        if (state == State.Normal)
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
            Debug.Log(Camera.main.transform.localEulerAngles);

            playerDirection.x += Input.GetAxis("Horizontal_R");
            playerDirection.y -= Input.GetAxis("Vertical_R");

            Camera.main.transform.rotation = Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraDirection;
            Camera.main.transform.position = transform.position + Quaternion.Euler(playerDirection.y, playerDirection.x, 0) * defaultCameraOffset;

            //�J��������]
            //Camera.main.transform.RotateAround(transform.position, transform.up, lotateAngle.x);
            //Camera.main.transform.RotateAround(transform.position, Camera.main.transform.right, lotateAngle.y);

            //���n����
            if (charaCon.isGrounded)
            {
                //�W�����v
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = 5.0f;
                }
            }
            //�e����
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject shot = Instantiate(shotPrefab);
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, (float)Screen.height / 1.2f, 1));//�^�񒆂�������߂����Ĕ���
                Vector3 worldDirection = ray.direction;
                shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

                //�ړ������ʒu����e����
                shot.transform.position = Camera.main.transform.position
                    + Camera.main.transform.forward * 8.0f;
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
            if (Input.GetButtonDown("Fire2") && getItemNum[0] != 0)
            {
                --getItemNum[0];
                GameObject shot = Instantiate(weaponPrefab);
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, (float)Screen.height / 1.2f, 1));//�^�񒆂�������߂����Ĕ���
                Vector3 worldDirection = ray.direction;
                shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

                //�ړ������ʒu����e����
                shot.transform.position = Camera.main.transform.position
                    + Camera.main.transform.forward * 8.0f;
            }
        }
        else
        {
            moveDirection.x = moveDirection.z = 0;//�v���C���Ă��Ȃ��Ƃ���xz�̈ړ��ʂ�0�ɂ���
        }

        moveDirection.y -= gravity * Time.deltaTime;

        Vector3 globalDirection = Quaternion.Euler(0, playerDirection.x, 0) * moveDirection;
        charaCon.Move(globalDirection * Time.deltaTime);
        //moveDirection = Vector3.zero;
        //charaCon.Move(moveDirection * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "WeaponItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                ++getItemNum[0];
                GameDirector.score += 5;
                break;
            case "SpeedItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                ++getItemNum[1];
                GameDirector.score += 2;
                speed *= 1.05f;
                break;
            case "Enemy":
                Instantiate(ps, this.transform.position, new Quaternion());
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
}
