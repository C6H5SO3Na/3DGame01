using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PlayerController : MonoBehaviour
{
    CharacterController charaCon;
    [SerializeField] GameObject shotPrefab;
    float speed;
    float fallSpeed;
    int getItemNum;
    Vector2 angle;
    Vector3 velocity;
    Vector2 lotateAngle;

    public enum State
    {
        Normal, Clear,
    }
    public State state;
    // Start is called before the first frame update
    void Start()
    {
        charaCon = GetComponent<CharacterController>();
        speed = 3.0f;
        fallSpeed = 0.0f;
        getItemNum = 0;
        lotateAngle = Vector3.zero;
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal)
        {
            //�v���C���̏㉺���E�ړ�
            velocity.x = speed * Input.GetAxis("Horizontal");
            velocity.z = speed * Input.GetAxis("Vertical");

            //�v���C���̎��_�ύX
            //Debug.Log(Camera.main.transform.localEulerAngles);

            lotateAngle.x = Input.GetAxis("Horizontal_R");
            lotateAngle.y = Input.GetAxis("Vertical_R");

            angle += lotateAngle;

            //�J��������]
            Camera.main.transform.RotateAround(transform.position, transform.up, lotateAngle.x);
            Camera.main.transform.RotateAround(transform.position, Camera.main.transform.right, lotateAngle.y);

            //���n����
            if (charaCon.isGrounded)
            {
                fallSpeed = 0.0f;
                //�W�����v
                if (Input.GetButtonDown("Jump"))
                {
                    fallSpeed = 10.0f;
                }
            }
            //�e����
            if (Input.GetMouseButtonDown(0))
            {
                GameObject shot = Instantiate(shotPrefab);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 worldDirection = ray.direction;
                shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

                //�ړ������ʒu����e����
                shot.transform.position = Camera.main.transform.position
                    + Camera.main.transform.forward * 5.5f;
            }
        }
        fallSpeed += -0.3f;
        velocity.y += fallSpeed;

        charaCon.Move(velocity * Time.deltaTime);
        velocity = Vector3.zero;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Item":
                //���炩�̏���
                ++getItemNum;
                Destroy(hit.gameObject);
                break;
        }
    }
}
