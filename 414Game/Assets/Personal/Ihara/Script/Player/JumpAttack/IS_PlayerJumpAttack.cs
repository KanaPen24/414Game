/**
 * @file   IS_PlayerJumpAttack.cs
 * @brief  Playerの跳躍攻撃クラス
 * @author IharaShota
 * @date   2023/05/29
 * @Update 2023/05/29 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerJumpAttack : IS_PlayerStrategy
{
    [SerializeField] IS_Player m_Player;       // IS_Playerをアタッチする
    [SerializeField] IS_PlayerGroundCollision　m_PlayerGroundCollision; // 地面判定

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerJumpAttack)
        {
            // 跳躍攻撃開始時に
            if (m_Player.GetSetJumpAttackFlg)
            {
                m_Player.GetSetJumpAttackFlg = false;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).StartAttack();
            }

            // =========
            // 状態遷移
            // =========
            // 「跳躍攻撃 → 待ち」
            if (m_PlayerGroundCollision.IsGroundCollision())
            {
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).FinAttack();
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                return;
            }
        }
    }
    /**
     * @fn
     * 更新処理
     * @brief  Playerの跳躍更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // アニメーション更新
        UpdateAnim();

        // 合計移動量をリセット(y成分はリセットしない)
        m_Player.GetSetMoveAmount =
            new Vector3(0f, m_Player.GetSetMoveAmount.y, 0f);

        // 重力を合計移動量に加算
        m_Player.m_vMoveAmount.y += m_Player.GetSetGravity;
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        if (m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
        {
            switch (m_Player.GetSetEquipWeaponState)
            {
                case EquipWeaponState.PlayerHpBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpAttackHPBar);
                    break;
                case EquipWeaponState.PlayerBossBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpAttackHPBar);
                    break;
                case EquipWeaponState.PlayerStart:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpAttackHPBar);
                    break;
            }
        }
    }
}
