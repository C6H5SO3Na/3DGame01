using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] ParticleSystem explosion;
    float mainCnt = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.CompareTag("Shot")) { return; }
        //定期的に消すことで処理落ち回避
        if (mainCnt >= 5.0f || this.transform.position.y < -100.0f)
        {
            Destroy(gameObject);
        }
        mainCnt += Time.deltaTime;
    }

    /// <summary>
    ///弾を発射
    /// </summary>
    /// <param name="direction">方向ベクトル</param>
    public void Shoot(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Weapon"))
        {
            gameManager.aud.PlayOneShot(gameManager.explosionSE);
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10.0f, Vector3.forward);
            foreach (RaycastHit hit in hits)
            {
                //周辺にいる敵とオブジェクトを破壊
                if (hit.collider.gameObject.tag.Contains("Object"))
                {
                    hit.collider.GetComponent<ObjectController>().Dead();
                }
                else if (hit.collider.gameObject.tag.Contains("Enemy"))
                {
                    hit.collider.GetComponent<EnemyController>().Dead();
                }
            }
            Instantiate(explosion, this.transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag.Contains("Object") || collision.gameObject.tag.Contains("Enemy"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().Dead();
        }
    }
}
