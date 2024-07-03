using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 武器拾取
/// </summary>
public class PickUpItem : MonoBehaviour
{
    [Tooltip("武器旋转的速度")]private float rotateSpeed;
    [Tooltip("武器编号")]public int itemID;
    private GameObject weaponModel;


    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3 (0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
        PlayerController player = other.GetComponent<PlayerController>();
        //查找获取 Inventory 物体下的各个武器物体
        weaponModel = GameObject.Find("Player/Assult_Rifle_Arm/Inventory/").gameObject.transform.GetChild(itemID).gameObject;
        player.PickUpWeapon(itemID, weaponModel);
        Destroy(gameObject);
        }
    }
}
