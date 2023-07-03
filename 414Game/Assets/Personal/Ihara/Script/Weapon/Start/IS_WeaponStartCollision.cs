/**
 * @file   IS_WeaponStartCollision.cs
 * @brief  Startの当たり判定処理
 * @author IharaShota
 * @date   2023/05/25
 * @Update 2023/05/25 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_WeaponStartCollision : MonoBehaviour
{
    [SerializeField] private IS_WeaponStart WeaponStart;
    [SerializeField] private IS_Player Player;    // Playerをアタッチ
    [SerializeField] private int m_nDamage2Enemy; // 敵に与えるダメージ量
    [SerializeField] private int m_nDamage2Start; // Startに与えるダメージ量
    [SerializeField] private ParticleSystem HitEffect;  // ヒットエフェクト

    private void OnTriggerEnter(Collider other)
    {
        // ボスへのダメージ処理
        if (other.gameObject.GetComponent<NK_BossSlime>() != null)
        {
            if (WeaponStart.GetSetAttack && !other.GetComponent<NK_BossSlime>().GetSetDamageFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponStart.GetSetHp -= m_nDamage2Start;
                other.GetComponent<NK_BossSlime>().BossSlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();
            }
        }
        // スライムへのダメージ処理
        if (other.gameObject.GetComponent<slime>() != null)
        {
            if (WeaponStart.GetSetAttack && !other.GetComponent<slime>().GetSetDamageFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponStart.GetSetHp -= m_nDamage2Start;
                other.GetComponent<slime>().SlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();
            }
        }

        // 蝙蝠へのダメージ処理
        if (other.gameObject.GetComponent<bat>() != null)
        {
            if (WeaponStart.GetSetAttack && !other.GetComponent<bat>().GetSetDamageFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponStart.GetSetHp -= m_nDamage2Start;
                other.GetComponent<bat>().BatDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();
            }
        }


        // スライムベスへのダメージ処理
        if (other.gameObject.GetComponent<NK_SlimeBes>() != null)
        {
            IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
            WeaponStart.GetSetHp -= m_nDamage2Start;
            other.GetComponent<NK_SlimeBes>().BesDamage(m_nDamage2Enemy);
            other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
            HitEffect.transform.position = other.transform.position;
            HitEffect.Play();
        }

        // 耐久値が0以下になったらゲームオーバー
        if (WeaponStart.GetSetHp <= 0)
        {
            Player.RemovedWeapon();
        }
    }
}
