/**
 * @file   IS_PlayerWalk.cs
 * @brief  Playerの移動クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 作成
 * @Update 2023/03/12 アニメーション処理追加
 * @Update 2023/04/12 歩行エフェクト追加
 * @Update 2023/04/17 歩行SE追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerWalk : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player;                          // IS_Playerをアタッチする
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Playerの地面判定
    [SerializeField] private YK_UseSkill m_UseSkill;                      // 回避できるかのスクリプト
    [SerializeField] private ParticleSystem walkEffect;                   // 歩行エフェクト
    [SerializeField] private float m_fMovePow;                            // 移動する力
    [SerializeField] private float m_fMaxDustCnt;                         // 歩行エフェクト最大カウント
    [SerializeField] private Vector3 m_vEffectLocalPos;                   // エフェクトローカル座標
    private float m_fDustCnt;                                             // 歩行エフェクトカウント                                         //

    /**
     * @fn
     * 初期化処理(外部参照を除く)
     * @brief  メンバ初期化処理
     * @detail 特に無し
     */
    private void Awake()
    {
        m_fDustCnt = 0f;
    }

    private void Update()
    {
        if (m_Player.GetSetPlayerState == PlayerState.PlayerWalk)
        {
            // 歩行開始時に
            if (m_Player.GetSetWalkFlg)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_PlayerWalk);
                m_Player.GetSetWalkFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            //「移動 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
                return;
            }
            // 「移動 → 待機」
            if (!m_Player.bInputRight && !m_Player.bInputLeft)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「移動 → 跳躍」
            if (m_Player.bInputJump)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerJump;
                m_Player.GetSetJumpFlg = true;
                return;
            }
            // 「移動 → 溜め移動 or 攻撃01」
            if (m_Player.bInputAttack &&
                m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                if (m_Player.GetSetEquipWeaponState == EquipWeaponState.PlayerSkillIcon)
                {
                    m_Player.GetSetPlayerState = PlayerState.PlayerChargeWalk;
                    m_Player.GetSetChargeWalkFlg = true;
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
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerAvoidance;
                m_Player.GetSetAvoidFlg = true;
                return;
            }
        }
    }

    /**
     * @fn
     * 更新処理
     * @brief  Playerの移動更新処理
     * @detail 継承元からoverrideしています
     */
    public override void UpdateStrategy()
    {
        // アニメーション更新
        UpdateAnim();

        // 合計移動量をリセット
        m_Player.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

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

        // m_fMaxDustCntの数値間隔で砂埃エフェクト発生
        if (m_fDustCnt >= m_fMaxDustCnt)
        {
            m_fDustCnt = 0f;
            StartDust();
        }
        else m_fDustCnt += Time.deltaTime;
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
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkHPBar);
                    break;
                case EquipWeaponState.PlayerSkillIcon:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkSkillIcon);
                    break;
                case EquipWeaponState.PlayerBossBar:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkBossBar);
                    break;
                case EquipWeaponState.PlayerClock:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkClock);
                    break;
                case EquipWeaponState.PlayerStart:
                    m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkHPBar);
                    break;
            }
        }
        else m_Player.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Walk);
    }

    /**
     * @fn
     * 砂埃エフェクト発生
     * @brief  Playerの歩行エフェクト発生
     * @detail 特に無し?
     */
    private void StartDust()
    {
        // エフェクト再生
        ParticleSystem Effect = Instantiate(walkEffect);
        Effect.Play();
        Effect.transform.localScale = new Vector3(1f, 1f, 1f);

        // SE再生
        //IS_AudioManager.instance.PlaySE(SEType.SE_PlayerWalk);

        // Playerの向きによってエフェクト位置修正
        if (m_Player.GetSetPlayerDir == PlayerDir.Right)
        {
            Effect.transform.position = m_Player.gameObject.transform.position +
                new Vector3(-m_vEffectLocalPos.x, m_vEffectLocalPos.y, m_vEffectLocalPos.z);

        }
        else if (m_Player.GetSetPlayerDir == PlayerDir.Left)
        {
            Effect.transform.position = m_Player.gameObject.transform.position +
                new Vector3(+m_vEffectLocalPos.x, m_vEffectLocalPos.y, m_vEffectLocalPos.z);
        }

        Destroy(Effect.gameObject, m_fMaxDustCnt); // m_fMaxDustCnt秒後に消える
    }
}
