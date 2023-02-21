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

// ================================
// PlayerDir
// �c Player�̕������Ǘ�����񋓑�
// ================================
enum PlayerDir
{
    Right,
    Left,
}

public class ProtoPlayer : MonoBehaviour
{
    [SerializeField] private PlayerState m_PlayerState;                 // Player�̏�Ԃ��Ǘ�����
    [SerializeField] private PlayerDir m_PlayerDir;                     // Player�̕������Ǘ�����
    [SerializeField] private Rigidbody m_Rigidbody;                     // Player��RigidBody
    [SerializeField] private GameObject[] m_RayPoints;                  // Ray���΂��n�_(4��)
    [SerializeField] private Vector3 m_vMove;                           // �ړ������
    [SerializeField] private Vector3 m_vGravity;                        // �d��
    [SerializeField] private float   m_fJumpPow;                        // ������
    [SerializeField] private float   m_fRayLength;                      // Ray�̒���

    private Vector3 m_vMoveAmount; // ���v�ړ���(�ړ�����d�͂����Z�������̂�velocity�ɑ������)        
    private bool bInputUp; 
    private bool bInputRight;
    private bool bInputLeft;

    // Start is called before the first frame update
    void Start()
    {
        // �����o�̏�����
        m_PlayerDir   = PlayerDir.Right;
        m_vMoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
        bInputUp      = false;
        bInputRight   = false;
        bInputLeft    = false;
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
        // Player�̏�Ԃɂ���čX�V����
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

        //Debug.Log(m_RayPoint.transform.position);
    }

    // ================================= 
    // �҂���Ԃ̍X�V����
    // =================================
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

    // ================================= 
    // �ړ���Ԃ̍X�V����
    // =================================
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
            m_PlayerDir = PlayerDir.Right;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
            m_PlayerDir = PlayerDir.Left;
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

    // ================================= 
    // �����Ԃ̍X�V����
    // =================================
    private void UpdateJump()
    {
        // ���v�ړ��ʂ����Z�b�g(y�����̓��Z�b�g���Ȃ�)
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // DA�L�[�ňړ�����
        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
            m_PlayerDir = PlayerDir.Right;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
            m_PlayerDir = PlayerDir.Left;
        }

        // ��ԑJ��
        // �u���� �� �����v
        if (m_vMoveAmount.y <= 0.0f)
        {
            m_PlayerState = PlayerState.PlayerDrop;
        }
    }

    // ============================================
    // ������Ԃ̍X�V����
    // ����ԑJ�ڂ͑��̊֐��ōs��
    // ============================================
    private void UpdateDrop()
    {
        // ���v�ړ��ʂ����Z�b�g(y�����̓��Z�b�g���Ȃ�)
        m_vMoveAmount.x = 0.0f;
        m_vMoveAmount.z = 0.0f;

        // DA�L�[�ňړ�����
        if (bInputRight)
        {
            m_vMoveAmount.x += m_vMove.x;
            m_PlayerDir = PlayerDir.Right;
        }
        if (bInputLeft)
        {
            m_vMoveAmount.x -= m_vMove.x;
            m_PlayerDir = PlayerDir.Left;
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
        // =========================================== 
        // �ϐ��錾 
        // ===========================================

        // Ray�̏�����
        Ray[] ray = new Ray[m_RayPoints.Length];
        //Ray�����������I�u�W�F�N�g�̏������锠
        RaycastHit hit;

        // ===========================================

        // Ray�̐�������������
        for (int i = 0; i < m_RayPoints.Length;++i)
        {
            //Ray�̍쐬�@�@�@�@�@�@�@��Ray���΂����_�@�@�@��Ray���΂�����
            ray[i] = new Ray(m_RayPoints[i].transform.position, Vector3.down);
            Debug.DrawRay(m_RayPoints[i].transform.position, Vector3.down, Color.red, m_fRayLength);
        }

        // �n�ʂƂ̓����蔻��(���ł��ʂ��true)
        for (int i = 0; i < m_RayPoints.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, m_fRayLength))
            {
                //if (m_PlayerState != PlayerState.PlayerJump)
                //{
                //    // ���W�̏C��
                //    this.transform.position = this.transform.position + 
                //        new Vector3(0.0f,
                //                    hit.collider.transform.position.y / 2,
                //                    0.0f);
                //}
                return true;
            }
        }
        return false;
    }
}

