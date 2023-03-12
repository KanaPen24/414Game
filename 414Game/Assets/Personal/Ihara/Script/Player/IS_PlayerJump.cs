/**
 * @file   IS_PlayerJump.cs
 * @brief  Playerの跳躍クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/03/12 アニメーション処理追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerJump : IS_PlayerStrategy
{
    [SerializeField] IS_Player m_Player;      // IS_Playerをアタッチする
    [SerializeField] private float m_fJumpPow;// 跳躍力
    [SerializeField] private float m_fMovePow;// 移動する力
    /**
     * @fn
     * 更新処理
     * @brief  Playerの跳躍更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
        //Debug.Log("PlayerJump");

        // 跳躍開始時に跳躍力を合計移動量に加算
        if(m_Player.GetSetJumpFlg)
        {
            m_Player.GetSetMoveAmount = new Vector3(0f, m_fJumpPow, 0f);
            m_Player.GetSetJumpFlg = false;
        }

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

        // =========
        // 状態遷移
        // =========
        // 「跳躍 → 落下」
        if (m_Player.GetSetMoveAmount.y <= 0.0f)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
            m_Player.GetAnimator().SetBool("isDrop", true);
            m_Player.GetAnimator().SetBool("isJump", false);
            return;
        }
    }
}
