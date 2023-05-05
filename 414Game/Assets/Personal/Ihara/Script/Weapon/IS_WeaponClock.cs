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

public class IS_WeaponClock : IS_Weapon
{
    [SerializeField] private IS_Player Player;
    [SerializeField] private YK_Clock clock;
    [SerializeField] private float m_fPlayerMovePow;
    /**
     * @fn
     * 初期化処理(override前提)
     * @brief 初期化処理(外部参照は避けること)
     */
    protected override void Awake()
    {
        // メンバの初期化
        m_eWeaponType = WeaponType.Clock; // 武器種類は時計
        m_bAttack = false;
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
        // 攻撃ON
        GetSetAttack = true;
        clock.GetSetStopTime = true;
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
        // 右向き
        if (Player.bInputRight)
        {
            Player.m_vMoveAmount.x += m_fPlayerMovePow;
            Player.GetSetPlayerDir = PlayerDir.Right;
        }
        // 左向き
        if (Player.bInputLeft)
        {
            Player.m_vMoveAmount.x -= m_fPlayerMovePow;
            Player.GetSetPlayerDir = PlayerDir.Left;
        }

        if (!clock.GetSetStopTime)
        {
            FinAttack();
        }
    }

    /**
     * @fn
     * 表示更新処理(override)
     * @brief 表示更新処理
     */
    public override void UpdateVisible()
    {
    }
}
