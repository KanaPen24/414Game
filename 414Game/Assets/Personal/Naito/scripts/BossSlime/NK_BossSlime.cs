﻿/**
 * @file   NK_BossSlime.cs
 * @brief  BossSlimeのクラス
 * @author NaitoKoki
 * @date   2023/04/04
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;


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
    public IS_Player m_BSPlayer;//プレイヤー
    [SerializeField] private IS_GoalEffect goalEffect;//倒されたときに発生するエフェクト
    [SerializeField] private List<NK_BossSlimeStrategy> m_BossSlimeStrategy; // BossSlime挙動クラスの動的配列
    [SerializeField] private BossSlimeState m_BossSlimeState;      // BossSlimeの状態を管理する
    [SerializeField] private BossSlimeDir m_BossSlimeDir;        // BossSlimeの向きを管理する
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private bool m_DamageFlag;
    [SerializeField] private CubismRenderController renderController;
    [SerializeField] private float m_InvincibleTime;
    private float m_fViewX;

    private void Update()
    {
        m_fViewX = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (m_DamageFlag)
        {
            //Mathf.Absは絶対値を返す、Mathf.Sinは＋なら１，－なら0を返す
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            renderController.Opacity = level;
        }
        else renderController.Opacity = 1f;
    }

    private void FixedUpdate()
    {
        if (m_Clock.GetSetStopTime || m_fViewX >= 3)
        {
            return;
        }
        m_BossSlimeStrategy[(int)m_BossSlimeState].UpdateStrategy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //// プレイヤーだったら
        //if (collision.gameObject == m_Player.gameObject)
        //{
        //    Debug.Log("Player Damage!!");
        //    //m_Player.GetPlayerHp().DelLife(10);
        //    m_Player.Damage(10,2.0f);
        //}
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

        // プレイヤーだったら
        if (other.gameObject == m_BSPlayer.gameObject)
        {
            Debug.Log("Player Damage!!");
            //m_Player.GetPlayerHp().DelLife(10);
            m_BSPlayer.Damage(10, 2.0f);
        }

        if (other.gameObject.GetComponent<IS_WeaponHPBar>() != null)
        {
            if(m_BSPlayer.GetWeapons((int)m_BSPlayer.GetSetEquipWeaponState).GetSetAttack)
            {
                m_BSPlayer.GetWeapons((int)m_BSPlayer.GetSetEquipWeaponState).GetSetHp -= 10;
                BossSlimeDamage(5);
            }
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
    private void InvincibleEnd()
    {
        m_DamageFlag = false;
    }
    public bool GetSetDamageFlag
    {
        get { return m_DamageFlag; }
        set { m_DamageFlag = value; }
    }

    public void BossSlimeDamage(int Damage)
    {
        if (!m_DamageFlag)
        {
            m_nHP -= Damage;
            m_DamageFlag = true;
            Invoke("InvincibleEnd", m_InvincibleTime);
            // HPが0になったら、紙吹雪エフェクト発生
            if (m_nHP <= 0)
            {
                IS_AudioManager.instance.PlaySE(SEType.SE_GameClear);
                goalEffect.StartEffect();
                Destroy(this.gameObject);
            }
        }
    }
}
