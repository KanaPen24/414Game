/**
 * @file   IS_PlayerJumpAttack.cs
 * @brief  Playerのジャンプ攻撃クラス
 * @author IharaShota
 * @date   2023/04/26
 * @Update 2023/04/26 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerJumpAttack : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player; // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerJumpAttack)
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
            // 「跳躍攻撃 → 待機」
            if (!m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                //m_Player.GetAnimator().SetBool("isWait", true);
                //m_Player.GetAnimator().SetBool("isAttack", false);
                return;
            }
            // 「攻撃 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack = false;
                ///m_Player.GetAnimator().SetBool("isDrop", true);
                //m_Player.GetAnimator().SetBool("isAttack", false);
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

        // 合計移動量をリセット(y成分はリセットしない)
        m_Player.GetSetMoveAmount =
            new Vector3(0f, m_Player.GetSetMoveAmount.y, 0f);

        // 指定した武器で攻撃処理
        m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).UpdateAttack();

        // 重力を合計移動量に加算
        m_Player.m_vMoveAmount.y += m_Player.GetSetGravity;
    }
}
