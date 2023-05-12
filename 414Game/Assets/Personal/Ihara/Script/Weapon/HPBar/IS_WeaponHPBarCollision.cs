/**
 * @file   IS_WeaponHPBarCollision.cs
 * @brief  HPBarの当たり判定処理
 * @author IharaShota
 * @date   2023/05/11
 * @Update 2023/05/11 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_WeaponHPBarCollision : MonoBehaviour
{
    [SerializeField] private IS_WeaponHPBar weaponHPBar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<NK_BossSlime>() != null)
        {
            if (weaponHPBar.GetSetAttack && !other.GetComponent<NK_BossSlime>().GetSetDamageFlag)
            {
                weaponHPBar.GetSetHp -= 7;
                other.GetComponent<NK_BossSlime>().BossSlimeDamage(5);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, 5);
            }
        }

        if (other.gameObject.GetComponent<NK_Slime>() != null)
        {
            if (weaponHPBar.GetSetAttack && !other.GetComponent<NK_Slime>().GetSetDamageFlag)
            {
                weaponHPBar.GetSetHp -= 5;
                other.GetComponent<NK_Slime>().SlimeDamage(5);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, 5);
            }
        }
    }
}
