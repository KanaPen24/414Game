/**
 * @file   IS_WeaponBossBar.cs
 * @brief  BossBarの武器クラス
 * @author IharaShota
 * @date   2023/04/04
 * @Update 2023/04/04 作成(IS_WeaponHPBarからコピー)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IS_WeaponBossBar : IS_Weapon
{
    [SerializeField] private IS_Player m_Player;               // Player
    [SerializeField] private CapsuleCollider m_CapsuleCollider;// 当たり判定
    [SerializeField] private MeshRenderer m_MeshRender;        // メッシュ
    private int m_nCnt;

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照は避けること)
     */
    protected override void Awake()
    {
        // メンバの初期化
        m_eWeaponType = WeaponType.BossBar; // 武器種類はBossバー
        m_bAttack = false;
        m_bCharge = false;
        m_bVisible = false;
        m_bDestroy = false;
    }

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照する場合)
     */
    protected override void Start()
    {
        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);

        // 表示更新
        UpdateVisible();
    }

    private void Update()
    {
        UpdateVisible();

        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);
    }

    /**
    * @fn
    * 攻撃初期化処理(override)
    * @brief 攻撃初期化処理
    */
    public override void StartAttack()
    {
        // SE再生
        IS_AudioManager.instance.PlaySE(SEType.SE_FireHPBar);

        // 攻撃ON
        GetSetAttack = true;
    }

    /**
     * @fn
     * 攻撃終了処理(override)
     * @brief 攻撃終了処理
     */
    public override void FinAttack()
    {
        GetSetAttack = false; // 攻撃OFF
    }

    /**
     * @fn
     * 攻撃更新処理(override)
     * @brief 攻撃処理
     */
    public override void UpdateAttack()
    {
        if (m_Player.GetPlayerAnimator().AnimEnd(PlayerAnimState.AttackBossBar))
            FinAttack();
    }

    /**
     * @fn
     * 表示更新処理(override)
     * @brief 表示更新処理
     */
    public override void UpdateVisible()
    {
        // 表示状態だったら
        if (m_bVisible)
        {
            m_CapsuleCollider.enabled = true;
            m_MeshRender.enabled = true;
        }
        // 非表示状態だったら
        else
        {
            m_CapsuleCollider.enabled = false;
            m_MeshRender.enabled = false;
        }
    }
}
