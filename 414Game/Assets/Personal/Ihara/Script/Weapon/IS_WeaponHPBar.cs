/**
 * @file   IS_WeaponHPBar.cs
 * @brief  HPBar�̕���N���X
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_WeaponHPBar : IS_Weapon
{
    [SerializeField] private Vector3 vRotAmount;  // �U���̉�]��
    [SerializeField] private Vector3 vRotOrigin;  // �U���̏����p�x
    [SerializeField] private float fAttackRate;   // �U���̊���(�X�s�[�h)

    private float m_fRateAmount;                   // �����̍��v

    private void Start()
    {
        //m_vOriginRot = Quaternion.Euler(this.transform.rotation);
        GetSetAttack = false;
    }

    /**
     * @fn
     * �U������(override)
     * @brief �U������
     */
    public override void Attack()
    {
        // �����ɏ�����������
        // ��]�ʂ��v�Z
        Vector3 vRot =
            new Vector3(vRotAmount.x * fAttackRate, 
                        vRotAmount.y * fAttackRate, 
                        vRotAmount.z * fAttackRate);

        // �������]������
        //this.transform.position = this.transform.position + vRot;
        this.transform.rotation = this.transform.rotation * Quaternion.Euler(vRot);

        // �U���d�؂�����I������
        if (m_fRateAmount >= 1.0f)
        {
            GetSetAttack = false;
            m_fRateAmount = 0.0f;
            this.transform.rotation = Quaternion.Euler(vRotOrigin);
        }
        else m_fRateAmount += fAttackRate;
    }
}
