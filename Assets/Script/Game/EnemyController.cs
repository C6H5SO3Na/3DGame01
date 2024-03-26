using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] int life;
    CharacterController controller;
    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player(Clone)");
        controller = GetComponent<CharacterController>();
        playerController = player.GetComponent<PlayerController>();
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
            direction.y = 0;//�G���v���C���̃W�����v���ɋ󒆕��V���Ȃ��悤��
            moveVec = direction.normalized * Time.deltaTime;//�v���C����ǐ�
        }

        controller.Move(moveVec);

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

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().Dead();
        }
    }

    /// <summary>
    /// ���C�t��ݒ肷��
    /// </summary>
    /// <param name="l">�ݒ肵�������C�t</param>
    public void SetLife(int l)
    {
        life = l;
    }
}
