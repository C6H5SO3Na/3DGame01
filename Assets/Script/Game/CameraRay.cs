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
        player = GameObject.Find("Player(Clone)");
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 difference = player.transform.position - this.transform.position;
        Vector3 direction = difference.normalized;
        Ray ray = new Ray(this.transform.position, direction);
        RaycastHit[] rayCastHits = Physics.RaycastAll(ray);

        Debug.DrawRay(ray.origin, ray.direction);
        prevRaycast = raycastHitsList.ToArray();
        raycastHitsList.Clear();

        foreach (RaycastHit hit in rayCastHits)
        {
            MeshRenderer mesh = hit.collider.GetComponent<MeshRenderer>();
            if (mesh == null) { continue; }//åxçêñhé~
            Material material = mesh.material;
            if (hit.collider.tag == "Wall")
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
                raycastHitsList.Add(hit.collider.gameObject);//hitÇµÇΩgameobjectÇí«â¡Ç∑ÇÈ
            }
        }
        foreach (GameObject tmpGameObject in prevRaycast.Except<GameObject>(raycastHitsList))
        {
            Material material = tmpGameObject.GetComponent<MeshRenderer>().material;
            if (tmpGameObject != null)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1.0f);
                //noSampleMaterial.NotClearMaterialInvoke();
            }

        }
    }
}
