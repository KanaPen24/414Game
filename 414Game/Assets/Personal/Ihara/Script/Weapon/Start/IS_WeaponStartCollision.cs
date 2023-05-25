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
    [SerializeField] private IS_Player Player;    // Playerをアタッチ
    [SerializeField] private int m_nDamage2Enemy; // 敵に与えるダメージ量
    [SerializeField] private int m_nDamage2HPBar; // HPBarに与えるダメージ量
    [SerializeField] private int m_nDrainEnemyHp; // 雑魚敵から吸収するHP
    [SerializeField] private int m_nDrainBossHp;  // Bossから吸収するHP
    private void OnTriggerEnter(Collider other)
    {
        // ボスへのダメージ処理
        if (other.gameObject.GetComponent<NK_BossSlime>() != null)
        {
            if (weaponHPBar.GetSetAttack && !other.GetComponent<NK_BossSlime>().GetSetDamageFlag)
            {
                weaponHPBar.GetSetHp -= m_nDamage2HPBar;
                Player.GetSetHp += m_nDrainBossHp;
                other.GetComponent<NK_BossSlime>().BossSlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
            }
        }
        // スライムへのダメージ処理
        if (other.gameObject.GetComponent<NK_Slime>() != null)
        {
            if (weaponHPBar.GetSetAttack && !other.GetComponent<NK_Slime>().GetSetDamageFlag)
            {
                weaponHPBar.GetSetHp -= m_nDamage2HPBar;
                Player.GetSetHp += m_nDrainBossHp;
                other.GetComponent<NK_Slime>().SlimeDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
            }
        }

        // 蝙蝠へのダメージ処理
        if (other.gameObject.GetComponent<NK_Bat>() != null)
        {
            if (weaponHPBar.GetSetAttack && !other.GetComponent<NK_Bat>().GetSetDamageFlag)
            {
                weaponHPBar.GetSetHp -= m_nDamage2HPBar;
                Player.GetSetHp += m_nDrainBossHp;
                other.GetComponent<NK_Bat>().BatDamage(m_nDamage2Enemy);
                other.transform.GetComponent<YK_TakeDamage>().Damage(other, m_nDamage2Enemy);
            }
        }

        // 耐久値が0以下になったらゲームオーバー
        if (weaponHPBar.GetSetHp <= 0)
        {
            Player.GetSetPlayerState = PlayerState.PlayerGameOver;
            GameManager.instance.GetSetGameState = GameState.GameOver;
        }
    }
}
