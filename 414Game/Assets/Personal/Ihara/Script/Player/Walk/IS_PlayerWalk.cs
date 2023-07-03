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
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerWalk)
        {
            // 歩行開始時に
            if (IS_Player.instance.GetFlg().m_bStartWalkFlg)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetFlg().m_bStartWalkFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            //「移動 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                return;
            }
            // 「移動 → 待機」
            if (IS_XBoxInput.LStick_H < 0.2 && IS_XBoxInput.LStick_H > -0.2)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerWait;
                return;
            }
            // 「移動 → 跳躍」
            if (Input.GetKeyDown(IS_XBoxInput.B))
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerJump;
                IS_Player.instance.GetFlg().m_bStartJumpFlg = true;
                return;
            }
            // 「移動 → 溜め移動 or 攻撃01」
            if (Input.GetKeyDown(IS_XBoxInput.X) &&
                IS_Player.instance.GetSetEquipState != EquipState.EquipNone)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                if (IS_Player.instance.GetSetEquipState == EquipState.EquipSkillIcon)
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerChargeWalk;
                    IS_Player.instance.GetFlg().m_bStartChargeWaitFlg = true;
                }
                else
                {
                    IS_Player.instance.GetSetPlayerState = PlayerState.PlayerAttack01;
                    IS_Player.instance.GetFlg().m_bStartAttackFlg = true;
                }
                return;
            }
            // 「待機 → 回避」
            if (Input.GetKeyDown(IS_XBoxInput.A) && m_UseSkill.UseSkillJudge())
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerAvoidance;
                IS_Player.instance.GetFlg().m_bStartAvoidFlg = true;
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
        IS_Player.instance.m_vMoveAmount = new Vector3(0f, 0f, 0f);

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
        switch (IS_Player.instance.GetSetEquipState)
        {
            case EquipState.EquipHpBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkHPBar);
                break;
            case EquipState.EquipSkillIcon:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkSkillIcon);
                break;
            case EquipState.EquipBossBar:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkBossBar);
                break;
            case EquipState.EquipClock:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkClock);
                break;
            case EquipState.EquipStart:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.WalkHPBar);
                break;
            default:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.Walk);
                break;
        }
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

        // Playerの向きによってエフェクト位置修正
        if (IS_Player.instance.GetSetPlayerDir == PlayerDir.Right)
        {
            Effect.transform.position = IS_Player.instance.gameObject.transform.position +
                new Vector3(-m_vEffectLocalPos.x, m_vEffectLocalPos.y, m_vEffectLocalPos.z);

        }
        else if (IS_Player.instance.GetSetPlayerDir == PlayerDir.Left)
        {
            Effect.transform.position = IS_Player.instance.gameObject.transform.position +
                new Vector3(+m_vEffectLocalPos.x, m_vEffectLocalPos.y, m_vEffectLocalPos.z);
        }

        Destroy(Effect.gameObject, m_fMaxDustCnt); // m_fMaxDustCnt秒後に消える
    }
}
