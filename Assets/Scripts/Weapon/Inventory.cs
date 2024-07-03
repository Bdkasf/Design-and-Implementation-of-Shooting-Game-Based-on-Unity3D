using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 武器库逻辑
/// 人物的武器切换，添加，去除功能
/// </summary>
public class Inventory : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();

    public int currentWeaponID;
    private KeyCode throwWeapon = KeyCode.G;

    // Start is called before the first frame update
    void Start()
    {  
        currentWeaponID = -1;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCurrentWeaponID();
        if (Input.GetKeyDown(throwWeapon) && currentWeaponID >= 0 && currentWeaponID < weapons.Count)
        {
            ThrowWeapon(weapons[currentWeaponID]);
        }
    }

    /// <summary>
    /// 更新武器编号
    /// </summary>
    public void ChangeCurrentWeaponID()
    {
        //-0.1 - 0 - 0.1
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //通过鼠标滚轮下一把武器
            ChangeWeapon(currentWeaponID + 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //上一把武器
            ChangeWeapon(currentWeaponID - 1);
        }

        /* 通过字母数字键盘切换武器 */
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int num = 0;
                if (i == 10)
                {
                    num = 10;
                }
                else
                {
                    num = i - 1;
                }
                /* 如果数字小于武器列表个数的时候才能进一步处理
                如果有3把武器，但按下4-9，那么是无意义的，不处理 */
                if (num < weapons.Count)
                {
                    ChangeWeapon(num);
                }
            }
        }
    }

    /// <summary>
    /// 武器切换
    /// </summary>
    /// <param name="weaponID"></param>
    public void ChangeWeapon(int weaponID)
    {
        if (weapons.Count == 0) return;

        /* 
        如果切换到最大索引编号的枪的时候，就调出第一把枪
        如果切换到最小索引编号的枪的时候，就调出最后一把枪
        */
        //IndexOf:获取列表中首次出现的索引
        //Max list 里获取最大元素
        if ( weaponID > weapons.Max(weapons.IndexOf))
        {
            weaponID = weapons.Min(weapons.IndexOf);
        }
        else if ( weaponID < weapons.Min(weapons.IndexOf))
        {
            weaponID = weapons.Max(weapons.IndexOf);
        }

        if (weaponID == currentWeaponID)
        {
            //只有一种武器的时候不进行切换
            return;
        }

        currentWeaponID = weaponID;//更新武器索引

        /* 根据武器编号，显示出对应的武器 */
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weaponID ==i)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 添加武器
    /// </summary>
    public void AddWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon))
        {
            print("集合里已存在此枪械");
            return;
        }
        else
        {
            if (weapons.Count < 3)
            {
                weapons.Add(weapon);
                ChangeWeapon(currentWeaponID + 1);//显示武器
                weapon.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 丢弃武器
    /// </summary>
    public void ThrowWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon) && weapons.Count > 0)
        {
            weapons.Remove(weapon); //集合中删除对应的游戏武器
            ChangeWeapon(currentWeaponID - 1); //显示武器
            weapon.gameObject.SetActive(false);
        }
        else
        {
            print("没有这个武器,无法丢弃");
        }
    }
}
