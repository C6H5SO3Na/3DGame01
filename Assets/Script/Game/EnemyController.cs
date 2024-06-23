using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    SEPlayer sePlayer;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] int life;
    CharacterController controller;
    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sePlayer = GameObject.FindWithTag("SEPlayer").GetComponent<SEPlayer>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム本編以外は停止
        if (GameManager.phase != GameManager.Phase.Game) { return; }
        //プレイヤがやられたら、敵は停止
        if (PlayerController.playerState == PlayerController.State.Dead)
        {
            moveVec = Vector3.zero;
        }
        else
        {
            Vector3 direction = player.transform.position - this.transform.position;
            moveVec = direction.normalized * Time.deltaTime;//プレイヤを追跡
            controller.Move(moveVec);
        }


        //y座標を矯正
        Vector3 tmp = controller.gameObject.transform.position;
        tmp.y = 1.0f;
        controller.gameObject.transform.position = tmp;

        if (life <= 0)
        {
            Dead();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().Dead();
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Dead()
    {
        sePlayer.aud.PlayOneShot(sePlayer.explosionSE);
        Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
