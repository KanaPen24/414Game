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
    [SerializeField] private IS_Player m_Player;                          // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerUICatch)
        {
            // =========
            // 状態遷移
            // =========
            // 「UI取得 → 待機」
            if (m_Player.GetPlayerAnimator().AnimEnd(PlayerAnimState.UICatch))
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
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
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.UICatch);
    }
}
