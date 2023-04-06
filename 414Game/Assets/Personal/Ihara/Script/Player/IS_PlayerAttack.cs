/**
 * @file   IS_PlayerAttack.cs
 * @brief  Playerの攻撃クラス
 * @author IharaShota
 * @date   2023/03/10
 * @Update 2023/03/10 作成
 * @Update 2023/03/12 アニメーション処理追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerAttack : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player; // IS_Playerをアタッチする

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerAttack)
        {
            // 攻撃開始時の処理
            if (m_Player.GetSetAttackFlg)
            {
                m_Player.GetSetAttackFlg = false;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).StartAttack();
            }

            // =========
            // 状態遷移
            // =========
            // 「攻撃 → 待機」
            if (!m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                m_Player.GetAnimator().SetBool("isWait", true);
                m_Player.GetAnimator().SetBool("isAttack", false);
                return;
            }
        }
    }
    /**
     * @fn
     * 更新処理
     * @brief  Playerの攻撃更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
        //Debug.Log("PlayerAttack");

        // 合計移動量をリセット
        m_Player.GetSetMoveAmount =
            new Vector3(0f, 0f, 0f);

        // 指定した武器で攻撃処理
        m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).UpdateAttack();
    }

    /**
     * @fn
     * Playerの攻撃処理
     * @brief  方法はまだ考え中
     * @detail 制作中
     */
    private void Attack()
    {

    }
}
