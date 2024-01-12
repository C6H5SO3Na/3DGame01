using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject goal;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ParticleSystem ps;
    [SerializeField] int life;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //–Ú•W‚ğXV
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
        //’e‚Æ“–‚½‚Á‚½‚ç
        if (collision.gameObject.CompareTag("Shot"))
        {
            --life;
        }
    }
}
