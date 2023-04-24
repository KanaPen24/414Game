/**
 * @file   NK_Bat.cs
 * @brief  Batのクラス
 * @author NaitoKoki
 * @date   2023/04/17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// BossBatState
// … BossBatの状態を管理する列挙体
// ※m_PlayerStateはこの順番になるように入れること
// ===============================================
public enum BatState
{
    //BatWait,     //待機状態
    BatMove,     //移動状態
    BatSonic,    //超音波攻撃状態
    BatFall,     //急降下攻撃

    MaxBatState
}

// ===============================================
// BossBatDir
// … BossBatの向きを管理する列挙体
// ===============================================
public enum BatDir
{
    Left, // 左向き
    Right,// 右向き

    MaxDir
}

public class NK_Bat : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    [SerializeField] private int m_nMaxHP;//敵の最大体力
    [SerializeField] private IS_Player m_Player;//プレイヤー
    //[SerializeField] private IS_GoalEffect goalEffect;//倒されたときに発生するエフェクト
    [SerializeField] private List<NK_BatStrategy> m_BatStrategy; // BossBat挙動クラスの動的配列
    [SerializeField] private BatState m_BatState;      // BossBatの状態を管理する
    [SerializeField] private BatDir m_BatDir;        // BossBatの向きを管理する
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private Rigidbody m_Rbody;
    public Vector3 m_MoveValue;

    private void Start()
    {
        m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_Rbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GetSetBatState == BatState.BatMove)
        {
            if (m_Player.transform.position.x > this.gameObject.transform.position.x)
            {
                GetSetBatDir = BatDir.Right;
            }
            else
            {
                GetSetBatDir = BatDir.Left;
            }
        }
    }

    private void FixedUpdate()
    {
        m_BatStrategy[(int)m_BatState].UpdateStrategy();

        m_Rbody.velocity = m_MoveValue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーだったら
        if (collision.gameObject == m_Player.gameObject)
        {
            Debug.Log("Player Damage!!");
            //m_Player.GetPlayerHp().DelLife(10);
        }

        // 武器だったら
        if (collision.gameObject.tag == "Weapon")
        {
            Debug.Log("Enemy Damage!!");
            //m_HpBarHP.DelLife(10);
            m_nHP -= 5;
        }

        // HPが0になったら、このオブジェクトを破壊
        if (m_nHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    /**
 * @fn
 * BossBatの状態のgetter・setter
 * @return m_BossBatState
 * @brief BossBatの状態を返す・セット
 */
    public BatState GetSetBatState
    {
        get { return m_BatState; }
        set { m_BatState = value; }
    }

    /**
     * @fn
     * BossBatの向きのgetter・setter
     * @return m_BossBatDir
     * @brief BossBatの向きを返す・セット
     */
    public BatDir GetSetBatDir
    {
        get { return m_BatDir; }
        set { m_BatDir = value; }
    }

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

    public Vector3 GetSetMoveValue
    {
        get { return m_MoveValue; }
        set { m_MoveValue = value; }
    }
}
