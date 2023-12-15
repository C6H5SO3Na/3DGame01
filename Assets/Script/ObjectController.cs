using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectController : MonoBehaviour
{
    [SerializeField] int life;
    bool isPlayingEffect = false;
    //Rigidbody rb;
    GameObject explosion;
    ParticleSystem ps;
    //[SerializeField] Renderer rd;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        explosion = GameObject.Find("Explosion");
        ps = explosion.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0 && !this.ps.isPlaying)
        {
            ps.Play();
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
