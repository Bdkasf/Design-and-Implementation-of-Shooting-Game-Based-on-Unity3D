using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public int MAX_Damage;
    public int MIN_Damage;

    private void OnTriggerEnter(Collider other)
    {
        //玩家受到伤害扣血
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().PlayerHealth(Random.Range(MIN_Damage, MAX_Damage));
        }
    }
}
