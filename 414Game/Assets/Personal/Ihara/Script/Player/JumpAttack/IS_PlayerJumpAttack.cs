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
    [SerializeField] IS_Player m_Player;      // IS_Playerをアタッチする

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerJumpAttack)
        {
            // 跳躍攻撃開始時に
            if (m_Player.GetSetAttackFlg)
            {
                m_Player.GetSetAttackFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            // 「跳躍攻撃 → 落下」
            if (m_Player.GetSetMoveAmount.y <= 0.0f)
            {
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
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpHPBar);
                    break;
                case EquipWeaponState.PlayerSkillIcon:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpSkillIcon);
                    break;
                case EquipWeaponState.PlayerBossBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpBossBar);
                    break;
                case EquipWeaponState.PlayerClock:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpClock);
                    break;
                case EquipWeaponState.PlayerStart:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpHPBar);
                    break;
            }
        }
        else m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Jump);
    }
}
