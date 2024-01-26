using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponNumManager : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject canvas;
    RectTransform rectTransform;
    int weaponNum;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = weapon.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        weaponNum = PlayerController.getItemNum[3];
    }

    public void SetSprite()
    {
        rectTransform.position = new Vector3(weaponNum * 40, rectTransform.position.y, rectTransform.position.z);
        GameObject prefab = Instantiate(weapon, rectTransform);
        prefab.transform.SetParent(canvas.transform, false);
    }
}
