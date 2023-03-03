/**
 * @file   IS_PlayerMove.cs
 * @brief  Playerの移動クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerMove : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player;                          // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private float m_fMovePow;                            // 移動する力
    /**
     * @fn
     * 更新処理
     * @brief  Playerの移動更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
        Debug.Log("PlayerMove");

        // 合計移動量をリセット
        m_Player.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

        // DAキーで移動する
        if (m_Player.bInputRight)
        {
            m_Player.m_vMoveAmount.x += m_fMovePow;
        }
        if (m_Player.bInputLeft)
        {
            m_Player.m_vMoveAmount.x -= m_fMovePow;
        }

        // =========
        // 状態遷移
        // =========
        //「移動 → 落下」
        if (!m_PlayerGroundColl)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
            return;
        }
        // 「移動 → 跳躍」
        //  Wキーで跳躍する
        if (m_Player.bInputUp)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerJump;
            m_Player.GetSetJumpFlg = true;
            return;
        }
        // 「移動 → 待機」
        if (!m_Player.bInputRight && !m_Player.bInputLeft)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerWait;
            return;
        }
    }
}
