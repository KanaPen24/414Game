/**
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
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private float m_fMovePow;                            // 移動する力
    [SerializeField] ParticleSystem landingEffect;

    private void Update()
    {
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerDrop)
        {
            // =========
            // 状態遷移
            // =========
            // 「落下 → 待機 or 移動」
            if (m_PlayerGroundColl.IsGroundCollision())
            {
                // SE再生
                IS_AudioManager.instance.PlaySE(SEType.SE_PlayerLanding);

                // エフェクト再生
                ParticleSystem Effect = Instantiate(landingEffect);
                Effect.Play();
                Effect.transform.position = this.transform.position;
                Effect.transform.localScale = new Vector3(1f, 1f, 1f);
                Destroy(Effect.gameObject, 1.0f); // 1秒後に消える

                if (IS_XBoxInput.LStick_H >= 0.2 || IS_XBoxInput.LStick_H <= -0.2)
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWalk;
                    IS_Player.instance.GetFlg().m_bStartWalkFlg = true;
                    return;
                }

                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「落下 → 跳躍攻撃」
            if (Input.GetKeyDown(IS_XBoxInput.X))
            {
                if (IS_Player.instance.GetSetEquipState == EquipState.EquipHpBar ||
                   IS_Player.instance.GetSetEquipState == EquipState.EquipBossBar ||
                   IS_Player.instance.GetSetEquipState == EquipState.EquipStart)
                {
                    IS_Player.instance.GetFlg().m_bStartJumpAttackFlg = true;
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerJumpAttack;
                    return;
                }
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
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.DropHPBar);
                break;
            case EquipState.EquipSkillIcon:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.DropSkillIcon);
                break;
            case EquipState.EquipBossBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.DropBossBar);
                break;
            case EquipState.EquipClock:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.DropClock);
                break;
            case EquipState.EquipStart:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.DropHPBar);
                break;
            default:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Drop);
                break;
        }
    }
}
