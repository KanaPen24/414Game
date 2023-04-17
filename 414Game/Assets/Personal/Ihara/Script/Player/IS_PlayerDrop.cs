﻿/**
 * @file   IS_PlayerDrop.cs
 * @brief  Playerの落下クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/03/12 アニメーション処理追加
 * @Update 2023/04/17 SE実装
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerDrop : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player;                          // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private float m_fMovePow;                            // 移動する力

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerDrop)
        {
            // =========
            // 状態遷移
            // =========
            // 「落下 → 待機 or 移動」
            if (m_PlayerGroundColl.IsGroundCollision())
            {
                // SE再生
                IS_AudioManager.instance.PlaySE(SEType.SE_PlayerLanding);

                if (m_Player.bInputLeft || m_Player.bInputRight)
                {
                    m_Player.GetSetPlayerState = PlayerState.PlayerWalk;
                    m_Player.GetAnimator().SetBool("isWalk", true);
                    m_Player.GetAnimator().SetBool("isDrop", false);
                    return;
                }

                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                m_Player.GetAnimator().SetBool("isWait", true);
                m_Player.GetAnimator().SetBool("isDrop", false);
                return;
            }
        }
    }
    /**
     * @fn
     * 更新処理
     * @brief  Playerの落下更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
        //Debug.Log("PlayerDrop");

        // 合計移動量をリセット(y成分はリセットしない)
        m_Player.GetSetMoveAmount =
            new Vector3(0f, m_Player.GetSetMoveAmount.y, 0f);

        // DAキーで移動する
        if (m_Player.bInputRight)
        {
            m_Player.m_vMoveAmount.x += m_fMovePow;
            m_Player.GetSetPlayerDir = PlayerDir.Right;
        }
        if (m_Player.bInputLeft)
        {
            m_Player.m_vMoveAmount.x -= m_fMovePow;
            m_Player.GetSetPlayerDir = PlayerDir.Left;
        }

        // 重力を合計移動量に加算
        m_Player.m_vMoveAmount.y += m_Player.GetSetGravity;
    }
}
