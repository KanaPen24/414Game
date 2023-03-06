/**
 * @file   IS_Player.cs
 * @brief  Player�̃N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// PlayerState
// �c Player�̏�Ԃ��Ǘ�����񋓑�
// ��m_PlayerState�͂��̏��ԂɂȂ�悤�ɓ���邱��
// ===============================================
public enum PlayerState
{
    PlayerWait, // �҂����
    PlayerMove, // �ړ����
    PlayerJump, // ������
    PlayerDrop, // �������

    MaxPlayerState
}

public class IS_Player : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody;                       // Player��RigidBody
    [SerializeField] private List<IS_PlayerStrategy> m_PlayerStrategys;   // Player�����N���X�̓��I�z��
    [SerializeField] private PlayerState m_PlayerState;                   // Player�̏�Ԃ��Ǘ�����

    public float m_fGravity;      // �d��
    public Vector3 m_vMoveAmount; // ���v�ړ���(�ړ�����d�͂����Z�������̂�velocity�ɑ������)
    public bool bInputUp;
    public bool bInputRight;
    public bool bInputLeft;

    private bool m_bJumpFlg;      // ����J�n�t���O

    private void Start()
    {
        // �����N���X�Ɨ񋓌^�̐����Ⴆ�΃��O�o��
        if(m_PlayerStrategys.Count != (int)PlayerState.MaxPlayerState)
        {
            Debug.Log("m_PlayerStarategy�̗v�f����m_PlayerState�̐��������ł͂���܂���");
        }
        
        // �����o�̏�����
        m_vMoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
        m_bJumpFlg = false;
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
        // ���݂�Player�̏�Ԃ�int�^�Ɋi�[
        int nState = (int)GetSetPlayerState;

        // Player�̏�Ԃɂ���čX�V����
        m_PlayerStrategys[nState].UpdateStrategy();

        // ���v�ړ��ʂ�velocity�ɉ��Z
        m_Rigidbody.velocity = m_vMoveAmount;
    }

    /**
     * @fn
     * Player��Rigidbody��getter
     * @return m_Rigidbody(Rigidbody)
     * @brief Player��RIgidbody��Ԃ�
     */
    public Rigidbody GetRigidbody()
    {
        return m_Rigidbody;
    }

    /**
     * @fn
     * �d�͂�getter�Esetter
     * @return m_fGravity(float)
     * @brief �d�͂�Ԃ��E�Z�b�g
     */
    public float GetSetGravity
    {
        get { return m_fGravity; }
        set { m_fGravity = value; }
    }

    /**
     * @fn
     * ���v�ړ��ʂ�getter�Esetter
     * @return m_vAmount(Vector3)
     * @brief ���v�ړ��ʂ�Ԃ��E�Z�b�g
     */
    public Vector3 GetSetMoveAmount
    {
        get { return m_vMoveAmount; }
        set { m_vMoveAmount = value; }
    }

    /**
     * @fn
     * ����J�n�t���O��getter�Esetter
     * @return m_bJumpFlg(bool)
     * @brief ����J�n�t���O��Ԃ��E�Z�b�g
     */
    public bool GetSetJumpFlg
    {
        get { return m_bJumpFlg; }
        set { m_bJumpFlg = value; }
    }

    /**
     * @fn
     * Player�̏�Ԃ�getter�Esetter
     * @return m_PlayerState
     * @brief Player�̏�Ԃ�Ԃ��E�Z�b�g
     */
    public PlayerState GetSetPlayerState
    {
        get { return m_PlayerState; }
        set { m_PlayerState = value; }
    }
}