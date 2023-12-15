using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectController : MonoBehaviour
{   
    public int life;
    bool isPlayingParticle;
    //Rigidbody rb;
    ParticleSystem ps;
    //[SerializeField] Renderer rd;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0 && !isPlayingParticle)
        {
            this.ps.Play();
            //rd.enabled = true;
            isPlayingParticle = true;
        }
        if (isPlayingParticle && !this.ps.isPlaying)
        {
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
