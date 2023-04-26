/**
 * @file   NK_BossSlime.cs
 * @brief  BossSlimeのクラス
 * @author NaitoKoki
 * @date   2023/04/04
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// BossSlimeState
// … BossSlimeの状態を管理する列挙体
// ※m_PlayerStateはこの順番になるように入れること
// ===============================================
public enum BossSlimeState
{
    BossSlimeWait,     //待機状態
    BossSlimeSummon,   //召喚攻撃状態
    BossSlimeMartial,  //近接攻撃状態

    MaxBossSlimeState
}

// ===============================================
// BossSlimeDir
// … BossSlimeの向きを管理する列挙体
// ===============================================
public enum BossSlimeDir
{
    Left, // 左向き
    Right,// 右向き

    MaxDir
}

public class NK_BossSlime : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    [SerializeField] private int m_nMaxHP;//敵の最大体力
    [SerializeField] private IS_Player m_Player;//プレイヤー
    [SerializeField] private IS_GoalEffect goalEffect;//倒されたときに発生するエフェクト
    [SerializeField] private List<NK_BossSlimeStrategy> m_BossSlimeStrategy; // BossSlime挙動クラスの動的配列
    [SerializeField] private BossSlimeState m_BossSlimeState;      // BossSlimeの状態を管理する
    [SerializeField] private BossSlimeDir m_BossSlimeDir;        // BossSlimeの向きを管理する
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;

    private void FixedUpdate()
    {
        if (m_Clock.GetSetStopTime)
        {
            return;
        }
        m_BossSlimeStrategy[(int)m_BossSlimeState].UpdateStrategy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーだったら
        if (collision.gameObject == m_Player.gameObject)
        {
            Debug.Log("Player Damage!!");
            //m_Player.GetPlayerHp().DelLife(10);
            m_Player.Damage(10,2.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 武器だったら
        //if (other.gameObject.tag == "Weapon")
        //{
        //    Debug.Log("Enemy Damage!!");
        //    //m_HpBarHP.DelLife(10);
        //    m_nHP -= 5;
        //}

        if(other.gameObject.GetComponent<IS_WeaponHPBar>() != null)
        {
            if(m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetAttack)
            {
                m_nHP -= 5;
                m_Player.GetWeapons((int)m_Player.GetSetEquipWeaponState).GetSetHp -= 10;
            }
        }

        // HPが0になったら、紙吹雪エフェクト発生
        if (m_nHP <= 0)
        {
            IS_AudioManager.instance.PlaySE(SEType.SE_GameClear);
            goalEffect.StartEffect();
            Destroy(this.gameObject);
        }
    }

    /**
 * @fn
 * BossSlimeの状態のgetter・setter
 * @return m_BossSlimeState
 * @brief BossSlimeの状態を返す・セット
 */
    public BossSlimeState GetSetBossSlimeState
    {
        get { return m_BossSlimeState; }
        set { m_BossSlimeState = value; }
    }

    /**
     * @fn
     * BossSlimeの向きのgetter・setter
     * @return m_BossSlimeDir
     * @brief BossSlimeの向きを返す・セット
     */
    public BossSlimeDir GetSetBossSlimeDir
    {
        get { return m_BossSlimeDir; }
        set { m_BossSlimeDir = value; }
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
}
