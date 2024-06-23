using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    float mainCnt = 0.0f;

    // Update is called once per frame
    void Update()
    {
        //����I�ɏ������Ƃŏ����������
        if (mainCnt >= 5.0f || this.transform.position.y < -100.0f)
        {
            Destroy(gameObject);
        }
        mainCnt += Time.deltaTime;
    }

    /// <summary>
    ///�e�𔭎�
    /// </summary>
    /// <param name="direction">�����x�N�g��</param>
    public void Shoot(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Object"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().Dead();
            Destroy(this.gameObject);
        }
    }
}
