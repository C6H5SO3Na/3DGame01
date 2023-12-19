using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    CharacterController charaCon;
    [SerializeField] GameObject shotPrefab;
    bool isJumping;
    float speed;
    float fallSpeed;
    Vector2 angle;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        charaCon = GetComponent<CharacterController>();
        isJumping = false;
        speed = 3.0f;
        fallSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���̏㉺���E�ړ�
        velocity.x = speed * Time.deltaTime * Input.GetAxis("Horizontal_L");
        velocity.z = speed * Time.deltaTime * Input.GetAxis("Vertical_L");

        //�v���C���̎��_�ύX
        if (Input.GetAxis("Vertical_R") != 0.0f)
        {
            //angle.y -= Input.GetAxis("Vertical_R");
            angle.y = Input.GetAxis("Vertical_R");
        }
        if (Input.GetAxis("Horizontal_R") != 0.0f)
        {
            angle.x = Input.GetAxis("Horizontal_R");
        }

        //Camera.main.transform.localRotation = Quaternion.Euler(angle.y, angle.x, 0);
        //�J��������]
        Camera.main.transform.RotateAround(transform.position, transform.up, angle.x);
        Camera.main.transform.RotateAround(transform.position, transform.right, angle.y);
        //�W�����v
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            fallSpeed = 10.0f * Time.deltaTime;
        }
        fallSpeed += -0.3f * Time.deltaTime;
        velocity.y += fallSpeed;

        charaCon.Move(velocity);
        velocity = Vector3.zero;
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
    void OnControllerColliderHit(ControllerColliderHit hit)
    //(Collision collision)
    {
        isJumping = false;
    }
}
