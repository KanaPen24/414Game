/**
 * @file   IS_WeaponHPBar.cs
 * @brief  HPBar‚Ì•ŠíƒNƒ‰ƒX
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 ì¬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_WeaponHPBar : IS_Weapon
{
    [SerializeField] private Vector3 vRotAmount;  // UŒ‚‚Ì‰ñ“]—Ê
    [SerializeField] private Vector3 vRotOrigin;  // UŒ‚‚Ì‰ŠúŠp“x
    [SerializeField] private float fAttackRate;   // UŒ‚‚ÌŠ„‡(ƒXƒs[ƒh)

    private float m_fRateAmount;                   // Š„‡‚Ì‡Œv

    private void Start()
    {
        //m_vOriginRot = Quaternion.Euler(this.transform.rotation);
        GetSetAttack = false;
    }

    /**
     * @fn
     * UŒ‚ˆ—(override)
     * @brief UŒ‚ˆ—
     */
    public override void Attack()
    {
        // ‚±‚±‚Éˆ—‚ð‰Á‚¦‚é
        // ‰ñ“]—Ê‚ðŒvŽZ
        Vector3 vRot =
            new Vector3(vRotAmount.x * fAttackRate, 
                        vRotAmount.y * fAttackRate, 
                        vRotAmount.z * fAttackRate);

        // •Ší‚ð‰ñ“]‚³‚¹‚é
        //this.transform.position = this.transform.position + vRot;
        this.transform.rotation = this.transform.rotation * Quaternion.Euler(vRot);

        // UŒ‚ŽdØ‚Á‚½‚çI—¹‚·‚é
        if (m_fRateAmount >= 1.0f)
        {
            GetSetAttack = false;
            m_fRateAmount = 0.0f;
            this.transform.rotation = Quaternion.Euler(vRotOrigin);
        }
        else m_fRateAmount += fAttackRate;
    }
}
