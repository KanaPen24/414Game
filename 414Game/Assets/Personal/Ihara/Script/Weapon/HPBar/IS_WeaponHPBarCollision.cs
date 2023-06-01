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
using UnityEngine.VFX;

public class IS_WeaponHPBarCollision : MonoBehaviour
{
    [SerializeField] private IS_WeaponHPBar WeaponHPBar; // HPBar
    [SerializeField] private IS_Player Player;           // Playerをアタッチ
    [SerializeField] private VFX_Drain m_DrainEffect;    // 吸収エフェクト
    [SerializeField] private VisualEffect DrainEffect;    // 吸収エフェクト

    [SerializeField] private int m_nDamage2Enemy;        // 敵に与えるダメージ量
    [SerializeField] private int m_nDamage2HPBar;        // HPBarに与えるダメージ量
    [SerializeField] private int m_nDrainEnemyHp;        // 雑魚敵から吸収するHP
    [SerializeField] private int m_nDrainBossHp;         // Bossから吸収するHP
    [SerializeField] private ParticleSystem HitEffect;  // ヒットエフェクト

    private void OnTriggerEnter(Collider other)
    {
        // ボスへのダメージ処理
        if (other.gameObject.GetComponent<NK_BossSlime>() != null)
        {
            if (WeaponHPBar.GetSetAttack && !other.GetComponent<NK_BossSlime>().GetSetDamageFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponHPBar.GetSetHp -= m_nDamage2HPBar;
                Player.GetSetHp += m_nDrainBossHp;
                other.GetComponent<NK_BossSlime>().BossSlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();

                m_DrainEffect.SetStartPos(other.transform.position);
                m_DrainEffect.GetVisualEffect().Reinit();
                m_DrainEffect.GetVisualEffect().Play();
            }
        }
        // スライムへのダメージ処理
        if (other.gameObject.GetComponent<NK_Slime>() != null)
        {
            if (WeaponHPBar.GetSetAttack && !other.GetComponent<NK_Slime>().GetSetDamageFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponHPBar.GetSetHp -= m_nDamage2HPBar;
                Player.GetSetHp += m_nDrainBossHp;
                other.GetComponent<NK_Slime>().SlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();

                m_DrainEffect.SetStartPos(other.transform.position);
                m_DrainEffect.GetVisualEffect().Reinit();
                m_DrainEffect.GetVisualEffect().Play();
            }
        }

        // 蝙蝠へのダメージ処理
        if (other.gameObject.GetComponent<NK_Bat>() != null)
        {
            if (WeaponHPBar.GetSetAttack && !other.GetComponent<NK_Bat>().GetSetDamageFlag)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
                WeaponHPBar.GetSetHp -= m_nDamage2HPBar;
                Player.GetSetHp += m_nDrainBossHp;
                other.GetComponent<NK_Bat>().BatDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
                HitEffect.transform.position = other.transform.position;
                HitEffect.Play();

                m_DrainEffect.SetStartPos(other.transform.position);
                m_DrainEffect.GetVisualEffect().Reinit();
                m_DrainEffect.GetVisualEffect().Play();
            }
        }

        // スライムベスへのダメージ処理
        if (other.gameObject.GetComponent<NK_SlimeBes>() != null)
        {
            IS_AudioManager.instance.PlaySE(SEType.SE_HitHPBar);
            WeaponHPBar.GetSetHp -= m_nDamage2HPBar;
            Player.GetSetHp += m_nDrainBossHp;
            other.GetComponent<NK_SlimeBes>().BesDamage(m_nDamage2Enemy);
            other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
            HitEffect.transform.position = other.transform.position;
            HitEffect.Play();

            m_DrainEffect.SetStartPos(other.transform.position);
            m_DrainEffect.GetVisualEffect().Reinit();
            m_DrainEffect.GetVisualEffect().Play();
            
        }

        // 耐久値が0以下になったら装備を外す
        if (WeaponHPBar.GetSetHp <= 0)
        {
            Player.RemovedWeapon();
            IS_AudioManager.instance.PlaySE(SEType.SE_HPBarCrack_3);
            //HPバーが壊れたにかえる
            YK_GameOver.instance.GetSetGameOverState = GameOverState.BreakHPBar;
            GameManager.instance.GetSetGameState = GameState.GameOver;
            Player.GetSetPlayerState = PlayerState.PlayerGameOver;
        }
    }
}
