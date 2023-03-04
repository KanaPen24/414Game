/**
 * @file   IS_PlayerDrop.cs
 * @brief  Playerの落下クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerDrop : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player;                          // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private float m_fMovePow;                            // 移動する力

    /**
     * @fn
     * 更新処理
     * @brief  Playerの落下更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
        Debug.Log("PlayerDrop");

        // 合計移動量をリセット(y成分はリセットしない)
        m_Player.GetSetMoveAmount =
            new Vector3(0f, m_Player.GetSetMoveAmount.y, 0f);

        // DAキーで移動する
        if (m_Player.bInputRight)
        {
            m_Player.m_vMoveAmount.x += m_fMovePow;
        }
        if (m_Player.bInputLeft)
        {
            m_Player.m_vMoveAmount.x -= m_fMovePow;
        }

        // 重力を合計移動量に加算
        m_Player.m_vMoveAmount.y += m_Player.GetSetGravity;

        // =========
        // 状態遷移
        // =========
        // 「落下 → 待機」
        if (m_PlayerGroundColl.IsGroundCollision())
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerWait;
            return;
        }
    }
}
