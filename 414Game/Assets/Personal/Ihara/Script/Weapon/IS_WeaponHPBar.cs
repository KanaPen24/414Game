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
    [SerializeField] private Vector3 vRotAmount;  // 攻撃の回転量
    [SerializeField] private Vector3 vRotOrigin;  // 攻撃の初期角度
    [SerializeField] private float fAttackRate;   // 攻撃の割合(スピード)

    private float m_fRateAmount;                   // 割合の合計

    private void Start()
    {
        //m_vOriginRot = Quaternion.Euler(this.transform.rotation);
        GetSetAttack = false;
    }

    /**
     * @fn
     * 攻撃処理(override)
     * @brief 攻撃処理
     */
    public override void Attack()
    {
        // ここに処理を加える
        // 回転量を計算
        Vector3 vRot =
            new Vector3(vRotAmount.x * fAttackRate, 
                        vRotAmount.y * fAttackRate, 
                        vRotAmount.z * fAttackRate);

        // 武器を回転させる
        //this.transform.position = this.transform.position + vRot;
        this.transform.rotation = this.transform.rotation * Quaternion.Euler(vRot);

        // 攻撃仕切ったら終了する
        if (m_fRateAmount >= 1.0f)
        {
            GetSetAttack = false;
            m_fRateAmount = 0.0f;
            this.transform.rotation = Quaternion.Euler(vRotOrigin);
        }
        else m_fRateAmount += fAttackRate;
    }
}
