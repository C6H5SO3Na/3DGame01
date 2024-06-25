using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    SEPlayer sePlayer;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] int life;
    CharacterController controller;
    Animator animator;
    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sePlayer = GameObject.FindWithTag("SEPlayer").GetComponent<SEPlayer>();
        controller = GetComponent<CharacterController>();
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();//モデルに附属
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム本編以外は停止
        if (GameManager.phase != GameManager.Phase.Game)
        {
            moveVec = Vector3.zero;
            animator.speed *= 0.99f;
            return;
        }
        animator.speed = 1.0f;
        Vector3 direction = player.transform.position - this.transform.position;
        moveVec = direction.normalized * Time.deltaTime;//プレイヤを追跡

        moveVec.y -= 9.8f * Time.deltaTime;

        float normalizedDir = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, normalizedDir, 0.0f);
        controller.Move(moveVec);

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
