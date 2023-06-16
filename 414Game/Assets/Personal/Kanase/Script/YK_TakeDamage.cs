﻿/**
 * @file YK_TakeDamage.cs
 * @brief 敵にダメージを表記するためのスクリプト
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using UnityEngine;
using System.Collections;

public class YK_TakeDamage : MonoBehaviour
{

    //　DamageUIプレハブ
    [SerializeField] private GameObject damageUI;

    public void Damage(Collider col, int damage)
    {
        damageUI.GetComponent<YK_DamageUI>().SetDamage(damage);
        //　DamageUIをインスタンス化。登場位置は接触したコライダの中心からカメラの方向に少し寄せた位置
        var obj = Instantiate<GameObject>(damageUI, col.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);
    }
}