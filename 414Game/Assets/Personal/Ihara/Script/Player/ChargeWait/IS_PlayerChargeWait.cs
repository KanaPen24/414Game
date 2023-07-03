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
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerChargeWait)
        {
            // 溜め待機開始時に
            if (IS_Player.instance.GetFlg().m_bStartChargeWaitFlg)
            {
                if (!IS_Player.instance.GetFlg().m_bCharge)
                {
                    IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).StartCharge();
                }
                IS_Player.instance.GetFlg().m_bStartChargeWaitFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            //「溜め待機 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinCharge();
                return;
            }
            // 「溜め待機 → 溜め移動」
            if (IS_XBoxInput.LStick_H >= 0.2 || IS_XBoxInput.LStick_H <= -0.2)
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerChargeWalk;
                IS_Player.instance.GetFlg().m_bStartChargeWalkFlg = true;
                return;
            }
            // 「溜め移動 → 攻撃01」
            if (!Input.GetKey(IS_XBoxInput.X) &&
                IS_Player.instance.GetSetEquipState != EquipState.EquipNone)
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerAttack01;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinCharge();
                IS_Player.instance.GetFlg().m_bStartAttackFlg = true;
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
        IS_Player.instance.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

        // 指定した武器で溜め処理
        IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).UpdateCharge();
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
            case EquipState.EquipSkillIcon:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.ChargeWaitSkillIcon);
                break;
        }
    }
}
