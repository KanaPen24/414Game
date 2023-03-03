/**
 * @file   IS_PlayerWait.cs
 * @brief  Player�̑ҋ@�N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_PlayerWait : IS_PlayerStrategy
{
    [SerializeField] private IS_Player m_Player; // IS_Player���A�^�b�`����
    [SerializeField] private IS_PlayerGroundCollision m_PlayerGroundColl; // Player�̒n�ʔ���
    /**
     * @fn
     * �X�V����
     * @brief  Player�̑ҋ@�X�V����
     * @detail �p��������override���Ă��܂�
     */
    public override void UpdateStrategy()
    {
        // ������State���Ƃɏ�����������
        Debug.Log("PlayerWait");

        // ���v�ړ��ʂ����Z�b�g
        m_Player.GetSetMoveAmount = new Vector3(0f, 0f, 0f);

        // =========
        // ��ԑJ��
        // =========
        //�u�ҋ@ �� �����v
        if(!m_PlayerGroundColl)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerDrop;
            return;
        }
        //�u�ҋ@ �� ����v
        if (m_Player.bInputUp)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerJump;
            m_Player.GetSetJumpFlg = true;
            return;
        }
        // �u�ҋ@ �� �ړ��v
        if (m_Player.bInputRight || m_Player.bInputLeft)
        {
            m_Player.GetSetPlayerState = PlayerState.PlayerMove;
            return;
        }
    }
}
