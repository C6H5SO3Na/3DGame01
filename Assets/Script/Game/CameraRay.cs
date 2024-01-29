using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private Transform player;

    public GameObject[] prevRaycast;
    public List<GameObject> raycastHitsList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = (player.transform.position - this.transform.position);
        Vector3 direction = difference.normalized;
        Ray ray = new Ray(this.transform.position, direction);
        RaycastHit[] rayCastHits = Physics.RaycastAll(ray);

        prevRaycast = raycastHitsList.ToArray();
        raycastHitsList.Clear();

        foreach (RaycastHit hit in rayCastHits)
        {
            Material material = hit.collider.GetComponent<MeshRenderer>().material;
            if (hit.collider.tag == "Wall")
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
                raycastHitsList.Add(hit.collider.gameObject);//hit‚µ‚½gameobject‚ð’Ç‰Á‚·‚é
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
