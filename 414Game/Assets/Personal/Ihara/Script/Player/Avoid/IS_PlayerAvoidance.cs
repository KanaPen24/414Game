/**
 * @file   IS_PlayerAvoidance.cs
 * @brief  Playerの回避クラス
 * @author IharaShota
 * @date   2023/04/26
 * @Update 2023/04/26 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerAvoidance : IS_PlayerStrategy
{
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private ON_BlurController m_BlurController; // Playerのブラー
    [SerializeField] private float m_fAvoidTime; // 回避時間
    [SerializeField] private float m_MovePow;    // 移動速度

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerAvoidance)
        {
            //回避の音再生
            IS_AudioManager.instance.PlaySE(SEType.SE_Avoidance);

            // 回避開始時の処理
            if (IS_Player.instance.GetFlg().m_bAvoidFlg)
            {
                IS_Player.instance.GetFlg().m_bAvoidFlg = false;
                IS_Player.instance.GetSetPlayerInvincible.GetSetInvincibleCnt = m_fAvoidTime;
                m_BlurController.SetBlur(true);
            }

            // =========
            // 状態遷移
            // =========
            // 「回避 → 待機」
            if (IS_Player.instance.GetSetPlayerInvincible.GetSetInvincibleCnt <= 0f)
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
                m_BlurController.SetBlur(false);
                return;
            }
            // 「回避 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                m_BlurController.SetBlur(false);
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
        if (IS_Player.instance.GetSetPlayerDir == PlayerDir.Right)
        {
            IS_Player.instance.m_vMoveAmount = new Vector3(m_MovePow, 0f, 0f);
        }
        else if (IS_Player.instance.GetSetPlayerDir == PlayerDir.Left)
        {
            IS_Player.instance.m_vMoveAmount = new Vector3(-m_MovePow, 0f, 0f);
        }
    }

    /**
     * @fn
     * アニメーション更新処理
     * @brief Playerのアニメーション更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateAnim()
    {
        IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Avoid);
    }
}