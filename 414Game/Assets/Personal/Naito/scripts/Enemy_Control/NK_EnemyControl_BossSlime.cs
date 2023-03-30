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
    private bool m_bPosRight;       //右に動くか左に動くか、trueなら左
    private bool m_bKnockBack;      //ノックバックしているかどうか
    private bool m_bStandFlag;      //地面に立っているかどうか
    //モンスターの動きのやつ
    private NK_EnemyMove_Slime m_Move;
    //攻撃のやつ
    //private NK_EnemyAttack_Slime m_Attack;
    //動き出すまでの間隔
    [SerializeField] private int m_nMoveTime;
    //ビュー視点で画面のどこにいるか
    private float m_fViewX;
    //子供すらいむを召喚させる確率
    //[SerializeField] private int m_nKidRnd;
    //子供すらいむ出すやつ
    //[SerializeField] private GameObject m_gKidSlimeSpooner;
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
        //Debug.Log(m_bStandFlag);
        //Debug.Log(m_bMoveFlag);
        m_fViewX = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (m_bMoveFlag == false && m_bKnockBack == false && m_bStandFlag == true)
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
        //確率で動くか召喚か
        //int rnd = Random.Range(1, m_nKidRnd);
        //if (rnd != 1)
        //{
            m_Move.MoveFlagChanger(m_bPosRight);
            m_bStandFlag = false;
        //}else
        //{
            //Instantiate(m_gKidSlimeSpooner, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        //}
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Floor")
        {
            m_bStandFlag = true;
        }
    }
}
