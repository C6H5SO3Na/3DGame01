using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectController : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] int life;
    [SerializeField] GameObject[] itemPrefab;
    [SerializeField] ParticleSystem explosion;
    int[] score = new int[] { 1, 5 };
    public int hasItemNum;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            gameManager.aud.PlayOneShot(gameManager.explosionSE);
            if (hasItemNum != 0)
            {
                Instantiate(itemPrefab[hasItemNum - 1], this.transform.position, Quaternion.identity);
            }
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            int type = 0;
            //硬いオブジェクトだったら、硬いオブジェクト用のスコアを入れる
            if (gameObject.CompareTag("HardObject"))
            {
                type = 1;
            }
            gameManager.AddScore(score[type]);
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
            if (life <= 0) { return; }
            gameManager.aud.PlayOneShot(gameManager.hitSE);
        }
    }

    /// <summary>
    /// ライフを0にする
    /// </summary>
    public void Dead()
    {
        life = 0;
    }
}
