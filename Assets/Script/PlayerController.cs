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
    float fall;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        charaCon = GetComponent<CharacterController>();
        isJumping = false;
        speed = 3.0f;
        fall = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤの操作
        if (Input.GetKey(KeyCode.W))
        {
            velocity = speed * Time.deltaTime * transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity = speed * Time.deltaTime * -transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity = speed * Time.deltaTime * -transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity = speed * Time.deltaTime * transform.right;
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))// && !isJumping)
        {
            fall = 1;
            isJumping = true;
        }
        fall += Physics.gravity.y * Time.deltaTime * 0.0001f;
        velocity.y = fall;

        charaCon.Move(velocity);
        velocity = Vector3.zero;
        //弾発射
        if (Input.GetMouseButtonDown(0))
        {
            GameObject shot = Instantiate(shotPrefab);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldDirection = ray.direction;
            shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

            //移動した位置から弾発射
            shot.transform.position = Camera.main.transform.position
                + Camera.main.transform.forward * 5.5f;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            isJumping = false;
        }
    }
}
