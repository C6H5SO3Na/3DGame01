using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectController : MonoBehaviour
{
    GameObject gameDirector;
    [SerializeField] int life;
    [SerializeField] GameObject itemPrefab;
    //Rigidbody rb;
    [SerializeField] ParticleSystem ps;
    int[] score = new int[]{1, 5};
    public bool hasItem;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector");
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            if (hasItem)
            {
                Instantiate(itemPrefab, this.transform.position, new Quaternion());
            }
            Instantiate(ps,this.transform.position, new Quaternion());
            int type = 0;
            if (gameObject.CompareTag("HardObject"))
            {
                type = 1;
            }
            gameDirector.GetComponent<GameDirector>().AddScore(score[type]);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        //弾と当たったら
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
        }
    }

    public void SetLife(int l)
    {
        life = l;
    }
}
