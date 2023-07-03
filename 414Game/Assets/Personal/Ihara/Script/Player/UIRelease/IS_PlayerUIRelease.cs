/**
 * @file   IS_PlayerUIRelease.cs
 * @brief  PlayerのUI解放状態クラス
 * @author IharaShota
 * @date   2023/05/21
 * @Update 2023/05/21 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerUIRelease : IS_PlayerStrategy
{
    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerUIRelease)
        {
            // UI取得開始時だったら
            if (IS_Player.instance.GetFlg().m_bStartUIReleaseFlg)
            {
                // 装備解除
                IS_Player.instance.RemovedWeapon();

                IS_Player.instance.GetFlg().m_bStartUIReleaseFlg = false;
            }
            // =========
            // 状態遷移
            // =========
            // 「UI取得 → 待機」
            //if (IS_Player.instance.GetPlayerAnimator().AnimEnd(PlayerAnimState.UICatch))
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
        //IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.UICatch);
    }
}
