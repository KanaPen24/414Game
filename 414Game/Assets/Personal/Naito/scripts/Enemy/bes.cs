﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BesState
{
    BesWait,
    BesCrawl,
    BesAcid,

    MaxBesState
}

public class bes : NK_Enemy
{
    [SerializeField] private List<NK_SlimeBesStrategy> m_BesStrategy; // BossBat挙動クラスの動的配列
    [SerializeField] private BesState m_BesState;      // BossBatの状態を管理する
    [SerializeField] private EnemyDir m_BesDir;        // BossBatの向きを管理する
    //時を止めるUIをアタッチ
    [SerializeField] private YK_Clock m_Clock;
    private Rigidbody m_Rbody;
    [HideInInspector] public Vector3 m_MoveValue;
    private float m_localScalex;
    private float m_fViewX;
    [SerializeField] private float m_MoveReng;
    [SerializeField] private int m_PlayerDamage;
    //横移動
    [SerializeField] private float m_fMovePower;
    //ジャンプ力
    [SerializeField] private float m_fJumpPower;
    //死亡時エフェクト
    [SerializeField] private ParticleSystem m_DieEffect;
    //[SerializeField] private GameObject m_Hammer;
    private Animator m_Anim;
    private bool m_MoveAnimFlag;
    private bool m_AcidAnimFlag;
    private Vector3 m_gravity;
    // Start is called before the first frame update
    void Start()
    {
        m_MoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_Rbody = GetComponent<Rigidbody>();
        m_localScalex = this.transform.localScale.x;
        m_Anim = GetComponent<Animator>();
        m_Rbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_fViewX = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (!m_Clock.GetSetStopTime)
        {
            if (IS_Player.instance.transform.position.x > this.gameObject.transform.position.x)
            {
                GetSetEnemyDir = EnemyDir.Right;
                this.transform.localScale =
                 new Vector3(-m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
            else
            {
                GetSetEnemyDir = EnemyDir.Left;
                this.transform.localScale =
                 new Vector3(m_localScalex, this.transform.localScale.y, this.transform.localScale.z);
            }
        }
    }
    private void FixedUpdate()
    {
        if (m_Clock.GetSetStopTime)
        {
            return;
        }
        if (m_fViewX >= m_MoveReng)
        {
            return;
        }
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
        {
            return;
        }

        m_BesStrategy[(int)m_BesState].UpdateStrategy();

        m_Rbody.velocity = m_MoveValue;

        m_Anim.SetBool("AcidFlag", m_AcidAnimFlag);
        m_Anim.SetBool("IdouFlag", m_MoveAnimFlag);

        // -9.8fがデフォルトなのでこれを変えれば任意の重力にできる
        Vector3 gravity = new Vector3(0, -20.0f, 0);

        m_Rbody.AddForce(gravity, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == IS_Player.instance.gameObject)
        {
            Debug.Log("Player Damage!!");
            IS_Player.instance.Damage(m_PlayerDamage, 1.5f);
        }
    }

    public BesState GetSetBesState
    {
        get { return m_BesState; }
        set { m_BesState = value; }
    }

    public Vector3 GetSetMoveValue
    {
        get { return m_MoveValue; }
        set { m_MoveValue = value; }
    }

    public bool GetSetMoveAnimFlag
    {
        get { return m_MoveAnimFlag; }
        set { m_MoveAnimFlag = value; }
    }

    public bool GetSetAcidAnimFlag
    {
        get { return m_AcidAnimFlag; }
        set { m_AcidAnimFlag = value; }
    }

    public void BesDamage(int Damage)
    {
        m_nHP -= Damage;
        m_Rbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        if (GetSetEnemyDir == EnemyDir.Left)
        {
            m_Rbody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_Rbody.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            m_Rbody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_Rbody.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
        if (m_nHP <= 0)
        {
            // SE再生
            IS_AudioManager.instance.PlaySE(SEType.SE_DeathSlime);
            // エフェクト再生
            ParticleSystem Effect = Instantiate(m_DieEffect);
            Effect.Play();
            Effect.transform.position = this.transform.position;
            Destroy(Effect.gameObject, 2.0f);
            //Instantiate(m_Hammer, this.transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}
