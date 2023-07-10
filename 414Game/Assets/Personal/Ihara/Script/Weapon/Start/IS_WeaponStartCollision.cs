/**
 * @file   IS_WeaponStartCollision.cs
 * @brief  Startの当たり判定処理
 * @author IharaShota
 * @date   2023/05/25
 * @Update 2023/05/25 作成
 * @Update 2023/06/30 コンボ処理追加
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
            if (IS_Player.instance.GetFlg().m_bAttack && !other.GetComponent<NK_BossSlime>().GetSetDamageFlag)
            {
                YK_Controller.instance.ControllerVibration(0.3f);
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponStart.GetSetHp -= m_nDamage2Start;
                other.GetComponent<NK_BossSlime>().BossSlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();
                YK_Combo.AddCombo();
            }
        }
        // スライムへのダメージ処理
        if (other.gameObject.GetComponent<slime>() != null)
        {
            if (IS_Player.instance.GetFlg().m_bAttack && !other.GetComponent<slime>().GetSetDamageFlag)
            {
                YK_Controller.instance.ControllerVibration(0.3f);
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponStart.GetSetHp -= m_nDamage2Start;
                other.GetComponent<slime>().SlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();
                YK_Combo.AddCombo();
            }
        }

        // 蝙蝠へのダメージ処理
        if (other.gameObject.GetComponent<bat>() != null)
        {
            if (IS_Player.instance.GetFlg().m_bAttack && !other.GetComponent<bat>().GetSetDamageFlag)
            {
                YK_Controller.instance.ControllerVibration(0.3f);
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponStart.GetSetHp -= m_nDamage2Start;
                other.GetComponent<bat>().BatDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();
                YK_Combo.AddCombo();
            }
        }


        // スライムベスへのダメージ処理
        if (other.gameObject.GetComponent<bes>() != null)
        {
            YK_Controller.instance.ControllerVibration(0.3f);
            IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
            WeaponStart.GetSetHp -= m_nDamage2Start;
            other.GetComponent<bes>().BesDamage(m_nDamage2Enemy);
            other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
            HitEffect.transform.position = other.transform.position;
            HitEffect.Play();
            YK_Combo.AddCombo();
        }
    }
}
