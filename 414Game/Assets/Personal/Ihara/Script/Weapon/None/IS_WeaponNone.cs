﻿/*
 * @file   IS_WeaponNone.cs
 * @brief  武器無の武器クラス
 * @author IharaShota
 * @date   2023/06/26
 * @Update 2023/06/26 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IS_WeaponNone : IS_Weapon
{
    [SerializeField] private CapsuleCollider m_CapsuleCollider;// 当たり判定
    private int m_nCnt;

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照は避けること)
     */
    protected override void Awake()
    {
        // メンバの初期化
        m_eWeaponType = WeaponType.None; // 武器種類は武器無
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
        // 前回と状態が違ったら
        if (m_nCnt != Convert.ToInt32(m_bVisible))
        {
            UpdateVisible();
        }

        // 現在の状態に更新
        m_nCnt = Convert.ToInt32(m_bVisible);

        //攻撃中だったら当たり判定をON
        if (GetSetAttack)
        {
            m_CapsuleCollider.enabled = true;
        }
        else m_CapsuleCollider.enabled = false;
    }

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理
     */
    public override void Init()
    {

    }

    /**
     * @fn
     * 終了処理(override前提)
     * @brief 終了処理
     */
    public override void Uninit()
    {

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
        switch (IS_Player.instance.GetSetPlayerState)
        {
            case PlayerState.PlayerAttack01:
                if (IS_Player.instance.GetPlayerAnimator().AnimEnd(PlayerAnimState.Attack01HPBar))
                    FinAttack();
                break;
            case PlayerState.PlayerAttack02:
                if (IS_Player.instance.GetPlayerAnimator().AnimEnd(PlayerAnimState.Attack02HPBar))
                    FinAttack();
                break;
            default:
                break;
        }
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
        }
        // 非表示状態だったら
        else
        {
            m_CapsuleCollider.enabled = false;
        }
    }
}
