/**
 * @file   IS_WeaponBall.cs
 * @brief  Ballの武器クラス
 * @author IharaShota
 * @date   2023/03/18
 * @Update 2023/03/18 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IS_WeaponBall : IS_Weapon
{
    [SerializeField] private IS_Player m_Player; // Player    
    [SerializeField] private IS_Ball m_Ball;     // 生成Ball
    [SerializeField] private float fInitVel;     // 初期速度
    [SerializeField] private float m_fAttackRate;// 攻撃速度     

    private float m_fRateAmount;                   // 割合の合計
    private int nVisible;                          // bool型を格納するための変数

    private void Start()
    {
        m_eWeaponType = WeaponType.Ball; // 武器種類はBall
        GetSetAttack = false;
    }

    private void FixedUpdate()
    {
        UpdateVisible();
    }

    /**
     * @fn
     * 表示更新処理
     * @brief MeshRenderとColliderのコンポーネントのチェック有無
     */
    private void UpdateVisible()
    {
        if (nVisible != Convert.ToInt32(m_bVisible))
        {
            if (m_bVisible)
            {
            }
            else
            {
            }
        }
        nVisible = Convert.ToInt32(m_bVisible);
    }

    /**
    * @fn
    * 攻撃初期化処理(override)
    * @brief 攻撃初期化処理
    */
    public override void StartAttack()
    {
        GetSetAttack = true; // 攻撃ON
        GameObject shot = Instantiate(m_Ball.gameObject, this.transform.position, this.transform.rotation); // Ball生成
        IS_Ball Shot = shot.GetComponent<IS_Ball>();   // コンポーネントの取得
        Shot.Fire(fInitVel, m_Player.GetSetPlayerDir); // 弾発射
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
    }

    /**
     * @fn
     * 攻撃更新処理(override)
     * @brief 攻撃処理
     */
    public override void UpdateAttack()
    {
        // ここに処理を加える

        // 攻撃仕切ったら終了する
        if (m_fRateAmount >= 1.0f)
        {
            FinAttack();
        }
        else m_fRateAmount += m_fAttackRate;
    }
}
