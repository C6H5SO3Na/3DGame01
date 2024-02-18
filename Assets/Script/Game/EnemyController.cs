using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameObject goal;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] int life;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        goal = GameObject.Find("Player(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤがやられたら、敵は停止
        if (PlayerController.playerState == PlayerController.State.Dead) { return; }
        //目標を更新
        agent.destination = goal.transform.position;

        if (life <= 0)
        {
            Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
            Instantiate(explosion, this.transform.position, Quaternion.identity);
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
