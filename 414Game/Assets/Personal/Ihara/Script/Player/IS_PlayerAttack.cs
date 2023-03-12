/**
 * @file   IS_PlayerAttack.cs
 * @brief  Player�̍U���N���X
 * @author IharaShota
 * @date   2023/03/10
 * @Update 2023/03/10 �쐬
 * @Update 2023/03/12 �A�j���[�V���������ǉ�
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerAttack : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player; // IS_Player���A�^�b�`����
    private int i = 0;

    /**
     * @fn
     * �X�V����
     * @brief  Player�̍U���X�V����
     * @detail �p��������override���Ă��܂�
     */
    public override void UpdateStrategy()
    {
        // ������State���Ƃɏ�����������
        //Debug.Log("PlayerAttack");

        // �U���J�n���̏���
        if (m_Player.GetSetAttackFlg)
        {
            m_Player.GetSetAttackFlg = false;
            m_Player.GetWeapons((int)m_Player.GetSetPlayerWeaponState).GetSetAttack = true;
        }

        // ���v�ړ��ʂ����Z�b�g
        m_Player.GetSetMoveAmount =
            new Vector3(0f, 0f, 0f);

        // �w�肵������ōU������
        m_Player.GetWeapons((int)m_Player.GetSetPlayerWeaponState).Attack();

        // =========
        // ��ԑJ��
        // =========
        // �u�U�� �� �ҋ@�v
        if (!m_Player.GetWeapons(m_Player.nWeaponState).GetSetAttack)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerWait;
            m_Player.GetAnimator().SetBool("isWait", true);
            m_Player.GetAnimator().SetBool("isAttack", false);
            i = 0;
            return;
        }
    }

    /**
     * @fn
     * Player�̍U������
     * @brief  ���@�͂܂��l����
     * @detail ���쒆
     */
    private void Attack()
    {

    }
}
