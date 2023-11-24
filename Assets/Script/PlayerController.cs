using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{Å@
    Rigidbody rg;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.rg.AddForce(transform.forward * 2.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.rg.AddForce(transform.right * -2.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.rg.AddForce(transform.forward * -2.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.rg.AddForce(transform.right * 2.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rg.AddForce(transform.up * 200.0f);
        }
    }
}
