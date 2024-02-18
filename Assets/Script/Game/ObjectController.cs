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
            //�d���I�u�W�F�N�g��������A�d���I�u�W�F�N�g�p�̃X�R�A������
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
        //�e�Ɠ���������
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
            //�j�󂳂��Ƃ��͖炳�Ȃ�
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
