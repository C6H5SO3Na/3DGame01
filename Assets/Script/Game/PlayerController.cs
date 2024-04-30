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
using UnityEngine.UI;

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

    GameManager gameManager;

    public static float speed = 4.0f;
    public static int[] getItemNum = new int[4]; //0:None 1:Speed 2:RapidFire 3:Weapon
    Vector3 moveDirection;
    Vector2 playerDirection;
    Quaternion defaultCameraDirection;
    Vector3 defaultCameraOffset;
    public static bool isRapidFire = false;
    public static bool isAbleMultiShot = false;
    int mainCnt = 0;
    bool isClear = false;//�N���A���̃{�C�X��1�񂵂������Ȃ��悤�ɂ���

    //struct Jump//�A�j���[�V�����Ƃ̍��������̂���
    //{
    //    public bool isStart;
    //    //public bool isEnd;
    //}

    //Jump jump;

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
        gameManager = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
        playerState = State.Normal;
        defaultCameraDirection = Camera.main.transform.rotation;
        defaultCameraOffset = Camera.main.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null) { return; }
        if (GameManager.phase < GameManager.Phase.GAME) { return; }//�Q�[�����n�܂��Ă��Ȃ��Ƃ��͓��삵�Ȃ�
        if (playerState == State.Normal)
        {
            animator.SetBool("Run", Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f);
            //�v���C���̏㉺���E�ړ�
            if (animator.GetBool("Run"))
            {
                Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                if (inputVector.magnitude > 1.0f)
                {
                    inputVector = inputVector.normalized;
                }
                moveDirection.x = speed * inputVector.x;
                moveDirection.z = speed * inputVector.z;

                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                //���胂�[�V���������A���x���A�j���[�V�����ɔ��f
                if (stateInfo.IsName("Run"))
                {
                    float dir = Mathf.Pow(moveDirection.z * moveDirection.z + moveDirection.x * moveDirection.x, 0.5f);
                    SetAnimSpeed(dir / 5.0f);
                    //Debug.Log(dir);
                }
                else
                {
                    SetAnimSpeed(1.0f);
                }

                float normalizedDir = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0.0f, playerDirection.x + normalizedDir, 0.0f);
            }
            else
            {
                moveDirection.x = moveDirection.z = 0.0f;//�������͂���Ă��Ȃ��Ƃ���xz�̈ړ��ʂ�0�ɂ���
                SetAnimSpeed(1.0f);
            }

            //�v���C���̎��_�ύX
            playerDirection.x += Input.GetAxis("Horizontal_R");
            //playerDirection.y -= Input.GetAxis("Vertical_R");

            //�J�����̏㉺�𐧌�
            //playerDirection.y = Mathf.Clamp(playerDirection.y, -10.0f, 45.0f);

            //�J��������]
            Camera.main.transform.rotation = Quaternion.Euler(playerDirection.y, playerDirection.x, 0.0f) * defaultCameraDirection;
            Camera.main.transform.position = transform.position + Quaternion.Euler(playerDirection.y, playerDirection.x, 0.0f) * defaultCameraOffset;

            //���n����
            //if (controller.isGrounded)
            //{
            //if (!jump.isStart)
            //{
            //    animator.SetBool("Jump", false);
            //}

            ////�W�����v
            //if (Input.GetButtonDown("Jump"))
            //{
            //    jump.isStart = true;
            //    Invoke("SetJump", 0.4f);
            //    animator.SetBool("Jump", true);
            //}
            //}
            //else
            //{
                moveDirection.y -= gravity * Time.deltaTime;
                //jump.isStart = false;
            //}
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
                    transform.right * -0.5f,
                    transform.right * 0.5f
                };

                gameManager.aud.PlayOneShot(gameManager.shootSE);//3���˂��Ă�1�񂵂��炳�Ȃ�(���ʂ̓s��)
                //�}���`�V���b�g���L���ɂȂ��Ă���ꍇ�A3�̒e�𔭎�
                for (int i = 0; i < shotNum; ++i)
                {
                    GameObject shot = Instantiate(shotPrefab);
                    float vec;
                    if(i == 2)
                    {
                        vec = i * 5f;
                    }
                    else
                    {
                        vec = -i * 5f;
                    }
                    Ray ray = new Ray(transform.position + transform.forward * 2.0f + transform.up * 0.5f,
                        transform.forward * 8f + transform.right * vec);//�^�񒆂�������߂����Ĕ���
                    Vector3 worldDirection = ray.direction;
                    shot.GetComponent<ShotController>().Shoot(worldDirection * 800.0f);

                    //�ړ������ʒu����e����
                    shot.transform.position = transform.position + transform.forward * 2.0f + transform.up * 0.5f + difference[i];
                }
            }

            //���͒e����
            if (Input.GetButtonDown("Fire2") && getItemNum[3] != 0)
            {
                aud.PlayOneShot(weapon);
                --getItemNum[3];
                GameObject shot = Instantiate(weaponPrefab);
                Ray ray = new Ray(transform.position + transform.forward * 0.5f,
                    transform.forward + transform.up * 0.15f);//�^�񒆂�������߂����Ĕ���
                Vector3 worldDirection = ray.direction;
                shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

                //�ړ������ʒu����e����
                shot.transform.position = transform.position + transform.forward * 2.0f + transform.up * 0.5f;
            }
        }
        else
        {
            moveDirection.x = moveDirection.z = 0;//�v���C���Ă��Ȃ��Ƃ���xz�̈ړ��ʂ�0�ɂ���
            if (playerState == State.Clear)
            {
                SetAnimSpeed(1.0f);
                animator.SetTrigger("Clear");
                transform.rotation = Quaternion.Euler(0, playerDirection.x + 180.0f, 0);
                if (!isClear)
                {
                    aud.PlayOneShot(clear);
                    isClear = true;
                }
            }
        }



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
                gameManager.aud.PlayOneShot(gameManager.itemGetSE);
                ++getItemNum[0];
                GameManager.score += 1;
                speed *= 1.2f;
                break;
            case "RapidFireItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                gameManager.aud.PlayOneShot(gameManager.itemGetSE);
                ++getItemNum[1];
                GameManager.score += 2;
                isRapidFire = true;
                break;
            case "MultiShotItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                gameManager.aud.PlayOneShot(gameManager.itemGetSE);
                ++getItemNum[2];
                GameManager.score += 2;
                isAbleMultiShot = true;
                break;
            case "WeaponItem":
                Destroy(hit.gameObject);
                hit.gameObject.tag = "Destroyed";//������񂱂̊֐����Ăяo����邽�߃^�O��ύX���ČĂяo�������
                gameManager.aud.PlayOneShot(gameManager.itemGetSE);
                ++getItemNum[3];
                GameManager.score += 5;
                break;
        }
    }
    /// <summary>
    /// �v���C���̎��S����
    /// </summary>
    public void Dead()
    {
        animator.SetTrigger("Dead");
        playerState = State.Dead;
        GameManager.phase = GameManager.Phase.CLEAR;//�N���A��ԂƂ��Ă���
        gameManager.aud.PlayOneShot(gameManager.damageSE);
        aud.PlayOneShot(dead);
        Invoke("AnimStop", 2.0f / speed * 4.0f);//�����A�C�e���ɑΉ�
    }

    /// <summary>
    /// �A�˂̏���
    /// </summary>
    /// <returns>�e�����˂ł����true</returns>
    bool RapidFireOperation()
    {
        return isRapidFire && Input.GetButton("Fire1") && mainCnt % 8 == 0;
    }

    /// <summary>
    /// �W�����v�̏���
    /// </summary>
    void SetJump()
    {
        moveDirection.y = 6.0f;
    }

    public void SetAnimSpeed(float n)
    {
        animator.speed = n;
    }
    public void AnimStop()
    {
        animator.speed = 0.0f;
    }
}
