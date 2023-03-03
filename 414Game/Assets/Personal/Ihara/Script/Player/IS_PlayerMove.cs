/**
 * @file   IS_PlayerMove.cs
 * @brief  Player�̈ړ��N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerMove : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player;                          // IS_Player���A�^�b�`����
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Player�̒n�ʔ���
    [SerializeField] private float m_fMovePow;                            // �ړ������
    /**
     * @fn
     * �X�V����
     * @brief  Player�̈ړ��X�V����
     * @detail �p��������override���Ă��܂�
     */
    public override void UpdateStrategy()
    {
        // ������State���Ƃɏ�����������
        Debug.Log("PlayerMove");

        // ���v�ړ��ʂ����Z�b�g
        m_Player.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

        // DA�L�[�ňړ�����
        if (m_Player.bInputRight)
        {
            m_Player.m_vMoveAmount.x += m_fMovePow;
        }
        if (m_Player.bInputLeft)
        {
            m_Player.m_vMoveAmount.x -= m_fMovePow;
        }

        // =========
        // ��ԑJ��
        // =========
        //�u�ړ� �� �����v
        if (!m_PlayerGroundColl)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
            return;
        }
        // �u�ړ� �� ����v
        //  W�L�[�Œ��􂷂�
        if (m_Player.bInputUp)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerJump;
            m_Player.GetSetJumpFlg = true;
            return;
        }
        // �u�ړ� �� �ҋ@�v
        if (!m_Player.bInputRight && !m_Player.bInputLeft)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerWait;
            return;
        }
    }
}
