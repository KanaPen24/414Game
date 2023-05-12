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
                //m_Player.GetAnimator().SetBool("isDrop", true);
                m_Player.GetAnimator().SetBool("isWalk", false);
                return;
            }
            // 「移動 → 待機」①
            if (m_Player.GetSetReactionFlg)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                m_Player.GetAnimator().SetBool("isWait", true);
                m_Player.GetAnimator().SetBool("isWalk", false);
                return;
            }
            // 「移動 → 待機」②
            if (!m_Player.bInputRight && !m_Player.bInputLeft)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerWait;
                m_Player.GetAnimator().SetBool("isWait", true);
                m_Player.GetAnimator().SetBool("isWalk", false);
                return;
            }
            // 「移動 → 跳躍」
            if (m_Player.bInputUp)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerJump;
                m_Player.GetAnimator().SetBool("isJump", true);
                m_Player.GetAnimator().SetBool("isWalk", false);
                m_Player.GetSetJumpFlg = true;
                return;
            }
            // 「移動 → 攻撃」
            if (m_Player.bInputSpace && 
                m_Player.GetSetPlayerEquipState == PlayerEquipState.Equip)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                m_Player.GetSetPlayerState = PlayerState.PlayerAttack;
                m_Player.GetSetAttackFlg = true;
                //m_Player.GetAnimator().SetBool("isAttack", true);
                m_Player.GetAnimator().SetBool("isWalk", false);
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
        // ここにStateごとに処理を加える
        //Debug.Log("PlayerMove");

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
