/**
 * @file   IS_Player.cs
 * @brief  Player�̃N���X
 * @author IharaShota
 * @date   2023/03/03
 * @Update 2023/03/03 �쐬
 * @Update 2023/03/10 �U����p�̕ϐ��ǉ�
 * @Update 2023/03/12 Animator�ǉ�
 * @Update 2023/03/12 ������ǉ�
 * @Update 2023/03/12 �����ǉ�
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
    PlayerWait,   // �҂����
    PlayerWalk,   // �ړ����
    PlayerJump,   // ������
    PlayerDrop,   // �������
    PlayerAttack, // �U�����

    MaxPlayerState
}

// ===============================================
// PlayerDir
// �c Player�̌������Ǘ�����񋓑�
// ===============================================
public enum PlayerDir
{
    Left, // ������
    Right,// �E����

    MaxDir
}

// ================================================
// PlayerWeapon
// �c Player�̕�����Ǘ�����񋓑�
// ��m_PlayerWeapon�͂��̏��ԂɂȂ�悤�ɓ���邱��
// ================================================
public enum PlayerWeaponState
{
    PlayerHpBar,

    MaxPlayerWeaponState
}

public class IS_Player : MonoBehaviour
{
    [SerializeField] private GameObject              m_PlayerObj;        // Player�̃��f��
    [SerializeField] private Animator                m_animator;         // Player�̃A�j���[�V����
    [SerializeField] private Rigidbody               m_Rigidbody;        // Player��RigidBody
    [SerializeField] private YK_HPBarVisible         m_HpVisible;        // Player��Hp�\���Ǘ�
    [SerializeField] private YK_PlayerHP             m_Hp;               // Player��Hp
    [SerializeField] private List<IS_PlayerStrategy> m_PlayerStrategys;  // Player�����N���X�̓��I�z��
    [SerializeField] private List<IS_Weapon>         m_PlayerWeapons;          // ����N���X�̓��I�z��
    [SerializeField] private PlayerState             m_PlayerState;      // Player�̏�Ԃ��Ǘ�����
    [SerializeField] private PlayerDir               m_PlayerDir;        // Player�̌������Ǘ�����
    [SerializeField] private PlayerWeaponState       m_PlayerWeaponState;// Player�̕����Ԃ��Ǘ�����
    [SerializeField] private float                   m_fGravity;         // �d��

    public Vector3 m_vMoveAmount; // ���v�ړ���(�ړ�����d�͂����Z�������̂�velocity�ɑ������)
    public bool bInputUp;
    public bool bInputRight;
    public bool bInputLeft;
    public bool bInputSpace;

    public int nWeaponState;     // �����Ԃ�int�^�Ŋi�[����
    private bool m_bJumpFlg;      // ����J�n�t���O
    private bool m_bAttackFlg;    // �U���J�n�t���O

    private void Start()
    {
        // �����N���X�Ɨ񋓌^�̐����Ⴆ�΃��O�o��
        if(m_PlayerStrategys.Count != (int)PlayerState.MaxPlayerState)
        {
            Debug.Log("m_PlayerStarategy�̗v�f����m_PlayerState�̐��������ł͂���܂���");
        }

        // ����N���X�Ɨ񋓌^�̐����Ⴆ�΃��O�o��
        if (m_PlayerWeapons.Count != (int)PlayerWeaponState.MaxPlayerWeaponState)
        {
            Debug.Log("m_PlayerWeapons�̗v�f����m_PlayerWeaponState�̐��������ł͂���܂���");
        }

        // �����o�̏�����
        m_vMoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
        m_bJumpFlg    = false;
        bInputUp      = false;
        bInputRight   = false;
        bInputLeft    = false;
        bInputSpace   = false;
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

        if (Input.GetKey(KeyCode.Space))
        {
            bInputSpace = true;
        }
        else bInputSpace = false;

        if (Input.GetKey(KeyCode.Z))
        {
            m_HpVisible.GetSetVisible = false;
        }
        if (Input.GetKey(KeyCode.X))
        {
            m_HpVisible.GetSetVisible = true;
        }
    }

    private void FixedUpdate()
    {
        // ���݂�Player�̏�Ԃ�int�^�Ɋi�[
        int nPlayerState = (int)GetSetPlayerState;

        // ���݂�Player�̕����Ԃ�int�^�Ɋi�[
        nWeaponState = (int)GetSetPlayerWeaponState;

        // Player�̏�Ԃɂ���čX�V����
        m_PlayerStrategys[nPlayerState].UpdateStrategy();

        // ���v�ړ��ʂ�velocity�ɉ��Z
        m_Rigidbody.velocity = m_vMoveAmount;

        // �����ɂ���ă��f���̊p�x�ύX
        // �E����
        if(GetSetPlayerDir == PlayerDir.Right)
        {
            m_PlayerObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 90.0f, 0f));
        }
        // ������
        else if (GetSetPlayerDir == PlayerDir.Left)
        {
            m_PlayerObj.transform.rotation = Quaternion.Euler(new Vector3(0f, -90.0f, 0f));
        }
    }

    /**
     * @fn
     * Player��Animator��getter
     * @return m_Animator(Animator)
     * @brief Player��Animator��Ԃ�
     */
    public Animator GetAnimator()
    {
        return m_animator;
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
     * Player��Hp�\����getter
     * @return m_HpVisible(YK_HPBarVisible)
     * @brief Player��YK_HPBarVisible��Ԃ�
     */
    public YK_HPBarVisible GetHPVisible()
    {
        return m_HpVisible;
    }

    /**
     * @fn
     * Player��Hp�Ǘ���getter
     * @return m_Hp(YK_PlayerHP)
     * @brief Player��YK_PlayerHP��Ԃ�
     */
    public YK_PlayerHP GetPlayerHp()
    {
        return m_Hp;
    }

    /**
     * @fn
     * ����N���X��getter
     * @return m_Weapons[i]
     * @brief �����Ԃ�
     */
    public IS_Weapon GetWeapons(int i)
    {
        return m_PlayerWeapons[i];
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

    /**
     * @fn
     * Player�̌�����getter�Esetter
     * @return m_PlayerDir
     * @brief Player�̌�����Ԃ��E�Z�b�g
     */
    public PlayerDir GetSetPlayerDir
    {
        get { return m_PlayerDir; }
        set { m_PlayerDir = value; }
    }

    /**
     * @fn
     * Player�̕����Ԃ�getter�Esetter
     * @return m_PlayerWeaponState
     * @brief Player�̕����Ԃ�Ԃ��E�Z�b�g
     */
    public PlayerWeaponState GetSetPlayerWeaponState
    {
        get { return m_PlayerWeaponState; }
        set { m_PlayerWeaponState = value; }
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
     * �U���J�n�t���O��getter�Esetter
     * @return m_bAttackFlg(bool)
     * @brief �U���J�n�t���O��Ԃ��E�Z�b�g
     */
    public bool GetSetAttackFlg
    {
        get { return m_bAttackFlg; }
        set { m_bAttackFlg = value; }
    }
}
