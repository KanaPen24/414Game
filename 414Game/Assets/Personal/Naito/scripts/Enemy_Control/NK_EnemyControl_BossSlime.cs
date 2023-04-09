/*
 * @Update 2023/04/06 紙吹雪エフェクト実装
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_EnemyControl_BossSlime : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    [SerializeField] private int m_nMaxHP;
    [SerializeField] private IS_Player m_Player;
    [SerializeField] private ParticleSystem confettiEffect; // 紙吹雪エフェクト(倒されたとき発生)
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
        if (!m_bMoveFlag && !m_bKnockBack && m_bStandFlag)
        {
            StartCoroutine(Move());
        }
        if(m_fViewX <= 0 && !m_bKnockBack)
        {
            m_Move.KnockBack(true);
            m_bKnockBack = true;
            Invoke("KnockBackFlagChanger", 1.0f);
            RightFlagChanger();
        }
        if (m_fViewX >= 1 && !m_bKnockBack)
        {
            m_Move.KnockBack(false);
            m_bKnockBack = true;
            Invoke("KnockBackFlagChanger", 1.0f);
            RightFlagChanger();
        }

        if(m_nHP <= 0)
        {
            Destroy(this.gameObject);
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

        // プレイヤーだったら
        if (collision.gameObject == m_Player.gameObject)
        {
            Debug.Log("Player Damage!!");
            m_Player.GetPlayerHp().DelLife(10);
        }

        // 武器だったら
        if (collision.gameObject.tag == "Weapon")
        {
            Debug.Log("Enemy Damage!!");
            //m_HpBarHP.DelLife(10);
            m_nHP -= 5;
        }

        // HPが0になったら、紙吹雪エフェクト発生
        if(m_nHP <= 0)
        {
            ParticleSystem effect = Instantiate(confettiEffect);
            effect.Play();
            effect.transform.position =  
                new Vector3(m_Player.transform.position.x,0f,m_Player.transform.position.z);
            effect.transform.localScale = new Vector3(10f, 10f, 10f);
            Destroy(effect.gameObject, 5.0f); // 5秒後に消える
        }
    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    // 武器だったら
    //    if (collision.gameObject.tag == "Weapon")
    //    {
    //        if (m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack)
    //        {
    //            Debug.Log("Enemy Damage!!");
    //            m_HpBarHP.DelLife(10);
    //            m_nHP--;

    //            // Hpバーが当たっていた時、ドレイン処理を行う
    //            if (m_Player.GetSetEquipWeaponState == EquipWeaponState.PlayerHpBar)
    //            {
    //                m_Player.GetPlayerHp().AddLife(5);
    //            }
    //        }
    //    }
    //}


    public int GetSetHp
    {
        get { return m_nHP; }
        set { m_nHP = value; }
    }
    public int GetSetMaxHp
    {
        get { return m_nMaxHP; }
        set { m_nMaxHP = value; }
    }

}
