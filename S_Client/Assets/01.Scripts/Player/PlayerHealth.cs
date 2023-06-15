using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 50;
    int currentHP;
    public void Init() {
        currentHP = maxHP;
    }
    public void HitDamage(int damage)
    {
        Debug.Log($"{damage}아야!");
        currentHP -= damage;

    }
}
