/**
 * @file   NK_Slime.cs
 * @brief  Slimeのクラス
 * @author NaitoKoki
 * @date   2023/04/17
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
public enum SlimeState
{
    SlimeWait,     //待機状態
    SlimeMove,     //移動状態

    MaxSlimeState
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
    //死亡時エフェクト
    [SerializeField] private ParticleSystem m_DieEffect;
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private bool m_DamageFlag;
    [SerializeField] private CubismRenderController renderController;
    [SerializeField] private float m_InvincibleTime;
    private float m_fViewX;
    //影をアタッチする
    [SerializeField] private GameObject Shadow;

    private void Update()
    {
        m_fViewX = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (m_DamageFlag)
        {
            //Mathf.Absは絶対値を返す、Mathf.Sinは＋なら１，－なら0を返す
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            renderController.Opacity = level;
        }
        else
        {
            renderController.Opacity = 1f;
        }

        if (GetSetSlimeState == SlimeState.SlimeMove)
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
        if (m_SlimeState == SlimeState.SlimeMove)
        {
            Shadow.SetActive(false);
        }
        else
        {
            Shadow.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (m_Clock.GetSetStopTime || m_fViewX >= 3)
        {
            return;
        }

        m_SlimeStrategy[(int)m_SlimeState].UpdateStrategy();
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーだったら
        if (other.gameObject == m_Player.gameObject)
        {
            Debug.Log("Player Damage!!");
            m_Player.Damage(10, 5.0f);
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
    private void InvisbleEnd()
    {
        m_DamageFlag = false;
    }

    public bool GetSetDamageFlag
    {
        get { return m_DamageFlag; }
        set { m_DamageFlag = value; }
    }

    public void SlimeDamage(int Damage)
    {
        if (!m_DamageFlag)
        {
            m_nHP -= Damage;
            m_DamageFlag = true;
            Invoke("InvisbleEnd", m_InvincibleTime);
            // HPが0になったら、このオブジェクトを破壊
            if (m_nHP <= 0)
            {
                // SE再生
                IS_AudioManager.instance.PlaySE(SEType.SE_DeathSlime);
                // エフェクト再生
                ParticleSystem Effect = Instantiate(m_DieEffect);
                Effect.Play();
                Effect.transform.position = this.transform.position;
                Destroy(Effect.gameObject, 2.0f);

                Destroy(this.gameObject);
            }
        }
    }
}
