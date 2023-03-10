// ==============================================================
// NK_EnemyControl_Slime.cs
// Auther:Naito
// Update:2023/03/06 cs�쐬
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EnemyControl_Slime : MonoBehaviour
{
    //�G�̗̑�
    [SerializeField] private int m_nHP;
    //�G�̍U���͈�
    [SerializeField] private float m_fAttackRange;
    //�t���O�Ǘ�
    private bool m_bMoveFlag;       //�����Ă�����
    private bool m_bAttackFlag;     //�U�����Ă�����
    private bool m_bPosRight;          //�v���C���[���E�ɂ��邩���ɂ��邩�Btrue��������E
    //�����X�^�[�̓����̂��
    private NK_EnemyMove_Slime m_Move;
    //�U���̂��
    private NK_EnemyAttack_Slime m_Attack;
    //�����o���܂ł̊Ԋu
    [SerializeField] private int m_nMoveTime;
    //�U���܂ł̊Ԋu
    [SerializeField] private int m_nAttackTime;
    //�v���C���[�̈ʒu���i�[�p
    [SerializeField] private GameObject m_gPlayer;
    // Start is called before the first frame update
    void Start()
    {
        m_Move = GetComponent<NK_EnemyMove_Slime>();
        m_Attack = GetComponent<NK_EnemyAttack_Slime>();
    }

    // Update is called once per frame
    void Update()
    {
        //HP���[���ȉ��Ȃ������
        if(m_nHP <= 0)
        {
            Destroy(this.gameObject);
        }
        //�v���C���[�Ɉ�苗���߂Â�����U���A����ȊO�͈ړ�
        if (m_gPlayer.transform.position.x - m_fAttackRange <= this.transform.position.x && m_gPlayer.transform.position.x + m_fAttackRange >= this.transform.position.x)
        {
            if (m_bAttackFlag == false)
            {
                StartCoroutine(Attack());
            }
        }
        else
        {
            if (m_gPlayer.transform.position.x > this.transform.position.x)
            {
                m_bPosRight = true;
            }
            else
            {
                m_bPosRight = false;
            }
            if (m_bMoveFlag == false)
            {
                StartCoroutine(Move());
            }
        }
    }

    private IEnumerator Move()
    {
        m_bMoveFlag = true;
        m_Move.MoveFlagChanger(m_bPosRight);
        yield return new WaitForSeconds(m_nMoveTime);
        m_bMoveFlag = false;
    }

    private IEnumerator Attack()
    {
        m_bAttackFlag = true;
        yield return new WaitForSeconds(m_nAttackTime);
        m_Attack.CreateAttack(m_bPosRight);
        m_bAttackFlag = false;
    }

    //�X���C�����_���[�W��H�炢�A���ɂ̂������B�����Ƀ_���[�W��^�������Ƃ��͂���g��
    public void SlimeDamage()
    {
        m_nHP -= 1;
        m_Move.KnockBack(m_bPosRight);
    }
}
