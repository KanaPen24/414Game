// ==============================================================
// NK_EnemyControl_Slime.cs
// Auther:Naito
// Update:2023/03/06 cs作成
// ==============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EnemyControl_Slime : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    //敵の攻撃範囲
    [SerializeField] private float m_fAttackRange;
    //フラグ管理
    private bool m_bMoveFlag;       //動いていいか
    private bool m_bAttackFlag;     //攻撃していいか
    private bool m_bPosRight;          //プレイヤーより右にいるか左にいるか。trueだったら右
    //モンスターの動きのやつ
    private NK_EnemyMove_Slime m_Move;
    //攻撃のやつ
    private NK_EnemyAttack_Slime m_Attack;
    //動き出すまでの間隔
    [SerializeField] private int m_nMoveTime;
    //攻撃までの間隔
    [SerializeField] private int m_nAttackTime;
    //プレイヤーの位置情報格納用
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
        //HPがゼロ以下なら消える
        if(m_nHP <= 0)
        {
            Destroy(this.gameObject);
        }
        //プレイヤーに一定距離近づいたら攻撃、それ以外は移動
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

    //スライムがダメージを食らい、後ろにのけぞるやつ。こいつにダメージを与えたいときはこれ使う
    public void SlimeDamage()
    {
        m_nHP -= 1;
        m_Move.KnockBack(m_bPosRight);
    }
}
