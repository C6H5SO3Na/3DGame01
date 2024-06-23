using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectController : MonoBehaviour
{
    GameManager gameManager;
    SEPlayer sePlayer;
    [SerializeField] int life;
    [SerializeField] GameObject[] itemPrefab;
    [SerializeField] ParticleSystem explosion;
    int[] score = new int[] { 1, 5 };
    public int hasItemNum;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
        sePlayer = GameObject.FindWithTag("SEPlayer").GetComponent<SEPlayer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
            //破壊されるときはヒット音を鳴らさない
            if (life <= 0) {
                Dead();
                collision.gameObject.tag = "Destroyed";
                return;
            }
            sePlayer.aud.PlayOneShot(sePlayer.hitSE);
        }
    }

    /// <summary>
    /// 死亡させる
    /// </summary>
    public void Dead()
    {
        sePlayer.aud.PlayOneShot(sePlayer.explosionSE);
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
