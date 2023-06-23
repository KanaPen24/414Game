/**
 * @file   IS_PlayerAttack02.cs
 * @brief  Playerの攻撃02クラス
 * @author IharaShota
 * @date   2023/05/22
 * @Update 2023/05/22 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerAttack02 : IS_PlayerStrategy
{
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    private PlayerAnimState m_CurrentPlayerAnimState;

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerAttack02)
        {
            // 攻撃開始時の処理
            if (IS_Player.instance.GetFlg().m_bAttackFlg)
            {
                IS_Player.instance.GetFlg().m_bAttackFlg = false;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).StartAttack();
            }

            // =========
            // 状態遷移
            // =========
            // 「攻撃02 → 待機」
            if (IS_Player.instance.GetPlayerAnimator().AnimEnd(m_CurrentPlayerAnimState) &&
                !IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).GetSetAttack)
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「攻撃02 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinAttack();
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
        // アニメーション更新
        UpdateAnim();

        // 合計移動量をリセット
        IS_Player.instance.m_vMoveAmount =
            new Vector3(0f, 0f, 0f);

        // 指定した武器で攻撃処理
        IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).UpdateAttack();
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
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack02HPBar);
                m_CurrentPlayerAnimState = PlayerAnimState.Attack02HPBar;
                break;
            case EquipState.EquipStart:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack02HPBar);
                m_CurrentPlayerAnimState = PlayerAnimState.Attack02HPBar;
                break;
        }
    }
}
