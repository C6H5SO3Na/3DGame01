using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] int life;
    CharacterController controller;
    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.phase != GameManager.Phase.GAME) { return; }
        //�v���C�������ꂽ��A�G�͒�~
        if (PlayerController.playerState == PlayerController.State.Dead)
        {
            moveVec = Vector3.zero;
        }
        else
        {
            Vector3 direction = player.transform.position - this.transform.position;
            moveVec = direction.normalized * Time.deltaTime;//�v���C����ǐ�
            moveVec.y = 0.0f;//�G���󒆕��V���Ȃ��悤��
        }

        controller.Move(moveVec);

        //y���W������
        Vector3 tmp = controller.gameObject.transform.position;
        tmp.y = 1.0f;
        controller.gameObject.transform.position = tmp;

        if (life <= 0)
        {
            Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().Dead();
        }
    }

    /// <summary>
    /// ���C�t��0�ɂ���
    /// </summary>
    public void Dead()
    {
        life = 0;
    }
}
