using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Rigidbody rg;
    public GameObject shotPrefab;
    bool isJumping;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        isJumping = false;
        speed = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //ÉvÉåÉCÉÑÇÃëÄçÏ
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(speed * Time.deltaTime * transform.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(speed * Time.deltaTime * -transform.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(speed * Time.deltaTime * -transform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime * transform.right);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            this.rg.AddForce(300.0f * transform.up);
            isJumping = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject shot = Instantiate(shotPrefab);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldDirection = ray.direction;
            shot.GetComponent<ShotController>().Shoot(worldDirection.normalized * 1000.0f);

            //à⁄ìÆÇµÇΩà íuÇ©ÇÁíeî≠éÀ
            shot.transform.position = Camera.main.transform.position
                + Camera.main.transform.forward * 5.5f;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            isJumping = false;
        }
    }
}
