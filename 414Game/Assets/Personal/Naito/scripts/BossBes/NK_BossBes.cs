/**
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
public enum BossBesState
{
    BossBesWait,     //待機状態
    BossBesSummon,   //召喚攻撃状態
    BossBesMartial,  //近接攻撃状態
    BossBesUp,
    BossBesFlight,
    BossBesFall,

    MaxBossBesState
}

// ===============================================
// BossSlimeDir
// … BossSlimeの向きを管理する列挙体
// ===============================================
public enum BossBesDir
{
    Left, // 左向き
    Right,// 右向き

    MaxDir
}

public class NK_BossBes : MonoBehaviour
{
    //敵の体力
    [SerializeField] private int m_nHP;
    [SerializeField] private int m_nMaxHP;//敵の最大体力
    [SerializeField] public IS_Player m_BBPlayer;//プレイヤー
    [SerializeField] private IS_GoalEffect goalEffect;//倒されたときに発生するエフェクト
    [SerializeField] private List<NK_BossBesStrategy> m_BossSlimeStrategy; // BossSlime挙動クラスの動的配列
    [SerializeField] private BossBesState m_BossBesState;      // BossSlimeの状態を管理する
    [SerializeField] private BossBesDir m_BossBesDir;        // BossSlimeの向きを管理する
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private bool m_DamageFlag;
    [SerializeField] private CubismRenderController renderController;
    [SerializeField] private float m_InvincibleTime;
    private float m_fViewX;
    [SerializeField] private YK_Goal goal;
    private Rigidbody m_Rbody;
    [HideInInspector] public Vector3 m_BBMoveValue;
    private float m_localScalex;
    private bool m_MAnimFlag;   //近接攻撃アニメフラグ
    private bool m_SAnimFlag;   //召喚アニメフラグ
    [SerializeField] private NK_BossSlime_Aera m_Area;
    private Animator m_Anim;
    [SerializeField] private int m_PlayerDamage;
    private bool m_ClockFlag;

    private void Start()
    {
        m_BBMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_Rbody = GetComponent<Rigidbody>();
        m_localScalex = this.transform.localScale.x;
        m_BossBesState = BossBesState.BossBesFall;
        m_Anim = GetComponent<Animator>();
    }

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

        if (!m_ClockFlag)
        {
            if (m_BBPlayer.transform.position.x > this.gameObject.transform.position.x)
            {
                GetSetBossBesDir = BossBesDir.Right;
                this.transform.localScale =
                    new Vector3(-m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
            else
            {
                GetSetBossBesDir = BossBesDir.Left;
                this.transform.localScale =
                    new Vector3(m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_Clock.GetSetStopTime)
        {
            m_ClockFlag = true;
            m_Anim.SetFloat("Moving", 0.0f);
            return;
        }
        else
        {
            m_ClockFlag = false;
            m_Anim.SetFloat("Moving", 1.0f);
        }
        if (m_fViewX >= 3)
        {
            return;
        }
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
        {
            return;
        }
        m_BossSlimeStrategy[(int)m_BossBesState].UpdateStrategy();

        m_Rbody.velocity = m_BBMoveValue;
        m_Anim.SetBool("MartialFlag", m_MAnimFlag);
        m_Anim.SetBool("SummonFlag", m_SAnimFlag);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーだったら
        if (other.gameObject == m_BBPlayer.gameObject)
        {
            Debug.Log("Player Damage!!");
            m_BBPlayer.Damage(m_PlayerDamage, 2.0f);
        }
    }

    /**
 * @fn
 * BossSlimeの状態のgetter・setter
 * @return m_BossSlimeState
 * @brief BossSlimeの状態を返す・セット
 */
    public BossBesState GetSetBossBesState
    {
        get { return m_BossBesState; }
        set { m_BossBesState = value; }
    }

    /**
     * @fn
     * BossSlimeの向きのgetter・setter
     * @return m_BossSlimeDir
     * @brief BossSlimeの向きを返す・セット
     */
    public BossBesDir GetSetBossBesDir
    {
        get { return m_BossBesDir; }
        set { m_BossBesDir = value; }
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

    public bool GetSetMAnimFlag
    {
        get { return m_MAnimFlag; }
        set { m_MAnimFlag = value; }
    }

    public bool GetSetSAnimFlag
    {
        get { return m_SAnimFlag; }
        set { m_SAnimFlag = value; }
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
                goal.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
