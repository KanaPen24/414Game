// ==============================================================
// ProtoPlayer.cs
// Auther:Ihara
// Update:2023/02/20 cs�쐬
// Update:2023/02/21 �ړ��@���uCharacterController�v����
//                   �uRigidBody�v�ɕύX
// Update:2023/02/21 �n�ʔ���֐����쐬(��)
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ================================
// PlayerState
// �c Player�̏�Ԃ��Ǘ�����񋓑�
// ================================
enum PlayerState
{
    PlayerWait, // �҂����
    PlayerMove, // �ړ����
    PlayerJump, // ������
    PlayerDrop, // �������
}

public class ProtoPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody;                     // Player��RigidBody
    [SerializeField] private GameObject m_RayPoint;                     // Ray���΂��n�_
    [SerializeField] private Vector3 m_vMove;                           // �ړ������
    [SerializeField] private Vector3 m_vGravity;                        // �d��
    [SerializeField] private float   m_fJumpPow;                        // ������
    [SerializeField] private PlayerState m_PlayerState;                 // Player�̏�Ԃ��Ǘ�����
    private Vector3 m_vMoveAmount; // ���v�ړ���(�ړ�����d�͂����Z�������̂�velocity�ɑ������)        
    private bool bInputUp; 
    private bool bInputRight;
    private bool bInputLeft;

    // Start is called before the first frame update
    void Start()
    {
        // �����o�̏�����
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
        if (!IsGroundCollision())
        {
            // �d�͂����v�ړ��ʂɉ��Z
            m_vMoveAmount += m_vGravity;

            if(m_PlayerState != PlayerState.PlayerJump)
            {
                m_PlayerState = PlayerState.PlayerDrop;
            }
        }
        else
        {
            if (m_PlayerState == PlayerState.PlayerDrop)
            {
                m_PlayerState = PlayerState.PlayerWait;
            }
        }

        // ���v�ړ��ʂ�velocity�ɉ��Z
        m_Rigidbody.velocity = m_vMoveAmount;
    }

    // �҂���Ԃ̍X�V����
    private void UpdateWait()
    {
        // ���v�ړ��ʂ����Z�b�g
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.y = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // =========
        // ��ԑJ��
        // =========
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

        // =========
        // ��ԑJ��
        // =========
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
        // ���v�ړ��ʂ����Z�b�g(y�����̓��Z�b�g���Ȃ�)
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
        // ���v�ړ��ʂ����Z�b�g(y�����̓��Z�b�g���Ȃ�)
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
    }

    // ===========================================================
    // �n�ʔ���֐�
    // �߂�l: bool�^
    // �c �n�ʂ�ray���������Ă�����true��Ԃ�,
    //    �������Ă��Ȃ����false��Ԃ�
    // ===========================================================
    private bool IsGroundCollision()
    {
        //Ray�̍쐬�@�@�@�@�@�@�@��Ray���΂����_�@�@�@��Ray���΂�����
        Ray ray = new Ray(m_RayPoint.transform.position, new Vector3(0, -1, 0));

        //Ray�����������I�u�W�F�N�g�̏������锠
        RaycastHit hit;

        //Ray�̔�΂��鋗��
        float distance = 1.0f;

        //Ray�̉���    ��Ray�̌��_�@�@�@�@��Ray�̕����@�@�@�@�@�@�@�@�@��Ray�̐F
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

        //����Ray�ɃI�u�W�F�N�g���Փ˂�����
        //                  ��Ray  ��Ray�����������I�u�W�F�N�g ������
        if (Physics.Raycast(ray, out hit, distance))
        {
            return true;
        }
        else return false;
    }
}

