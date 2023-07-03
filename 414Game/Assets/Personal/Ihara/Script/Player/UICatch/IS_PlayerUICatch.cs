/**
 * @file   IS_PlayerUICatch.cs
 * @brief  PlayerのUI取得状態クラス
 * @author IharaShota
 * @date   2023/05/21
 * @Update 2023/05/21 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerUICatch : IS_PlayerStrategy
{
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerUICatch)
        {
            // UI取得開始時だったら
            if(IS_Player.instance.GetFlg().m_bStartUICatchFlg)
            {
                // 武器をしていなかったら…
                if(IS_Player.instance.GetSetEquipState == EquipState.EquipNone)
                {
                    // 武器装備
                    IS_Player.instance.EquipWeapon();
                }
                // 装備していたら…
                else
                {
                    // 武器変更
                    IS_Player.instance.ChangeWeapon();
                }

                IS_Player.instance.GetFlg().m_bStartUICatchFlg = false;
            }
            // =========
            // 状態遷移
            // =========
            // 「UI取得 → 待機」
            if (IS_Player.instance.GetPlayerAnimator().AnimEnd(PlayerAnimState.UICatch))
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
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
        IS_Player.instance.m_vMoveAmount = new Vector3(0f, 0f, 0f);
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.UICatch);
    }
}
