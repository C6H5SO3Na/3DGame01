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
        //ñ⁄ïWÇçXêV
        agent.destination = goal.transform.position;

        if (life <= 0)
        {
            Instantiate(itemPrefab, this.transform.position, new Quaternion());
            Instantiate(ps, this.transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        //íeÇ∆ìñÇΩÇ¡ÇΩÇÁ
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            hit.gameObject.SetActive(false);
        }
    }
}
