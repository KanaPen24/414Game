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
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private YK_UseSkill m_UseSkill;                      // 回避できるかのスクリプト

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerWait)
        {
            if (GameManager.instance.GetSetGameState == GameState.GameRule)
                return;
            // =========
            // 状態遷移
            // =========
            //「待機 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                return;
            }
            //「待機 → 跳躍」
            if (Input.GetKeyDown(IS_XBoxInput.B))
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerJump;
                IS_Player.instance.GetFlg().m_bJumpFlg = true;
                return;
            }
            // 「待機 → 移動」
            if (IS_XBoxInput.LStick_H >= 0.2 || IS_XBoxInput.LStick_H <= -0.2)
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWalk;
                IS_Player.instance.GetFlg().m_bWalkFlg = true;
                return;
            }
            // 「待機 → 溜め待機 or 攻撃01 」
            if (Input.GetKeyDown(IS_XBoxInput.X) &&
                IS_Player.instance.GetSetEquipState != EquipState.EquipNone)
            {
                if(IS_Player.instance.GetSetEquipState == EquipState.EquipSkillIcon)
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerChargeWait;
                    IS_Player.instance.GetFlg().m_bChargeWaitFlg = true;
                }
                else
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerAttack01;
                    IS_Player.instance.GetFlg().m_bAttackFlg = true;
                }
                return;
            }
            // 「待機 → 回避」
            if (Input.GetKeyDown(IS_XBoxInput.A) && m_UseSkill.UseSkillJudge())
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerAvoidance;
                IS_Player.instance.GetFlg().m_bAvoidFlg = true;
                return;
            }

            // 待機 → UI取得orUI解放」
            if((Input.GetKeyDown(IS_XBoxInput.LB) || Input.GetKeyDown(IS_XBoxInput.RB)) &&
                IS_Player.instance.GetUICatcher().GetSetUICatcherState == UICatcherState.None)
            {
                // カーソルがUIを取得している && カーソルが取得しているUIが現在武器にしているUIではない場合…
                // UI取得に遷移
                if(IS_Player.instance.GetCursolEvent().GetSetCurrentUI != null &&
                   (IS_Player.instance.GetCursolEvent().GetSetCurrentUI != 
                    IS_Player.instance.GetUICatcher().GetSetSelectUI))
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerUICatch;
                    IS_Player.instance.GetFlg().m_bUICatchFlg = true;
                    return;
                }
                // 武器を何も装備していない場合
                // UI解放に遷移
                else if(IS_Player.instance.GetSetEquipState != EquipState.EquipNone)
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerUIRelease;
                    IS_Player.instance.GetFlg().m_bUIReleaseFlg = true;
                    return;
                }
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
        switch (IS_Player.instance.GetSetEquipState)
        {
            case EquipState.EquipHpBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitHPBar);
                break;
            case EquipState.EquipSkillIcon:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitSkillIcon);
                break;
            case EquipState.EquipBossBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitBossBar);
                break;
            case EquipState.EquipClock:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitClock);
                break;
            case EquipState.EquipStart:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WaitHPBar);
                break;
            default:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Wait);
                break;
        }
    }
}
