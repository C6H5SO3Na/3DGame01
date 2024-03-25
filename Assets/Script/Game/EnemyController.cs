using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] int life;
    CharacterController controller;
    Vector3 moveVec;
    //NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player(Clone)");
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        //�v���C�������ꂽ��A�G�͒�~
        if (PlayerController.playerState == PlayerController.State.Dead)
        {
            moveVec = Vector3.zero;
        }
        else
        {
            Vector3 direction = player.transform.position - this.transform.position;
            direction.y = 0;//�G���󒆕��V���Ȃ��悤��
            moveVec = direction.normalized * Time.deltaTime;//�v���C����ǐ�
        }

        controller.Move(moveVec);
        Debug.Log(moveVec);
        //agent.destination = goal.transform.position;

        if (life <= 0)
        {
            Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
            Instantiate(explosion, this.transform.position, Quaternion.identity);
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

    public void SetLife(int l)
    {
        life = l;
    }
}
