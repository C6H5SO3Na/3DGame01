using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    CharacterController charaCon;
    public GameObject shotPrefab;
    bool isJumping;
    float speed;
    float fallSpeed;
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
        velocity.x = speed * Time.deltaTime * Input.GetAxis("Horizontal");
        velocity.z = speed * Time.deltaTime * Input.GetAxis("Vertical");

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
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
        if (hit.gameObject.CompareTag("Plane"))
        {
            isJumping = false;
        }
    }
}
