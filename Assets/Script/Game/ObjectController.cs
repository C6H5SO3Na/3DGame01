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
            //�j�󂳂��Ƃ��̓q�b�g����炳�Ȃ�
            if (life <= 0) {
                Dead();
                collision.gameObject.tag = "Destroyed";
                return;
            }
            sePlayer.aud.PlayOneShot(sePlayer.hitSE);
        }
    }

    /// <summary>
    /// ���S������
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
        //�d���I�u�W�F�N�g��������A�d���I�u�W�F�N�g�p�̃X�R�A������
        if (gameObject.CompareTag("HardObject"))
        {
            type = 1;
        }
        gameManager.AddScore(score[type]);
        Destroy(gameObject);
    }
}
