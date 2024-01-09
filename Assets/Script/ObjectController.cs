using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectController : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] GameObject itemPrefab;
    //Rigidbody rb;
    [SerializeField] ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            Instantiate(itemPrefab, this.transform.position, new Quaternion());
            Instantiate(ps,this.transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //’e‚Æ“–‚½‚Á‚½‚ç
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
        }
    }
}
