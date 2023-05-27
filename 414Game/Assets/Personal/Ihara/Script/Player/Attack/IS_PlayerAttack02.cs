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
    [SerializeField] private IS_Player m_Player; // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    private PlayerAnimState m_CurrentPlayerAnimState;

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerAttack02)
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
            // 「攻撃02 → 待機」
            if (m_Player.GetPlayerAnimator().AnimEnd(m_CurrentPlayerAnimState) &&
                !m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「攻撃02 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).FinAttack();
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
        m_Player.GetSetMoveAmount =
            new Vector3(0f, 0f, 0f);

        // 指定した武器で攻撃処理
        m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).UpdateAttack();
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        switch (m_Player.GetSetEquipWeaponState)
        {
            case EquipWeaponState.PlayerHpBar:
                m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack02HPBar);
                m_CurrentPlayerAnimState = PlayerAnimState.Attack02HPBar;
                break;
            case EquipWeaponState.PlayerStart:
                m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Attack02HPBar);
                m_CurrentPlayerAnimState = PlayerAnimState.Attack02HPBar;
                break;
        }
    }
}
