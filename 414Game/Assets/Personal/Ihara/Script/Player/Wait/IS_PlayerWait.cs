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
    [SerializeField] private IS_Player m_Player;                          // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private YK_UseSkill m_UseSkill;                      // 回避できるかのスクリプト

    private void Update()
    {
        if(m_Player.GetSetPlayerState == PlayerState.PlayerWait)
        {
            // 反動があれば状態遷移しない
            if (m_Player.GetSetReactionFlg)
                return;

            // =========
            // 状態遷移
            // =========
            //「待機 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                return;
            }
            //「待機 → 跳躍」
            if (m_Player.bInputJump)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerJump;
                m_Player.GetSetJumpFlg = true;
                return;
            }
            // 「待機 → 移動」
            if (m_Player.bInputRight || m_Player.bInputLeft)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerWalk;
                m_Player.GetSetWalkFlg = true;
                return;
            }
            // 「待機 → 溜め待機 or 攻撃01 」
            if (m_Player.bInputAttack &&
                m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
            {
                if (m_Player.GetSetEquipWeaponState == EquipWeaponState.PlayerSkillIcon)
                {
                    m_Player.GetSetPlayerState = PlayerState.PlayerChargeWait;
                    m_Player.GetSetChargeWaitFlg = true;
                }
                else
                {
                    m_Player.GetSetPlayerState = PlayerState.PlayerAttack01;
                    m_Player.GetSetAttackFlg = true;
                }
                return;
            }
            // 「待機 → 回避」
            if (m_Player.bInputAvoid && m_UseSkill.UseSkillJudge())
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerAvoidance;
                m_Player.GetSetAvoidFlg = true;
                return;
            }
            // 「待機 → 回避」
            if (Input.GetButtonDown("Decision") || Input.GetButtonDown("Decision_Debug"))
            {

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
        if (m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
        {
            switch (m_Player.GetSetEquipWeaponState)
            {
                case EquipWeaponState.PlayerHpBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitHPBar);
                    break;
                case EquipWeaponState.PlayerSkillIcon:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitSkillIcon);
                    break;
                case EquipWeaponState.PlayerBossBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitBossBar);
                    break;
                case EquipWeaponState.PlayerClock:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitClock);
                    break;
            }
        }
        else m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Wait);
    }
}
