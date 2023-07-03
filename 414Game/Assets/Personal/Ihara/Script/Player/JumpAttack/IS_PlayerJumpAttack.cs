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
    [SerializeField] IS_PlayerGroundCollision　m_PlayerGroundCollision; // 地面判定

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerJumpAttack)
        {
            // 跳躍攻撃開始時に
            if (IS_Player.instance.GetFlg().m_bStartJumpAttackFlg)
            {
                IS_Player.instance.GetFlg().m_bStartJumpAttackFlg = false;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).StartAttack();
            }

            // =========
            // 状態遷移
            // =========
            // 「跳躍攻撃 → 待ち」
            if (m_PlayerGroundCollision.IsGroundCollision())
            {
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinAttack();
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
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
        IS_Player.instance.m_vMoveAmount =
            new Vector3(0f, IS_Player.instance.m_vMoveAmount.y, 0f);

        // 重力を合計移動量に加算
        IS_Player.instance.m_vMoveAmount.y += IS_Player.instance.GetParam().m_fGravity;
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        switch (IS_Player.instance.GetSetEquipState)
        {
            case EquipState.EquipHpBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpAttackHPBar);
                break;
            case EquipState.EquipBossBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpAttackHPBar);
                break;
            case EquipState.EquipStart:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpAttackHPBar);
                break;
        }
    }
}
