﻿/**
 * @file   NK_BossSlime.cs
 * @brief  Slimeのクラス
 * @author NaitoKoki
 * @date   2023/04/17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================================
// BossSlimeState
// … BossSlimeの状態を管理する列挙体
// ※m_PlayerStateはこの順番になるように入れること
// ===============================================
public enum SlimeState
{
    SlimeWait,     //待機状態
    SlimeMove,     //移動状態

    MaxBossSlimeState
}

// ===============================================
// BossSlimeDir
// … BossSlimeの向きを管理する列挙体
// ===============================================
public enum SlimeDir
{
    Left, // 左向き
    Right,// 右向き

    MaxDir
}

public class NK_Slime : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    [SerializeField] private int m_nMaxHP;//敵の最大体力
    [SerializeField] private IS_Player m_Player;//プレイヤー
    //[SerializeField] private IS_GoalEffect goalEffect;//倒されたときに発生するエフェクト
    [SerializeField] private List<NK_SlimeStrategy> m_SlimeStrategy; // BossSlime挙動クラスの動的配列
    [SerializeField] private SlimeState m_SlimeState;      // BossSlimeの状態を管理する
    [SerializeField] private SlimeDir m_SlimeDir;        // BossSlimeの向きを管理する

    private void Update()
    {
        if(GetSetSlimeState == SlimeState.SlimeMove)
        {
            if(m_Player.transform.position.x > this.gameObject.transform.position.x)
            {
                GetSetSlimeDir = SlimeDir.Right;
            }
            else
            {
                GetSetSlimeDir = SlimeDir.Left;
            }
        }
    }

    private void FixedUpdate()
    {
        m_SlimeStrategy[(int)m_SlimeState].UpdateStrategy();
    }

    private void OnCollisionEnter(Collision collision)
    {
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

        // HPが0になったら、このオブジェクトを破壊
        if (m_nHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    /**
 * @fn
 * BossSlimeの状態のgetter・setter
 * @return m_BossSlimeState
 * @brief BossSlimeの状態を返す・セット
 */
    public SlimeState GetSetSlimeState
    {
        get { return m_SlimeState; }
        set { m_SlimeState = value; }
    }

    /**
     * @fn
     * BossSlimeの向きのgetter・setter
     * @return m_BossSlimeDir
     * @brief BossSlimeの向きを返す・セット
     */
    public SlimeDir GetSetSlimeDir
    {
        get { return m_SlimeDir; }
        set { m_SlimeDir = value; }
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