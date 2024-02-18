using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectController : MonoBehaviour
{
    GameDirector gameDirector;
    [SerializeField] int life;
    [SerializeField] GameObject[] itemPrefab;
    //Rigidbody rb;
    [SerializeField] ParticleSystem explosion;
    int[] score = new int[] { 1, 5 };
    public int hasItemNum;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            gameDirector.aud.PlayOneShot(gameDirector.explosionSE);
            if (hasItemNum != 0)
            {
                Instantiate(itemPrefab[hasItemNum - 1], this.transform.position, Quaternion.identity);
            }
            Instantiate(explosion, this.transform.position,Quaternion.identity);
            int type = 0;
            //硬いオブジェクトだったら、硬いオブジェクト用のスコアを入れる
            if (gameObject.CompareTag("HardObject"))
            {
                type = 1;
            }
            gameDirector.AddScore(score[type]);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        //弾と当たったら
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
            //破壊されるときは鳴らさない
            if(life > 0)
            {
                gameDirector.aud.PlayOneShot(gameDirector.hitSE);
            }
        }
    }

    public void SetLife(int l)
    {
        life = l;
    }
}
