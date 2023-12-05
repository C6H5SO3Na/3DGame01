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
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ÉvÉåÉCÉÑÇÃëÄçÏ
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(3 * Time.deltaTime * transform.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(3 * Time.deltaTime * -transform.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(3 * Time.deltaTime * -transform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(30 * Time.deltaTime * transform.right);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            this.rg.AddForce(100.0f * 3 * transform.up);
            isJumping = true;
        }

        //Ray position = Camera.main.ScreenPointToRay();
        //Debug.DrawRay(position.origin, position.direction, Color.red, 10, true);

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(shotPrefab, transform.TransformPoint(Input.mousePosition), new Quaternion());
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
