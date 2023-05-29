/**
 * @file   IS_PlayerChargeWait.cs
 * @brief  Playerの溜め待機クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/05/17 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerChargeWait : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player; // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerChargeWait)
        {
            // 溜め待機開始時に
            if (m_Player.GetSetChargeWaitFlg)
            {
                if(!m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetCharge)
                {
                    m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).StartCharge();
                }
                m_Player.GetSetChargeWaitFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            //「溜め待機 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).FinCharge();
                return;
            }
            // 「溜め待機 → 溜め移動」
            if (m_Player.bInputRight || m_Player.bInputLeft)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerChargeWalk;
                m_Player.GetSetChargeWalkFlg = true;
                return;
            }
            // 「溜め移動 → 攻撃01」
            if (!m_Player.bInputCharge &&
                m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerAttack01;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).FinCharge();
                m_Player.GetSetAttackFlg = true;
                return;
            }
        }
    }

    /**
     * @fn
     * 更新処理
     * @brief  Playerの待機更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // アニメーション更新
        UpdateAnim();

        // 合計移動量をリセット
        m_Player.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

        // 指定した武器で溜め処理
        m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).UpdateCharge();
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
                case EquipWeaponState.PlayerSkillIcon:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.ChargeWaitSkillIcon);
                    break;
            }
        }
    }
}
