﻿/**
 * @file   IS_WeaponHPBar.cs
 * @brief  HPBarの武器クラス
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IS_WeaponHPBar : IS_Weapon
{
    [SerializeField] private IS_Player m_Player;               // Player
    [SerializeField] private CapsuleCollider m_CapsuleCollider;// 当たり判定
    [SerializeField] private MeshRenderer m_MeshRender;        // メッシュ
    [SerializeField] private float fAttackRate;                // 攻撃の割合(スピード)
    [SerializeField] private Vector3 vRotAmount;               // 攻撃の回転量
    [SerializeField] private Vector3 vRotOrigin;               // 攻撃の初期角度

    private float m_fRateAmount;// 割合の合計
    private Vector3 vRot;       // 回転する大きさ
    private int m_nCnt;         

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照は避けること)
     */
    protected override void Awake()
    {
        // メンバの初期化
        m_eWeaponType = WeaponType.HPBar; // 武器種類はHPバー
        m_bAttack  = false;
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
        GetSetAttack = true; // 攻撃ON
    }

    /**
     * @fn
     * 攻撃終了処理(override)
     * @brief 攻撃終了処理
     */
    public override void FinAttack()
    {
        GetSetAttack = false; // 攻撃OFF
        m_fRateAmount = 0.0f;
        this.transform.rotation = Quaternion.Euler(vRotOrigin);
    }

    /**
     * @fn
     * 攻撃更新処理(override)
     * @brief 攻撃処理
     */
    public override void UpdateAttack()
    {
        // ここに処理を加える

        // 回転量を計算
        // 右向きなら
        if (m_Player.GetSetPlayerDir == PlayerDir.Right)
        {
            vRot = new Vector3(-vRotAmount.x * m_fRateAmount,
            -vRotAmount.y * m_fRateAmount,
            -vRotAmount.z * m_fRateAmount);
        }
        // 左向きなら
        else if (m_Player.GetSetPlayerDir == PlayerDir.Left)
        {
            vRot = new Vector3(vRotAmount.x * m_fRateAmount,
                        vRotAmount.y * m_fRateAmount,
                        vRotAmount.z * m_fRateAmount);
        }

        // 武器を回転させる
        this.transform.rotation = Quaternion.Euler(vRot);

        // 攻撃仕切ったら終了する
        if (m_fRateAmount >= 1.0f)
        {
            FinAttack();
        }
        else m_fRateAmount += fAttackRate;
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
