// ==============================================================
// ProtoPlayer.cs
// Auther:Ihara
// Update:2023/02/20 cs�쐬
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    PlayerWait,
    PlayerMove,
    PlayerJump,
    PlayerDrop,
}

public class ProtoPlayer : MonoBehaviour
{
    [SerializeField] private CharacterController m_CharacterController; //�L�����R���g���[���[(��)
    [SerializeField] private Vector3 m_vMove;                           // �ړ������
    [SerializeField] private Vector3 m_vGravity;                        // �d��
    [SerializeField] private float   m_fJumpPow;                        // ������
    [SerializeField] private PlayerState m_PlayerState;                 // Player�̏�Ԃ��Ǘ�����
    private Vector3 m_vMoveAmount; // ���v�ړ���(�ړ�����d�͂����Z��������)        
    private bool bInputUp; 
    private bool bInputRight;
    private bool bInputLeft;

    // Start is called before the first frame update
    void Start()
    {
        // �����o�̏�����
        m_PlayerState = PlayerState.PlayerDrop;
        bInputUp = false;
        bInputRight = false;
        bInputLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���͊Ǘ�
        if (Input.GetKey(KeyCode.W))
        {
            bInputUp = true;
        }
        else bInputUp = false;

        if (Input.GetKey(KeyCode.D))
        {
            bInputRight = true;
        }
        else bInputRight = false;

        if (Input.GetKey(KeyCode.A))
        {
            bInputLeft = true;
        }
        else bInputLeft = false;
    }

    private void FixedUpdate()
    {
        // �v���C���[�̏�Ԃɂ���čX�V����
        switch (m_PlayerState)
        {
            case PlayerState.PlayerWait:
                UpdateWait();
                break;
            case PlayerState.PlayerMove:
                UpdateMove();
                break;
            case PlayerState.PlayerJump:
                UpdateJump();
                break;
            case PlayerState.PlayerDrop:
                UpdateDrop();
                break;
        }

        // �󒆂ɂ���ꍇ�A�d�͂�^����
        if (!m_CharacterController.isGrounded)
        {
            m_vMoveAmount.x += m_vGravity.x;
            m_vMoveAmount.y += m_vGravity.y;
            m_vMoveAmount.z += m_vGravity.z;
        }

        //Debug.Log(m_CharacterController.isGrounded);

        // ���v�ړ��ʂ����Z
        m_CharacterController.Move(m_vMoveAmount);
    }

    // �҂���Ԃ̍X�V����
    private void UpdateWait()
    {
        // ���v�ړ��ʂ����Z�b�g
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.y = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // ��ԑJ��
        // �u�҂� �� ����v
        if (bInputUp)
        {
            m_PlayerState = PlayerState.PlayerJump;
            m_vMoveAmount.y = m_fJumpPow;
            return;
        }
        // �u�҂� �� �ړ��v
        if (bInputRight || bInputLeft)
        {
            m_PlayerState = PlayerState.PlayerMove;
            return;
        }
    }

    // �ړ���Ԃ̍X�V����
    private void UpdateMove()
    {
        // ���v�ړ��ʂ����Z�b�g
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.y = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // DA�L�[�ňړ�����
        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
        }

        // ��ԑJ��
        // �u�ړ� �� ����v
        //  W�L�[�Œ��􂷂�
        if (bInputUp)
        {
            m_PlayerState = PlayerState.PlayerJump;
            m_vMoveAmount.y = m_fJumpPow;
            return;
        }
        // �u�ړ� �� �҂��v
        if (!bInputRight && !bInputLeft)
        {
            m_PlayerState = PlayerState.PlayerWait;
            return;
        }
    }

    // �����Ԃ̍X�V����
    private void UpdateJump()
    {
        // ���v�ړ��ʂ����Z�b�g
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // DA�L�[�ňړ�����
        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
        }

        // ��ԑJ��
        // �u���� �� �����v
        if (m_vMoveAmount.y <= 0.0f)
        {
            m_PlayerState = PlayerState.PlayerDrop;
        }
    }

    // ������Ԃ̍X�V����
    private void UpdateDrop()
    {
        // ���v�ړ��ʂ����Z�b�g
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.z = 0.0f;

        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
        }

        if(m_CharacterController.isGrounded)
        {
            m_PlayerState = PlayerState.PlayerWait;
        }
    }
}
