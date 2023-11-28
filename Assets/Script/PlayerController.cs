using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rg;
    GameObject shotPrefab;
    bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        shotPrefab = GameObject.Find("ShotPrefab");
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * 3 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-transform.right * 3 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-transform.forward * 3 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * 3 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            this.rg.AddForce(transform.up * 3 * 100.0f);
            isJumping = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(shotPrefab);
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
