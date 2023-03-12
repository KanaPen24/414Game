/**
 * @file   IS_Weapon.cs
 * @brief  ����̃N���X
 * @author IharaShota
 * @date   2023/03/12
 * @Update 2023/03/12 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Weapon : MonoBehaviour
{
    private bool m_bAttack; // �U�������ǂ���

    /**
     * @fn
     * �U������(override�O��)
     * @brief �U������
     */
    public virtual void Attack()
    {
        // �����ɏ�����������
        Debug.Log("�F����");
    }


    /**
     * @fn
     * �U������getter�Esetter
     * @return m_bAttack(bool)
     * @brief �U������Ԃ��E�Z�b�g
     */
    public bool GetSetAttack
    {
        get { return m_bAttack; }
        set { m_bAttack = value; }
    }
}
