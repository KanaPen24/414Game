/**
 * @file   IS_PlayerWait.cs
 * @brief  Playerの待機クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/03/10「待機」→「攻撃」への処理追加
 * @Update 2023/03/12 アニメーション処理追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerWait : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player; // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    /**
     * @fn
     * 更新処理
     * @brief  Playerの待機更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // ここにStateごとに処理を加える
        //Debug.Log("PlayerWait");

        // 合計移動量をリセット
        m_Player.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

        // =========
        // 状態遷移
        // =========
        //「待機 → 落下」
        if (!m_PlayerGroundColl.IsGroundCollision())
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
            m_Player.GetAnimator().SetBool("isDrop", true);
            m_Player.GetAnimator().SetBool("isWait", false);
            return;
        }
        //「待機 → 跳躍」
        if (m_Player.bInputUp)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerJump;
            m_Player.GetAnimator().SetBool("isJump", true);
            m_Player.GetAnimator().SetBool("isWait", false);
            m_Player.GetSetJumpFlg = true;
            return;
        }
        // 「待機 → 移動」
        if (m_Player.bInputRight || m_Player.bInputLeft)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerWalk;
            m_Player.GetAnimator().SetBool("isWalk", true);
            m_Player.GetAnimator().SetBool("isWait", false);
            return;
        }
        // 「待機 → 攻撃」
        if (m_Player.bInputSpace && m_Player.GetSetEquipFlg)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerAttack;
            m_Player.GetSetAttackFlg = true;
            m_Player.GetAnimator().SetBool("isAttack", true);
            m_Player.GetAnimator().SetBool("isWait", false);
            return;
        }
    }
}
