/**
 * @file   IS_PlayerChargeWalk.cs
 * @brief  Playerの溜め移動クラス
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/05/17 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerChargeWalk : IS_PlayerStrategy
{
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
        if (IS_Player.instance.GetSetPlayerState == PlayerState.PlayerChargeWalk)
        {
            // 溜め移動開始時に
            if (IS_Player.instance.GetFlg().m_bChargeWalkFlg)
            {
                if (!IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).GetSetCharge)
                {
                    IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).StartCharge();
                    IS_AudioManager.instance.PlaySE(SEType.SE_PlayerWalk);
                }
                IS_Player.instance.GetFlg().m_bChargeWalkFlg = false;
            }

            // =========
            // 状態遷移
            // =========
            //「溜め移動 → 落下」
            if (!m_PlayerGroundColl.IsGroundCollision())
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerDrop;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinCharge();
                return;
            }
            // 「溜め移動 → 溜め待機」
            if (IS_XBoxInput.LStick_H < 0.2 && IS_XBoxInput.LStick_H > -0.2)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerChargeWait;
                IS_Player.instance.GetFlg().m_bChargeWaitFlg = true;
                return;
            }
            // 「溜め移動 → 攻撃01」
            if (!Input.GetKey(IS_XBoxInput.X) &&
                IS_Player.instance.GetSetEquipState != EquipState.EquipNone)
            {
                IS_AudioManager.instance.StopSE(SEType.SE_PlayerWalk);
                IS_Player.instance.GetSetPlayerState = PlayerState.PlayerAttack01;
                IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).FinCharge();
                IS_Player.instance.GetFlg().m_bAttackFlg = true;
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
        IS_Player.instance.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

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

        // 指定した武器で溜め処理
        IS_Player.instance.GetWeapons((int)IS_Player.instance.GetSetEquipState).UpdateCharge();
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
            case EquipState.EquipSkillIcon:
                IS_Player.instance.GetPlayerAnimator().ChangeAnim(PlayerAnimState.ChargeWalkSkillIcon);
                break;
        }
    }
}
