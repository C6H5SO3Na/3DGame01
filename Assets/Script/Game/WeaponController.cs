using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    SEPlayer sePlayer;
    [SerializeField] ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        sePlayer = GameObject.FindWithTag("SEPlayer").GetComponent<SEPlayer>();
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
        sePlayer.aud.PlayOneShot(sePlayer.explosionSE);
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
}

