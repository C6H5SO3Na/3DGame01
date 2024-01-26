using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameObject goal;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem ps;
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
        //�v���C�������ꂽ��A�G�͒�~
        if (goal == null) { return; }
        //�ڕW���X�V
        agent.destination = goal.transform.position;

        if (life <= 0)
        {
            Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
            Instantiate(ps, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //�e�Ɠ���������
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
        }
    }
}
