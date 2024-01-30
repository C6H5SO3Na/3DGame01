using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    GameDirector gameDirector;
    //Rigidbody rb;
    float mainCnt = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        //this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Shot"))
        {
            //5�b�o�߁A�Ⴕ���͓ޗ��ɗ������Ƃ��A����
            if (mainCnt >= 5.0f || this.transform.position.y < -100.0f)
            {
                Destroy(gameObject);
            }
            mainCnt += Time.deltaTime;
        }
    }

    //�e�𔭎�
    public void Shoot(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Weapon"))
        {
            gameDirector.aud.PlayOneShot(gameDirector.explosionSE);
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10.0f, Vector3.forward);
            foreach (var hit in hits)
            {
                //���ӂɂ���G�ƃI�u�W�F�N�g��j��
                if (hit.collider.gameObject.tag.Contains("Object"))
                {
                    hit.collider.GetComponent<ObjectController>().SetLife(0);
                }
                else if(hit.collider.gameObject.tag.Contains("Enemy"))
                {
                    hit.collider.GetComponent<EnemyController>().SetLife(0);
                }
            }
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag.Contains("Object") || collision.gameObject.tag.Contains("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
