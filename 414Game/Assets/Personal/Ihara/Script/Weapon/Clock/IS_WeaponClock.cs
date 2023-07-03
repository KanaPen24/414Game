/**
 * @file   IS_WeaponClock.cs
 * @brief  時計武器クラス
 * @author IharaShota
 * @date   2023/05/05
 * @Update 2023/05/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class IS_WeaponClock : IS_Weapon
{
    [SerializeField] private IS_Player Player;
    [SerializeField] private MeshRenderer m_MeshRender;        // メッシュ
    [SerializeField] private Image Clock_Inner;     //時計の赤い部分
    [SerializeField] private YK_Clock clock;
    private int m_nCnt;

    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照は避けること)
     */
    protected override void Awake()
    {
        // メンバの初期化
        m_eWeaponType = WeaponType.Clock; // 武器種類は時計
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
        IS_AudioManager.instance.PlaySE(SEType.SE_StopTime);
        IS_AudioManager.instance.GetBGM(BGMType.BGM_Game).Pause();
        // 攻撃ON
        IS_Player.instance.GetFlg().m_bAttack = true;
        clock.GetSetStopTime = true;
    }

    /**
     * @fn
     * 攻撃終了処理(override)
     * @brief 攻撃終了処理
     */
    public override void FinAttack()
    {
        IS_Player.instance.GetFlg().m_bAttack = false; // 攻撃OFF
    }

    /**
     * @fn
     * 攻撃更新処理(override)
     * @brief 攻撃処理
     */
    public override void UpdateAttack()
    {
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
            m_MeshRender.enabled = true;
            Clock_Inner.enabled = true;
        }
        // 非表示状態だったら
        else
        {
            m_MeshRender.enabled = false;
            Clock_Inner.enabled = false;
        }
    }
}
