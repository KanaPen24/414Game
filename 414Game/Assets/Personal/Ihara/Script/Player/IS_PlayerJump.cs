/**
 * @file   IS_PlayerJump.cs
 * @brief  Player�̒���N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 * @Update 2023/03/12 �A�j���[�V���������ǉ�
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerJump : IS_PlayerStrategy
{
    [SerializeField] IS_Player m_Player;      // IS_Player���A�^�b�`����
    [SerializeField] private float m_fJumpPow;// ������
    [SerializeField] private float m_fMovePow;// �ړ������
    /**
     * @fn
     * �X�V����
     * @brief  Player�̒���X�V����
     * @detail �p��������override���Ă��܂�
     */
    public override void UpdateStrategy()
    {
        // ������State���Ƃɏ�����������
        //Debug.Log("PlayerJump");

        // ����J�n���ɒ����͂����v�ړ��ʂɉ��Z
        if(m_Player.GetSetJumpFlg)
        {
            m_Player.GetSetMoveAmount = new Vector3(0f, m_fJumpPow, 0f);
            m_Player.GetSetJumpFlg = false;
        }

        // ���v�ړ��ʂ����Z�b�g(y�����̓��Z�b�g���Ȃ�)
        m_Player.GetSetMoveAmount = 
            new Vector3(0f, m_Player.GetSetMoveAmount.y, 0f);

        // DA�L�[�ňړ�����
        if (m_Player.bInputRight)
        {
            m_Player.m_vMoveAmount.x += m_fMovePow;
            m_Player.GetSetPlayerDir = PlayerDir.Right;
        }
        if (m_Player.bInputLeft)
        {
            m_Player.m_vMoveAmount.x -= m_fMovePow;
            m_Player.GetSetPlayerDir = PlayerDir.Left;
        }

        // �d�͂����v�ړ��ʂɉ��Z
        m_Player.m_vMoveAmount.y += m_Player.GetSetGravity;

        // =========
        // ��ԑJ��
        // =========
        // �u���� �� �����v
        if (m_Player.GetSetMoveAmount.y <= 0.0f)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
            m_Player.GetAnimator().SetBool("isDrop", true);
            m_Player.GetAnimator().SetBool("isJump", false);
            return;
        }
    }
}
