/**
 * @file   IS_PlayerJump.cs
 * @brief  Playerの跳躍クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/03/12 アニメーション処理追加
 * @Update 2023/04/17 SE実装
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerJump : IS_PlayerStrategy
{
    [SerializeField] private float m_fJumpPow;// 跳躍力
    [SerializeField] private float m_fMovePow;// 移動する力
    [SerializeField] ParticleSystem jumpEffect;

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerJump)
        {
            // 跳躍開始時に跳躍力を合計移動量に加算
            if (IS_Player.instance.GetFlg().m_bJumpFlg)
            {
                // エフェクト再生
                ParticleSystem Effect = Instantiate(jumpEffect);
                Effect.Play();
                Effect.transform.position = this.transform.position;
                Effect.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(Effect.gameObject, 1.0f); // 1秒後に消える

                IS_AudioManager.instance.PlaySE(SEType.SE_PlayerJump);
                IS_Player.instance.GetSetMoveAmount = new Vector3(0f, m_fJumpPow, 0f);
                IS_Player.instance.GetFlg().m_bJumpFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            // 「跳躍 → 落下」
            if (IS_Player.instance.m_vMoveAmount.y <= 0.0f)
            {
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                return;
            }
            // 「跳躍 → 跳躍攻撃」
            if (Input.GetKeyDown(IS_XBoxInput.X))
            {
                if (IS_Player.instance.GetSetEquipState == EquipState.EquipHpBar ||
                   IS_Player.instance.GetSetEquipState == EquipState.EquipBossBar ||
                   IS_Player.instance.GetSetEquipState == EquipState.EquipStart)
                {
                    IS_Player.instance.GetFlg().m_bJumpAttackFlg = true;
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerJumpAttack;
                    return;
                }
            }
        }
    }
    /**
     * @fn
     * 更新処理
     * @brief  Playerの跳躍更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // アニメーション更新
        UpdateAnim();

        // 合計移動量をリセット(y成分はリセットしない)
        IS_Player.instance.GetSetMoveAmount =
            new Vector3(0f, IS_Player.instance.GetSetMoveAmount.y, 0f);

        // 左向きに移動
        if (IS_XBoxInput.LStick_H >= 0.2)
        {
            IS_Player.instance.m_vMoveAmount.x -= m_fMovePow;
            IS_Player.instance.GetSetPlayerDir = PlayerDir.Left;
        }
        // 右向きに移動
        if (IS_XBoxInput.LStick_H <= -0.2)
        {
            IS_Player.instance.m_vMoveAmount.x += m_fMovePow;
            IS_Player.instance.GetSetPlayerDir = PlayerDir.Right;
        }

        // 重力を合計移動量に加算
        IS_Player.instance.m_vMoveAmount.y += IS_Player.instance.GetParam().m_fGravity;
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
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpHPBar);
                break;
            case EquipState.EquipSkillIcon:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpSkillIcon);
                break;
            case EquipState.EquipBossBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpBossBar);
                break;
            case EquipState.EquipClock:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpClock);
                break;
            case EquipState.EquipStart:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpHPBar);
                break;
            default:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Jump);
                break;
        }
    }
}
