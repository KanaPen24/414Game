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
    [SerializeField] IS_Player m_Player;      // IS_Playerをアタッチする
    [SerializeField] private float m_fJumpPow;// 跳躍力
    [SerializeField] private float m_fMovePow;// 移動する力
    [SerializeField] ParticleSystem jumpEffect;

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerJump)
        {
            // 跳躍開始時に跳躍力を合計移動量に加算
            if (m_Player.GetSetJumpFlg)
            {
                // エフェクト再生
                ParticleSystem Effect = Instantiate(jumpEffect);
                Effect.Play();
                Effect.transform.position = this.transform.position;
                Effect.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(Effect.gameObject, 1.0f); // 1秒後に消える

                IS_AudioManager.instance.PlaySE(SEType.SE_PlayerJump);
                m_Player.GetSetMoveAmount = new Vector3(0f, m_fJumpPow, 0f);
                m_Player.GetSetJumpFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            // 「跳躍 → 落下」
            if (m_Player.GetSetMoveAmount.y <= 0.0f)
            {
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                return;
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
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpHPBar);
                    break;
                case EquipWeaponState.PlayerSkillIcon:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpSkillIcon);
                    break;
                case EquipWeaponState.PlayerBossBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpBossBar);
                    break;
                case EquipWeaponState.PlayerClock:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpClock);
                    break;
                case EquipWeaponState.PlayerStart:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.JumpHPBar);
                    break;
            }
        }
        else m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Jump);
    }
}
