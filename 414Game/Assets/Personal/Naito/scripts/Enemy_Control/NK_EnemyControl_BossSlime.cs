using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EnemyControl_BossSlime : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    //敵の攻撃範囲
    //[SerializeField] private float m_fAttackRange;
    //フラグ管理
    private bool m_bMoveFlag;       //動いていいか
    private bool m_bAttackFlag;     //攻撃していいか
    private bool m_bPosRight;          //右に動くか左に動くか、trueなら左
    private bool m_bKnockBack;
    //モンスターの動きのやつ
    private NK_EnemyMove_Slime m_Move;
    //攻撃のやつ
    //private NK_EnemyAttack_Slime m_Attack;
    //動き出すまでの間隔
    [SerializeField] private int m_nMoveTime;
    //ビュー視点で画面のどこにいるか
    private float m_fViewX;
    //攻撃までの間隔
    //[SerializeField] private int m_nAttackTime;
    //プレイヤーの位置情報格納用
    //[SerializeField] private GameObject m_gPlayer;
    // Start is called before the first frame update
    void Start()
    {
        m_Move = GetComponent<NK_EnemyMove_Slime>();
    }

    // Update is called once per frame
    void Update()
    {
        m_fViewX = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (m_bMoveFlag == false && m_bKnockBack == false)
        {
            StartCoroutine(Move());
        }
        if(m_fViewX <= 0 && m_bKnockBack==false)
        {
            m_Move.KnockBack(true);
            m_bKnockBack = true;
            Invoke("KnockBackFlagChanger", 1.0f);
            RightFlagChanger();
        }
        if (m_fViewX >= 1 && m_bKnockBack == false)
        {
            m_Move.KnockBack(false);
            m_bKnockBack = true;
            Invoke("KnockBackFlagChanger", 1.0f);
            RightFlagChanger();
        }
    }

    private IEnumerator Move()
    {
        m_bMoveFlag = true;
        m_Move.MoveFlagChanger(m_bPosRight);
        yield return new WaitForSeconds(m_nMoveTime);
        m_bMoveFlag = false;
    }

    private void KnockBackFlagChanger()
    {
        m_bKnockBack = !m_bKnockBack;
    }
    private void RightFlagChanger()
    {
        m_bPosRight = !m_bPosRight;
    }
}
