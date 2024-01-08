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
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            if (!ps.isPlaying)
            {
                ps.transform.position = this.transform.position;
                ps.Play();
                Instantiate(itemPrefab, this.transform.position, new Quaternion());
            }
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

    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
