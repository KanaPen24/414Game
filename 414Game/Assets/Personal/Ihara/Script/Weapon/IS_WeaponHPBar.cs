/**
 * @file   IS_WeaponHPBar.cs
 * @brief  HPBarの武器クラス
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_WeaponHPBar : IS_Weapon
{
    [SerializeField] private IS_Player m_Player;  // Player
    [SerializeField] private Vector3 vRotAmount;  // 攻撃の回転量
    [SerializeField] private Vector3 vRotOrigin;  // 攻撃の初期角度
    [SerializeField] private float fAttackRate;   // 攻撃の割合(スピード)

    private float m_fRateAmount;                   // 割合の合計
    Vector3 vRot;

    private void Start()
    {
        m_eWeaponType = WeaponType.HPBar; // 武器種類はHPバー
        GetSetAttack = false;
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
}
