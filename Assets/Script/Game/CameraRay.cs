using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    GameObject player;

    public GameObject[] prevRaycast;
    public List<GameObject> raycastHitsList = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 difference = player.transform.position - this.transform.position;
        Vector3 direction = difference.normalized;
        Ray ray = new Ray(this.transform.position, direction);
        RaycastHit[] rayCastHits = Physics.RaycastAll(ray, 7.0f);//プレイヤが壁の手前に来たときに半透明にならないようにする

        prevRaycast = raycastHitsList.ToArray();
        raycastHitsList.Clear();

        //カメラに映るオブジェクトを探す
        foreach (RaycastHit hit in rayCastHits)
        {
            MeshRenderer mesh = hit.collider.GetComponent<MeshRenderer>();
            if (mesh == null) { continue; }//警告防止
            Material material = mesh.material;
            if (hit.collider.CompareTag("Wall"))
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
                raycastHitsList.Add(hit.collider.gameObject);//hitしたgameobjectを追加する
            }
        }

        //壁を透過させる
        foreach (GameObject tmpGameObject in prevRaycast.Except<GameObject>(raycastHitsList))
        {
            Material material = tmpGameObject.GetComponent<MeshRenderer>().material;
            if (tmpGameObject != null)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1.0f);
            }
        }
    }
}
