using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    int mainCnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Shot"))
        {
            if (this.rb.velocity == Vector3.zero || this.transform.position.y < -100.0f)
            {
                Destroy(gameObject);
            }
            ++mainCnt;
        }
    }

    //�e�𔭎�
    public void Shoot(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Weapon"))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10.0f, Vector3.forward);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag.Contains("Object"))
                {
                    hit.collider.GetComponent<ObjectController>().SetLife(0);
                }
            }
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag.Contains("Object"))
        {
            Destroy(this.gameObject);
        }
    }
}
