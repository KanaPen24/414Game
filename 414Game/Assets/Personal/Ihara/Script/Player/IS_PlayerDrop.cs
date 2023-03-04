/**
 * @file   IS_PlayerDrop.cs
 * @brief  Player�̗����N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerDrop : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player;                          // IS_Player���A�^�b�`����
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Player�̒n�ʔ���
    [SerializeField] private float m_fMovePow;                            // �ړ������

    /**
     * @fn
     * �X�V����
     * @brief  Player�̗����X�V����
     * @detail �p��������override���Ă��܂�
     */
    public override void UpdateStrategy()
    {
        // ������State���Ƃɏ�����������
        Debug.Log("PlayerDrop");

        // ���v�ړ��ʂ����Z�b�g(y�����̓��Z�b�g���Ȃ�)
        m_Player.GetSetMoveAmount =
            new Vector3(0f, m_Player.GetSetMoveAmount.y, 0f);

        // DA�L�[�ňړ�����
        if (m_Player.bInputRight)
        {
            m_Player.m_vMoveAmount.x += m_fMovePow;
        }
        if (m_Player.bInputLeft)
        {
            m_Player.m_vMoveAmount.x -= m_fMovePow;
        }

        // �d�͂����v�ړ��ʂɉ��Z
        m_Player.m_vMoveAmount.y += m_Player.GetSetGravity;

        // =========
        // ��ԑJ��
        // =========
        // �u���� �� �ҋ@�v
        if (m_PlayerGroundColl.IsGroundCollision())
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerWait;
            return;
        }
    }
}
