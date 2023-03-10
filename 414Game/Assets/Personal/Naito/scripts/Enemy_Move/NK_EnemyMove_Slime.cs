// ==============================================================
// NK_EnemyMove_Slime.cs
// Auther:Naito
// Update:2023/03/06 cs�쐬
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EnemyMove_Slime : MonoBehaviour
{
    //�t���O�Ǘ�
    //private bool m_bMoveFlag;           //�ړ��t���O
    //private bool m_bPosRight;           //�v���C���[���E�ɂ��邩���ɂ��邩�Btrue��������E
    //���W�b�g�{�f�B
    private Rigidbody m_rRdoby;
    //���ړ�
    [SerializeField] private float m_fMovePower;
    //�W�����v��
    [SerializeField] private float m_fJumpPower;

    private void Start()
    {
        m_rRdoby = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    public void MoveFlagChanger(bool PosRight)
    {
        if (PosRight)
        {
            //�΂ߔ��
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            //�΂ߔ��
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
    }

    public void KnockBack(bool PosRight)
    {
        if (PosRight)
        {
            //�m�b�N�o�b�N
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            //�m�b�N�o�b�N
            this.m_rRdoby.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            this.m_rRdoby.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
    }
}


